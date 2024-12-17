using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory_Models;
using Inventory_UI;
using Shop_UI;
using UI;
using UnityEngine;

namespace Shop_UI
{
    public class UI_SellShopSO : IInventory
    {
        new public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        [SerializeField] public InventoryController controller;
        new public event Action clickedItem;

        new public event Action<int, int> OnSwapItem;


        private void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        new public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UI_InventoryItem uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
            }
        }

        new private void HandleSwap(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            };
            if (!controller.inventoryData.inventoryItems[index].isEmpty)
            {
                if (controller.inventoryData.inventoryItems[currentlyDraggedItemIndex].item.name == controller.inventoryData.inventoryItems[index].item.name)
                {
                    HandleStacking(currentlyDraggedItemIndex, index);
                }
            }
            OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        new internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        new private void HandleItemSelection(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            currentlySelectedIndex = index;
            if (controller.inventoryData.inventoryItems[index].isEmpty)
            {
                currentlySelectedIndex = -1;
            }
            clickedItem.Invoke();
        }
    }
}