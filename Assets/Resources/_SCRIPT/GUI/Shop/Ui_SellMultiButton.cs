using System;
using System.Collections;
using System.Collections.Generic;
using Shop;
using Shop_Action;
using Shop_UI;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Shop_UI
{
    public class UI_SellMultiButton : MonoBehaviour
    {
        private Button yourButton;
        private IInventory UI_Shop;
        private UI_ShopSellController sellController;

        void Awake()
        {
            yourButton = GetComponent<Button>();
            sellController = FindObjectOfType<UI_ShopSellController>();
        }
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            UI_Shop = FindObjectOfType<UI_SellShopSO>();
        }

        void TaskOnClick()
        {
            if (UI_Shop.currentlySelectedIndex == -1)
                return;

            try
            {
                sellController.inventoryData.GetItemAt(UI_Shop.currentlySelectedIndex);
            }
            catch
            {
                return;
            }


            UI_ShopAction.ShowSellItemUI.Invoke();
        }
    }
}