using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using Shop.Models;
using Shop_Action;
using UnityEngine;
using UnityEngine.UI;

namespace Shop_UI
{
    public class UI_ShopSellButton : MonoBehaviour
    {
        public Button yourButton;
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            if (GameController.state == GameState.ShoppingSell)
                return;
            UI_ShopAction.GoToSellShop.Invoke();
        }
    }
}