using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory_Models;
using Inventory_UI;
using NPC_Models;
using Shop.Models;
using Shop_Action;
using Unity.VisualScripting;
using UnityEngine;
using UI;
using Shop_UI;
using Money_UI;
using Money_Models;

namespace Shop
{

    public class UI_ShopBuyController : InventoryController
    {

        [SerializeField] private InventorySO playerInventory;
        [SerializeField] private UI_InputItemBuy UIInputItemBuy;
        [SerializeField]
        public UI_BuyShopSO UI_Shop;
        [SerializeField] public GameObject SellShop;
        [SerializeField] public GameObject BuyShop;
        [SerializeField] private UI_Money UIMoney;
        [SerializeField] private MoneySO money;

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();

            UI_ShopAction.GoToBuyShop += ShowBuyShop;

            UI_ShopAction.BuyItem += (int index) =>
            {
                BuyItem(index);
            };

            UI_ShopAction.ShowBuyItemUI += ShowItemUI;
            UI_ShopAction.HideSellItemUI += HideItemUI;


            inventoryUI.clickedItem += UpdateMoneyUI;
            inventoryUI.clickedItem += CheckForUserInputForDivide;
        }

        private void UpdateMoneyUI()
        {
            if (inventoryData.inventoryItems[inventoryUI.currentlySelectedIndex].isEmpty)
            {
                UIMoney.UpdateUI(0);
                return;
            }

            UIMoney.UpdateUI(inventoryData.GetItemAt(inventoryUI.currentlySelectedIndex).item.ItemMarketValue);
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.itemImage, item.name, item.Description);
        }


        private void ShowItemUI()
        {
            UIInputItemBuy.gameObject.SetActive(true);
        }

        private void HideItemUI()
        {
            UIInputItemBuy.gameObject.SetActive(false);
        }

        public void BuyMultipleItems(int index, int val)
        {
            if (playerInventory.IsInventoryFull())
                return;

            playerInventory.AddItem(inventoryData.GetItemAt(index).item, val);
            money.SubtractMoney(inventoryData.GetItemAt(index).item.ItemMarketValue * val);
            inventoryData.RemoveItem(index, val);

            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }

        public void BuyItem(int index)
        {
            if (playerInventory.IsInventoryFull() || money.GetMoney() < inventoryData.GetItemAt(index).item.ItemMarketValue)
                return;

            playerInventory.AddOneItem(inventoryData.GetItemAt(index).item);
            money.SubtractMoney(inventoryData.GetItemAt(index).item.ItemMarketValue);
            inventoryData.RemoveItem(index, 1);

            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }

        public void InsertNewInventoryData(InventorySO Model)
        {
            Debug.Log("Inserted");
            Debug.Log(Model);

            inventoryData.inventoryItems = Model.inventoryItems;


            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }


        public void RefreshInventoryDataStock(ShopInventoryDataSO Model)
        {
            Debug.Log("Inserted");
            inventoryData.inventoryItems = Model.TraderStock;

            UI_Shop.ResetSelection();
            UI_Shop.ResetAllItems();
            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }

        private void ShowBuyShop()
        {
            Debug.Log("1");
            SellShop.gameObject.SetActive(false);
            BuyShop.gameObject.SetActive(true);
            UI_Shop.ResetSelection();
            UIMoney.UpdateUI(0);

            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                UI_Shop.UpdateData(item.Key,
                    item.Value.item.itemImage,
                    item.Value.quantity);
            }
        }

        new public void HandleUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SellShop.SetActive(false);
                BuyShop.SetActive(false);
                UI_Shop.ResetDraggedItem();
                UI_ShopAction.ShopToFreeRoam.Invoke();
            }
        }

        new private void CheckForUserInputForDivide()
        {
            if (Input.GetKey(KeyCode.LeftShift) && inventoryUI.currentlySelectedIndex != -1)
            {
                inputItemDivide.InsertController(gameObject.GetComponent<InventoryController>(), inventoryUI);
                inputItemDivide.gameObject.SetActive(true);
            }
        }
    }
}
