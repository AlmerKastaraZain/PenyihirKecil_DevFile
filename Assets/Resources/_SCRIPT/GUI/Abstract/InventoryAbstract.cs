using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory_Models;
using Inventory_UI;
using Shop_Action;
using Shop_UI;
using UnityEngine;
using UnityEngine.Windows;

namespace UI
{
    public class IInventory : MonoBehaviour
    {

        [SerializeField] public UI_InventoryItem itemPrefab;
        [SerializeField] public RectTransform contentPanel;
        [SerializeField] public UI_InventoryDescription itemDescription;
        [SerializeField] public MouseFollower mouseFollower;

        [SerializeField] public ItemActionPanel actionPanel;
        virtual public event Action clickedItem;

        public List<UI_InventoryItem> listOfUIItems = new List<UI_InventoryItem>();
        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested,
            OnStartDragging;

        public event Action<int, int> OnSwapItem;
        public event Action<int, int> OnStackItem;

        public int currentlyDraggedItemIndex = -1;
        public int currentlySelectedIndex = -1;
        [SerializeField] private InventorySO inventoryData;

        private void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorySize)
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
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }


        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void DeselectAllItems()
        {
            foreach (UI_InventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }


        public void HandleShowItemActions(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }

            OnItemActionRequested?.Invoke(index);
        }

        public void HandleItemStack()
        {

        }

        public void HandleEndDrag(UI_InventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }

        public void HandleSwap(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);

            if (index == -1)
            {
                return;
            };

            if (currentlyDraggedItemIndex == index)
                return;

            if (!inventoryData.inventoryItems[index].isEmpty)
            {
                if (inventoryData.inventoryItems[currentlyDraggedItemIndex].item.ID == inventoryData.inventoryItems[index].item.ID)
                {
                    HandleStacking(currentlyDraggedItemIndex, index);
                    return;
                }
            }
            OnSwapItem?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        public void HandleStacking(int index, int index2)
        {
            Debug.Log(index);
            Debug.Log(index2);
            OnStackItem?.Invoke(index, index2);
        }

        public void HandleBeginDrag(UI_InventoryItem inventoryItemUI)
        {

            int index = listOfUIItems.IndexOf(inventoryItemUI);
            HideAction();
            if (index == -1) return;
            currentlyDraggedItemIndex = index;

            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }


        public void HandleItemSelection(UI_InventoryItem inventoryItemUI)
        {
            HideAction();
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;

            currentlySelectedIndex = index;
            OnDescriptionRequested?.Invoke(index);
            clickedItem.Invoke();
        }

        public void HideAction()
        {
            actionPanel.Toggle(false);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

    }
}