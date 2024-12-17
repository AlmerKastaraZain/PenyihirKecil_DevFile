using System;
using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Crafting_UI;
using Shop_Action;
using Shop_UI;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace Shop_UI
{
    public class UI_CraftingBrewMultiButton : MonoBehaviour
    {
        private Button yourButton;
        private UI_CraftingController controller;

        void Awake()
        {
            yourButton = GetComponent<Button>();
            controller = FindObjectOfType<UI_CraftingController>();
        }
        void Start()
        {
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            if (controller.craftingData.inventoryItems[0].isEmpty || controller.craftingData.inventoryItems[1].isEmpty || controller.craftingData.inventoryItems[2].isEmpty)
            {
                return;
            }

            UI_CraftingAction.ShowCraftingItemUI?.Invoke();
        }
    }
}