using System.Collections;
using System.Collections.Generic;
using Time_Management;
using UnityEngine;
using UnityEngine.UI;

public class SleepButtonDay : MonoBehaviour
{
    private Button yourButton;
    [SerializeField] TimeManager time;
    [SerializeField] private SleepInput sleepInput;

    [SerializeField] private BlackCoverEffect cover;
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
        time.NextDay();
        cover.gameObject.SetActive(true);
        cover.StartEffectCoroutine();

        sleepInput.Hide();
    }
}
