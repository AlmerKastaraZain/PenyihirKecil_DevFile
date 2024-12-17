using System;
using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Crafting_Models;
using Inventory_UI;
using UI;
using UnityEngine;
using UnityEngine.Windows;


namespace Crafting_UI
{
    public class UI_CraftingJournal : MonoBehaviour
    {

        [SerializeField] public UI_InventoryItem itemPrefab;
        [SerializeField] public RectTransform contentPanel;
        [SerializeField] public UI_InventoryDescription itemDescription;
        virtual public event Action clickedItem;

        public List<UI_InventoryItem> listOfUIItems = new List<UI_InventoryItem>();
        public event Action<int> OnDescriptionRequested;
        public int currentlySelectedIndex = -1;

        private void Awake()
        {
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

        public void HandleItemSelection(UI_InventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            currentlySelectedIndex = index;
            OnDescriptionRequested?.Invoke(index);
            clickedItem.Invoke();
        }

    }
}