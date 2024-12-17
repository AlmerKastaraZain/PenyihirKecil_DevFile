using System;
using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Inventory_Models;
using Inventory_UI;
using Shop_UI;
using UI;
using UnityEngine;


namespace Crafting_UI
{
    public class UI_CraftingPanel : IInventory
    {
        private UI_Crafting craftingInv;

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
            craftingInv = FindAnyObjectByType<UI_Crafting>();
        }

        public void InitializeCraftingUI()
        {
            for (int i = 0; i < 3; i++)
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

            Debug.Log(listOfUIItems.IndexOf(inventoryItemUI));
            int index = listOfUIItems.IndexOf(inventoryItemUI);

            if (currentlyDraggedItemIndex == index)
                return;

            string ComesFrom = (currentlyDraggedItemIndex != -1) ? "Crafting" : "Inventory";

            if (ComesFrom == "Crafting")
            {
                index = listOfUIItems.IndexOf(inventoryItemUI);

                if (index == -1)
                {
                    return;
                };

                if (!controller.craftingData.inventoryItems[index].isEmpty)
                {
                    if (controller.craftingData.inventoryItems[currentlyDraggedItemIndex].item.ID == controller.craftingData.inventoryItems[index].item.ID)
                    {
                        Debug.Log("AAA");
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

                if (!controller.craftingData.inventoryItems[index].isEmpty)
                {
                    if (controller.inventoryData.inventoryItems[craftingInv.currentlyDraggedItemIndex].item.ID == controller.craftingData.inventoryItems[index].item.ID)
                    {
                        OnStackDifferentInventoryItem.Invoke(craftingInv.currentlyDraggedItemIndex, index);
                        return;
                    }
                }
                // inventory                // craft Index
                OnSwapDifferentItem?.Invoke(craftingInv.currentlyDraggedItemIndex, index);
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
            craftingInv.currentlySelectedIndex = -1;
            clickedItem.Invoke();
        }
    }
}