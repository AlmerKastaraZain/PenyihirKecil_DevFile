using Inventory_UI;
using Inventory_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Unity.VisualScripting;
using Money_UI;

namespace Inventory
{
    public class UI_InventoryController : InventoryController
    {
        [SerializeField]
        private Item itemPrefab;
        [SerializeField]
        private UI_Inventory inventoryUIActions;
        [SerializeField] private UI_Money UIMoney;
        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();

            inventoryUI.clickedItem += CheckForUserInputForDivide;

            inventoryUI.clickedItem += UpdateMoneyUI;
            inventoryUIActions.OnStackItem += HandleStacking;

        }

        private void UpdateMoneyUI()
        {
            if (inventoryData.inventoryItems[inventoryUI.currentlySelectedIndex].isEmpty)
            {
                UIMoney.UpdateUI(0);
                return;
            }

            Debug.Log("ta");
            UIMoney.UpdateUI(inventoryData.GetItemAt(inventoryUIActions.currentlySelectedIndex).item.ItemMarketValue);
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItem += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        public void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (itemAction != null || destroyableItem != null)
                inventoryUIActions.ShowItemAction(itemIndex);


            if (itemAction != null)
            {
                inventoryUIActions.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            if (destroyableItem != null)
            {
                inventoryUIActions.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryData.GetItemAt(itemIndex).isEmpty)
                    inventoryUIActions.ResetSelection();
            }
        }


        private void DropItem(int itemIndex, int quantity)
        {
            if (inventoryData.GetItemAt(itemIndex).isEmpty)
                return;

            GameObject player = this.gameObject;
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            Item item = Instantiate(itemPrefab, playerPos, Quaternion.identity);
            InventoryItem itemData = inventoryData.GetItemAt(itemIndex);
            item.Initialize(itemData);

            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUIActions.ResetSelection();
            audioSource.PlayOneShot(dropClip);
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
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    UI_InventoryAction.ShowInventory.Invoke();
                    inventoryUIActions.gameObject.SetActive(true);
                    inventoryUIActions.ResetSelection();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUIActions.UpdateData(item.Key,
                            item.Value.item.itemImage,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUIActions.HideAction();
                    inventoryUIActions.gameObject.SetActive(false);
                    inventoryUIActions.ResetDraggedItem();
                    UI_InventoryAction.HideInventory.Invoke();

                    UIMoney.UpdateUI(0);
                }
            }
        }
        new private void CheckForUserInputForDivide()
        {
            if (Input.GetKey(KeyCode.LeftShift) && inventoryUI.currentlySelectedIndex != -1)
            {
                inputItemDivide.InsertController(gameObject.GetComponent<InventoryController>(), inventoryUI);
                inputItemDivide.gameObject.SetActive(true);
            }
            Debug.Log("Hello");
        }
    }
}