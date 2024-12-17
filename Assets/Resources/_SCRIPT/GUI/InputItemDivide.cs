using System;
using Crafting_UI;
using Inventory;
using Shop_UI;
using UI;
using UnityEngine;

namespace Name
{
    public class InputItemDivide : IInputField
    {
        private InventoryController Controller;
        [SerializeField] private UI_CraftingPanel craftingPanel;
        [SerializeField] private UI_CraftingController craftingController;
        public static Action ShowDivideInput;
        public static Action HideDivideInput;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        public void InsertController(InventoryController controller, IInventory inventory)
        {
            Controller = controller;
            UIInventory = inventory;
        }

        private void Start()
        {
            ShowDivideInput += Show;
            HideDivideInput += Hide;
        }


        //SAYA SUKA MAKAN. MAKAN ITU ENAK. ADJAJDHAJKFKJASGFKGASKHFGADSKHGFYAEBGEWUQVERSKUVKUERVK
        override public void OnEndEdit(string text)
        {
            if (text.Length <= 0) return;

            if (UIInventory.currentlySelectedIndex == -1)
            {
                if (Convert.ToInt32(text) > craftingController.craftingData.GetItemAt(craftingPanel.currentlySelectedIndex).quantity)
                {
                    Debug.Log("Valueess exceded quantity");
                    inputField.text = Convert.ToString(craftingController.craftingData.GetItemAt(craftingPanel.currentlySelectedIndex).quantity);
                    return;
                }
                return;
            }


            if (Convert.ToInt32(text) > Controller.inventoryData.GetItemAt(UIInventory.currentlySelectedIndex).quantity)
            {
                Debug.Log("Value exceded quantity");
                inputField.text = Convert.ToString(Controller.inventoryData.GetItemAt(UIInventory.currentlySelectedIndex).quantity);
            }
        }

        override public void OnClick()
        {
            if (UIInventory.currentlySelectedIndex == -1)
            {
                if (Convert.ToInt32(inputField.text) <= 0 || Convert.ToInt32(inputField.text) >= craftingController.craftingData.GetItemAt(craftingPanel.currentlySelectedIndex).quantity)
                {
                    HideDivideInput.Invoke();
                    return;
                }
            }

            if (craftingPanel.currentlySelectedIndex == -1)
            {
                if (Convert.ToInt32(inputField.text) <= 0 || Convert.ToInt32(inputField.text) >= Controller.inventoryData.GetItemAt(UIInventory.currentlySelectedIndex).quantity)
                {
                    HideDivideInput.Invoke();
                    return;
                }
            }

            if (UIInventory.currentlySelectedIndex == -1)
            {
                craftingController.AddStackableItemToCraftingEmptySlot(craftingController.craftingData.GetItemAt(craftingPanel.currentlySelectedIndex), Convert.ToInt32(inputField.text));
                craftingController.craftingData.RemoveItem(craftingPanel.currentlySelectedIndex, Convert.ToInt32(inputField.text));
                HideDivideInput.Invoke();
                return;
            }

            Controller.AddStackableItemToEmptySlot(Controller.inventoryData.GetItemAt(UIInventory.currentlySelectedIndex), Convert.ToInt32(inputField.text));
            Controller.inventoryData.RemoveItem(UIInventory.currentlySelectedIndex, Convert.ToInt32(inputField.text));
            HideDivideInput.Invoke();
        }
    }
}