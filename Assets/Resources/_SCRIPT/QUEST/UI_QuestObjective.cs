using System;
using TMPro;
using UnityEngine;

public class UI_QuestObjective : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textObject;

    void Start()
    {
        GameController.instance.questEvents.onUpdateQuestObjectiveUI += UpdateText;
    }
    public void Show()
    {
        textObject.SetActive(true);
    }


    public void Hide()
    {
        textObject.SetActive(false);
    }

    public void UpdateText(string val)
    {
        if (val == "")
        {
            Hide();
        }

        text.text = val;
    }
}
