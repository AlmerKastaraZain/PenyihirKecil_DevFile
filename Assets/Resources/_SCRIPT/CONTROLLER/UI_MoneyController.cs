using System;
using System.Collections;
using System.Collections.Generic;
using Money_Models;
using Money_UI;
using UnityEngine;

public class UI_MoneyController : MonoBehaviour
{
    [SerializeField] MoneySO moneyObject;
    [SerializeField] int initialMoney;
    public static event Action<int> UpdateMoneyUI;
    [SerializeField]
    private UI_MoneyCounter moneyCounter;
    // Start is called before the first frame update
    void Start()
    {
        initializeMoney();
        UpdateMoneyCounter();

        MoneySO.MoneyChanged += UpdateMoneyCounter;
    }

    private void initializeMoney()
    {
        moneyObject.SetMoney(initialMoney);
    }

    void UpdateMoneyCounter()
    {
        moneyCounter.UpdateUI(moneyObject.GetMoney());
    }
}
