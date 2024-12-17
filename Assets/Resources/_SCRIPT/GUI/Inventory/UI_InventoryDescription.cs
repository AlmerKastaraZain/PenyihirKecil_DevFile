using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_UI
{
    public class UI_InventoryDescription : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text Description;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            this.itemImage.sprite = null;
            this.title.text = "";
            this.Description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName,
            string itemDescription)
        {
            this.itemImage.sprite = sprite;
            this.title.text = itemName;
            this.Description.text = itemDescription;
        }


    }
}