using System;
using Crafting_Action;
using Crafting_Models;
using Inventory_Models;
using Shop_Action;
using Shop_UI;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Crafting_UI
{
    public class UI_InputItemCraft : IInputField
    {
        [SerializeField] public CraftingSO craftingData;
        [SerializeField] public ListOfCraftableSO listOfCraftable;
        [SerializeField] public UI_CraftingController controller;
        private int craftingLimit = 0;
        private void Awake()
        {
            button.onClick.AddListener(OnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void Start()
        {
            UI_CraftingAction.ShowCraftingItemUI += Show;
            UI_CraftingAction.HideCraftingItemUI += Hide;
        }

        new private void Show()
        {
            input.gameObject.SetActive(true);
            SetQuantityLimit();
        }

        private void SetQuantityLimit()
        {
            if (listOfCraftable.CheckIfCraftingDataIsInvalid(craftingData.inventoryItems[0].item, craftingData.inventoryItems[1].item, craftingData.inventoryItems[2].item))
            {
                craftingLimit = 0;
                return;
            }

            InventoryItem item = craftingData.inventoryItems[0];
            for (int i = 1; i < 3; i++)
            {
                if (Mathf.Min(item.quantity, craftingData.inventoryItems[i].quantity) == item.quantity)
                    continue;

                item = craftingData.inventoryItems[i];
            };

            craftingLimit = item.quantity;
        }


        override public void OnEndEdit(string text)
        {
            if (text.Length <= 0) return;
            if (Convert.ToInt32(text) > craftingLimit)
            {
                Debug.Log("Value exceded quantity");
                inputField.text = Convert.ToString(craftingLimit);
            }
        }


        override public void OnClick()
        {
            if (inputField.text.Length <= 0) return;

            controller.BrewMultiplePotions(Convert.ToInt32(inputField.text));
            UI_CraftingAction.HideCraftingItemUI.Invoke();
        }
    }
}
