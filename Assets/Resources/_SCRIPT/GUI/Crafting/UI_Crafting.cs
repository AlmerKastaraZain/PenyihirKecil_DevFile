using System;
using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Inventory_UI;
using UI;
using UnityEngine;
using UnityEngine.Windows;


namespace Crafting_UI
{
    public class UI_Crafting : IInventory
    {
        private UI_CraftingPanel craftingPanel;

        [SerializeField] private UI_CraftingController controller;

        new public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        new public event Action<int, int> OnSwapItem;
        public event Action<int, int> OnSwapDifferentItem;
        new public event Action clickedItem;
        public event Action<int, int> OnStackDifferentInventoryItem;


        private void Awake()
        {
            mouseFollower.Toggle(false);
            craftingPanel = FindObjectOfType<UI_CraftingPanel>();
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

        private void Start()
        {
            UI_CraftingAction.GoToCrafting += () =>
            {
                ResetSelection();
            };
            UI_CraftingAction.GoToCraftingJournal += () =>
            {
                ResetSelection();
            };
            UI_CraftingAction.CraftingToFreeRoam += () =>
            {
                ResetDraggedItem();
            };
        }


        //private void HandleShowItemActions(UI_InventoryItem inventoryItemUI)
        //{
        //    int index = listOfUIItems.IndexOf(inventoryItemUI);
        //    if (index == -1)
        //    {
        //        return;
        //    }
        //
        //    OnItemActionRequested?.Invoke(index);
        //}

        new private void HandleSwap(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);

            if (currentlyDraggedItemIndex == index)
                return;

            string ComesFrom = (currentlyDraggedItemIndex != -1) ? "Inventory" : "Crafting";

            if (ComesFrom == "Inventory")
            {
                index = listOfUIItems.IndexOf(inventoryItemUI);

                if (index == -1)
                {
                    return;
                };

                if (!controller.inventoryData.inventoryItems[index].isEmpty)
                {
                    if (controller.inventoryData.inventoryItems[currentlyDraggedItemIndex].item.ID == controller.inventoryData.inventoryItems[index].item.ID)
                    {
                        HandleStacking(currentlyDraggedItemIndex, index);
                        return;
                    }
                }

                OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
                HandleItemSelection(inventoryItemUI);
            }
            else
            {
                index = listOfUIItems.IndexOf(inventoryItemUI);

                if (index == -1)
                {
                    return;
                };

                if (!controller.inventoryData.inventoryItems[index].isEmpty)
                {
                    if (controller.craftingData.inventoryItems[craftingPanel.currentlyDraggedItemIndex].item.ID == controller.inventoryData.inventoryItems[index].item.ID)
                    {
                        OnStackDifferentInventoryItem.Invoke(craftingPanel.currentlyDraggedItemIndex, index);
                        return;
                    }
                }

                //inventory                // craft Index
                OnSwapDifferentItem?.Invoke(index, craftingPanel.currentlyDraggedItemIndex);
            }
        }


        new private void HandleBeginDrag(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            currentlyDraggedItemIndex = index;

            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        new private void HandleItemSelection(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            currentlySelectedIndex = index;
            if (index == -1) return;
            craftingPanel.currentlySelectedIndex = -1;
            clickedItem.Invoke();
        }
    }
}