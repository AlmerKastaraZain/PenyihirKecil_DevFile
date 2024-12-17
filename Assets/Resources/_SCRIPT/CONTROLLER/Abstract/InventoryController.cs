using Inventory_UI;
using Inventory_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Unity.VisualScripting;
using Shop_UI;
using UI;
using Name;

namespace Inventory
{
    public abstract class InventoryController : MonoBehaviour
    {
        [SerializeField] public IInventory inventoryUI;
        [SerializeField] public InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField] public InputItemDivide inputItemDivide;


        private void Start()
        {
            PrepareInventoryData();
            inventoryUI.OnStackItem += HandleStacking;
        }


        //OnStackItem
        public void PrepareInventoryData()
        {
            inventoryData.Initalize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.isEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        public void ShowDivideInput()
        {
            inputItemDivide.gameObject.SetActive(true);
        }

        public void HandleStacking(int index1, int index2)
        {
            InventoryItem inventoryItem = inventoryData.inventoryItems[index1];
            inventoryData.RemoveItem(index1, inventoryData.inventoryItems[index1].quantity);
            inventoryData.HandleItemStack(inventoryItem, index2);
        }

        public void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.itemImage,
                    item.Value.quantity);
            }
        }

        public void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.itemImage, inventoryItem.quantity);
        }

        public void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        public void HandleUpdate()
        {

        }

        public void CheckForUserInputForDivide()
        {
            if (Input.GetKey(KeyCode.LeftShift) && inventoryUI.currentlySelectedIndex != -1)
            {
                inputItemDivide.InsertController(gameObject.GetComponent<InventoryController>(), inventoryUI);
                InputItemDivide.ShowDivideInput.Invoke();
            }
        }

        public void AddStackableItemToEmptySlot(InventoryItem inventoryItem, int quantity)
        {
            inventoryData.AddItemWithoutStacking(inventoryItem, quantity);
        }
    }
}