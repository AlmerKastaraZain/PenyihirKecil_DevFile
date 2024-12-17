using System;
using Inventory_UI;
using Shop_Action;
using UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Shop;
using Money_Models;

namespace Shop_UI
{
    public class UI_InputItemSell : IInputField
    {
        public UI_ShopSellController sellController;
        public IInventory UI_ShopSO;
        [SerializeField] private TextMeshProUGUI balanceText;
        private void Awake()
        {
            balanceText.gameObject.SetActive(false);
            sellController = FindAnyObjectByType<UI_ShopSellController>();
            UI_ShopSO = FindAnyObjectByType<UI_SellShopSO>();
            button.onClick.AddListener(OnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            UI_ShopAction.ShowSellItemUI += Show;
            UI_ShopAction.HideSellItemUI += Hide;
        }

        private void OnValueChanged(string text)
        {
            if (text.Length <= 0)
            {
                balanceText.gameObject.SetActive(false);
                return;
            }

            if (text.Length > 0)
            {
                balanceText.gameObject.SetActive(true);
            }

            int quantityInputed = Convert.ToInt32(text);

            balanceText.text = "+" + (quantityInputed * sellController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).item.ItemMarketValue).ToString();
        }

        override public void OnEndEdit(string text)
        {
            if (text.Length <= 0) return;
            if (Convert.ToInt32(text) > sellController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).quantity)
            {
                Debug.Log("Value exceded quantity");
                inputField.text = Convert.ToString(sellController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).quantity);
            }

        }

        override public void OnClick()
        {
            if (inputField.text.Length <= 0) return;

            sellController.SellMultipleItems(UI_ShopSO.currentlySelectedIndex, Convert.ToInt32(inputField.text));
            UI_ShopAction.HideSellItemUI.Invoke();
        }

    }
}