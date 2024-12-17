using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Money_UI
{
    public class UI_Money : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void UpdateUI(int value)
        {
            text.text = value.ToString();
        }
    }
}