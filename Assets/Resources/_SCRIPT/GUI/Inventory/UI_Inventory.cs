using System;
using System.Collections;
using System.Collections.Generic;
using Shop_UI;
using UnityEngine;
using UI;
using Inventory;

namespace Inventory_UI
{
    public class UI_Inventory : IInventory
    {
        new public event Action clickedItem;
        new public event Action<int, int> OnSwapItem;

        [SerializeField] public InventoryController controller;

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


        new internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        private void Start()
        {
            UI_InventoryAction.ShowInventory += () =>
            {
                itemDescription.ResetDescription();
                ResetSelection();
            };
            UI_InventoryAction.HideInventory += () =>
            {
                ResetDraggedItem();
            };
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        new private void HandleSwap(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);

            Debug.Log(index);

            index = listOfUIItems.IndexOf(inventoryItemUI);

            if (index == -1)
            {
                return;
            };
            if (!controller.inventoryData.inventoryItems[index].isEmpty)
            {
                if (controller.inventoryData.inventoryItems[currentlyDraggedItemIndex].item.name == controller.inventoryData.inventoryItems[index].item.name)
                {
                    HandleStacking(currentlyDraggedItemIndex, index);
                    return;
                }
            }
            OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        new public void ResetSelection()
        {
            actionPanel.Toggle(false);
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        new public void AddSelection()
        {
            actionPanel.Toggle(false);

            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        new public void DeselectAllItems()
        {
            actionPanel.Toggle(false);

            foreach (UI_InventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }


        new private void HandleItemSelection(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            currentlySelectedIndex = index;
            if (controller.inventoryData.inventoryItems[currentlySelectedIndex].isEmpty)
            {
                currentlySelectedIndex = -1;
            }

            clickedItem.Invoke();
        }
    }
}