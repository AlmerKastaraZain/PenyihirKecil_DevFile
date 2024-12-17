using System;
using System.Collections;
using System.Collections.Generic;
using Shop_Action;
using Shop;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Shop_UI
{
    public class UI_BuyMultiButton : MonoBehaviour
    {
        private Button yourButton;
        private UI_BuyShopSO UI_Shop;
        private UI_ShopBuyController buyController;

        void Awake()
        {
            yourButton = GetComponent<Button>();
            buyController = FindObjectOfType<UI_ShopBuyController>();
        }
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            UI_Shop = FindObjectOfType<UI_BuyShopSO>();
        }

        void TaskOnClick()
        {
            if (UI_Shop.currentlySelectedIndex == -1)
                return;

            try
            {
                buyController.inventoryData.GetItemAt(UI_Shop.currentlySelectedIndex);
            }
            catch
            {
                return;
            }


            UI_ShopAction.ShowBuyItemUI.Invoke();
        }
    }
}