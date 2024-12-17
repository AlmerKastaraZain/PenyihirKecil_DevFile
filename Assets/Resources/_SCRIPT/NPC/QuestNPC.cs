using System.Collections;
using System.Collections.Generic;
using NPC_Models;
using Quest_Models;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestNPC : MonoBehaviour, Interactable
{
    [Header("NPC")]
    [SerializeField] private QuestNPCListSO listOfNPCModel;
    private QuestNPCSO CurrentModel;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;
    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = false;
    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;
    private QuestIcon questIcon;
    private UI_QuestObjective questObjective;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        questObjective = FindAnyObjectByType<UI_QuestObjective>();

        CurrentModel = listOfNPCModel.NPCList[0];
    }

    private void UpdateNPCModel(Quest quest)
    {
        Debug.Log(quest.state);
        switch (quest.state)
        {
            case QuestState.REQUIREMENT_NOT_MET:
                if (listOfNPCModel.NPCList[0] != null)
                {
                    CurrentModel = listOfNPCModel.NPCList[0];
                }
                break;
            case QuestState.CAN_START:
                if (listOfNPCModel.NPCList[1] != null)
                {
                    CurrentModel = listOfNPCModel.NPCList[1];
                }
                break;
            case QuestState.IN_PROGRESS:
                if (listOfNPCModel.NPCList[2] != null)
                {
                    CurrentModel = listOfNPCModel.NPCList[2];
                }
                break;
            case QuestState.CAN_FINISH:
                if (listOfNPCModel.NPCList[3] != null)
                {
                    CurrentModel = listOfNPCModel.NPCList[3];
                }
                break;
            case QuestState.FINISHED:
                if (listOfNPCModel.NPCList[4] != null)
                {
                    CurrentModel = listOfNPCModel.NPCList[4];
                }
                break;
            default:
                Debug.LogWarning("There's an Invalid State");
                break;
        }

        CurrentModel.audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameController.instance.questEvents.onQuestStateChange += UpdateNPCModel;
        GameController.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameController.instance.questEvents.onQuestStateChange -= UpdateNPCModel;
        GameController.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, endPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void SubmitPressed()
    {
        if (playerIsNear != true)
        {
            return;
        }

        DialogueManager.Instance.ChangeName(CurrentModel.NPCName);
        DialogueManager.Instance.ChangePotrait(CurrentModel.NPCPotrait);
        GameController.state = GameState.Dialogue;
        StartCoroutine(DialogueManager.Instance.ShowDialogue(CurrentModel.dialogue, CurrentModel.audioSource));
        StartCoroutine(QuestActivationAfterDialogue());
    }

    private IEnumerator QuestActivationAfterDialogue()
    {
        while (GameController.state == GameState.Dialogue)
            yield return null;

        Debug.Log("Pass");
        ActivateQuest();
    }

    private void ActivateQuest()
    {
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameController.instance.questEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && endPoint)
        {
            GameController.instance.questEvents.FinishQuest(questId);
        }
    }

    public void Interact()
    {
        SubmitPressed();
    }
}
