using System;
using Money_Models;
using Shop;
using Shop_Action;
using TMPro;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Shop_UI
{
    public class UI_InputItemBuy : IInputField
    {
        public UI_ShopBuyController buyController;
        public IInventory UI_ShopSO;
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private MoneySO money;

        private void Awake()
        {
            balanceText.gameObject.SetActive(false);
            buyController = FindAnyObjectByType<UI_ShopBuyController>();
            UI_ShopSO = FindAnyObjectByType<UI_BuyShopSO>();
            button.onClick.AddListener(OnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
            inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            UI_ShopAction.ShowBuyItemUI += Show;
            UI_ShopAction.HideBuyItemUI += Hide;
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

            balanceText.text = "-" + (quantityInputed * buyController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).item.ItemMarketValue).ToString();
            if (balanceText.text == "0")
            {
                balanceText.gameObject.SetActive(false);
            }
        }

        override public void OnEndEdit(string text)
        {
            if (text.Length <= 0) return;
            int quantityInputed = Convert.ToInt32(text);

            if (quantityInputed > buyController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).quantity)
            {
                Debug.Log("Value exceded quantity");
                inputField.text = Convert.ToString(buyController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).quantity);
            }

            if (quantityInputed * buyController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).item.ItemMarketValue > money.GetMoney())
            {
                Debug.Log("CATO!");
                double calculation = money.GetMoney() / buyController.inventoryData.GetItemAt(UI_ShopSO.currentlySelectedIndex).item.ItemMarketValue;
                int newQuantity = (int)Math.Floor(calculation);

                inputField.text = Convert.ToString(newQuantity);
            }
        }


        override public void OnClick()
        {
            if (inputField.text.Length <= 0) UI_ShopAction.HideBuyItemUI.Invoke();

            buyController.BuyMultipleItems(UI_ShopSO.currentlySelectedIndex, Convert.ToInt32(inputField.text));

            UI_ShopAction.HideBuyItemUI.Invoke();
        }
    }
}
