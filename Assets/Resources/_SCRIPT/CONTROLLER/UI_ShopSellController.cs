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

    public class UI_ShopSellController : InventoryController
    {
        [SerializeField] private UI_InputItemSell UIInputItemSell;
        [SerializeField] private UI_ShopBuyController shopController;
        [SerializeField]
        public UI_SellShopSO UI_Shop;
        [SerializeField] public GameObject SellShop;
        [SerializeField] public GameObject BuyShop;
        [SerializeField] private UI_Money UIMoney;
        [SerializeField] private MoneySO money;

        private void Start()
        {
            shopController = FindObjectOfType<UI_ShopBuyController>();
            PrepareUI();
            PrepareInventoryData();
            UI_ShopAction.GoToSellShop += ShowSellShop;
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;


            UI_ShopAction.SellItem += (int index) =>
            {
                SellItem(index);
            };


            UI_ShopAction.GoToSellShop += ShowSellShop;

            UI_ShopAction.ShowSellItemUI += ShowItemToggle;
            UI_ShopAction.HideSellItemUI += HideItemToggle;


            inventoryUI.clickedItem += CheckForUserInputForDivide;
            inventoryUI.clickedItem += UpdateMoneyUI;
            inventoryUI.OnStackItem += HandleStacking;

        }

        private void UpdateMoneyUI()
        {
            if (inventoryData.inventoryItems[inventoryUI.currentlySelectedIndex].isEmpty)
            {
                UIMoney.UpdateUI(0);
                return;
            }

            UIMoney.UpdateUI(inventoryData.GetItemAt(UI_Shop.currentlySelectedIndex).item.ItemMarketValue);
        }

        public void SellMultipleItems(int index, int val)
        {
            if (shopController.inventoryData.IsInventoryFull())
                return;

            shopController.inventoryData.AddItem(inventoryData.GetItemAt(index).item, val);
            money.AddMoney(inventoryData.GetItemAt(index).item.ItemMarketValue * val);
            inventoryData.RemoveItem(index, val);
            //TODO: Give money to player

            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }

        public void SellItem(int index)
        {
            shopController.inventoryData.AddOneItem(inventoryData.GetItemAt(index).item);
            money.AddMoney(inventoryData.GetItemAt(index).item.ItemMarketValue);
            inventoryData.RemoveItem(index, 1);

            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());

            //TODO: Give money to player
        }

        new public void HandleStacking(int index1, int index2)
        {
            InventoryItem inventoryItem = inventoryData.inventoryItems[index1];
            inventoryData.RemoveItem(index1, inventoryData.inventoryItems[index1].quantity);
            inventoryData.AddItemAt(inventoryItem, index2);
        }

        private void ShowItemToggle()
        {
            UIInputItemSell.gameObject.SetActive(true);
        }

        private void HideItemToggle()
        {
            UIInputItemSell.gameObject.SetActive(false);
        }
        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnSwapItem += HandleSwapItems;
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnStartDragging += HandleDragging;
        }

        private void ShowSellShop()
        {
            SellShop.gameObject.SetActive(true);
            BuyShop.gameObject.SetActive(false);
            UIMoney.UpdateUI(0);

            UI_Shop.ResetSelection();
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                UI_Shop.UpdateData(item.Key,
                    item.Value.item.itemImage,
                    item.Value.quantity);
            }
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