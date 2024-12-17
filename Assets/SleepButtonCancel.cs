using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepButtonCancel : MonoBehaviour
{
    private Button yourButton;
    [SerializeField] private SleepInput sleepInput;

    void Awake()
    {
        yourButton = GetComponent<Button>();
    }
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        sleepInput.Hide();

    }
}
