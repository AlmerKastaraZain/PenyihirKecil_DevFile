using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepInput : MonoBehaviour
{
    [SerializeField] private GameObject input;
    public static event Action onShowSleepInput, onHideSleepInput;

    public void Show()
    {
        input.SetActive(true);
        onShowSleepInput.Invoke();
    }

    public void Hide()
    {
        input.SetActive(false);
        onHideSleepInput.Invoke();
    }
}
