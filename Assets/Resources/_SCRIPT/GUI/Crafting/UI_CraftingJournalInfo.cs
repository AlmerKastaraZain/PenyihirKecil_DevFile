using System;
using System.Collections.Generic;
using Crafting_Models;
using Inventory_Models;
using Inventory_UI;
using Money_UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting_UI
{
    public class UI_CraftingJournalInfo : MonoBehaviour
    {

        [SerializeField] public UI_InventoryItem itemPrefab;
        [SerializeField] public RectTransform contentPanel;
        [SerializeField] public CraftingJournalSO craftingJournal;

        public List<UI_InventoryItem> listOfUIItems = new List<UI_InventoryItem>();
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI effectText;


        public void InitializeJournalUI()
        {
            for (int i = 0; i < 3; i++)
            {
                UI_InventoryItem uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);
            }
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }


        public void UpdateJournalInfo(int index)
        {
            image.sprite = craftingJournal.inventoryItems[index].item.itemImage;
            title.text = craftingJournal.inventoryItems[index].item.Name;
            descriptionText.text = craftingJournal.inventoryItems[index].item.Description;
            effectText.text = craftingJournal.inventoryItems[index].item.Effect;
        }
    }
}
