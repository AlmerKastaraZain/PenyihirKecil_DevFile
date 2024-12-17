using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private GameObject StartquestIcon;
    [SerializeField] private GameObject finishQuestIcon;

    public void SetState(QuestState newState, bool startPoint, bool finishPoint)
    {
        StartquestIcon.SetActive(false);
        finishQuestIcon.SetActive(false);

        switch (newState)
        {
            case QuestState.REQUIREMENT_NOT_MET:
                break;
            case QuestState.CAN_START:
                if (startPoint) StartquestIcon.SetActive(true);
                break;
            case QuestState.IN_PROGRESS:
                break;
            case QuestState.CAN_FINISH:
                if (startPoint) finishQuestIcon.SetActive(true);
                break;
            case QuestState.FINISHED:
                break;
            default:
                Debug.LogWarning("Quest State not recognized bu switch statement for quest icon: " + newState);
                break;
        }
    }
}
