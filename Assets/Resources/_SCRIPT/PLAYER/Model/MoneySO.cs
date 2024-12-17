using System;
using UnityEngine;

namespace Money_Models
{
    [CreateAssetMenu(fileName = "Money", menuName = "ScriptableObjects/Money", order = 1)]
    public class MoneySO : ScriptableObject
    {
        private int Money = 0;
        public static event Action MoneyChanged;
        public static event Action<int> MoneyGained, MoneyLost, MoneySet;

        public void SubtractMoney(int number)
        {
            Money -= number;
            MoneyChanged?.Invoke();
        }

        public void AddMoney(int number)
        {
            Money += number;
            MoneyChanged?.Invoke();
        }

        public void SetMoney(int number)
        {
            Money = number;
            MoneyChanged?.Invoke();
        }

        public int GetMoney()
        {
            return Money;
        }
    }
}