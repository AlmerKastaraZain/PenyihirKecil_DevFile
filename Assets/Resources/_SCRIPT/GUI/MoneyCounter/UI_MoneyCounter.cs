using System.Collections;
using System.Collections.Generic;
using Money_Models;
using TMPro;
using UnityEngine;

namespace Money_UI
{
    public class UI_MoneyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyAmount;

        public void UpdateUI(int amount)
        {
            moneyAmount.text = amount.ToString();
        }
    }
}