using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using Quest_Models;
using UI;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;
    [SerializeField] Transform questObject;
    [SerializeField] private InventorySO playerInventory;
    private UI_QuestObjective questObjective;
    private void Awake()
    {
        questObjective = FindAnyObjectByType<UI_QuestObjective>();
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameController.instance.questEvents.onStartQuest += StartQuest;
        GameController.instance.questEvents.onAdvancedQuest += AdvanceQuest;
        GameController.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameController.instance.questEvents.onStartQuest -= StartQuest;
        GameController.instance.questEvents.onAdvancedQuest -= AdvanceQuest;
        GameController.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GameController.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameController.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetRequirement = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequistes)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetRequirement = false;
            }
        }

        return meetRequirement;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENT_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        questObjective.Show();
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(questObject);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);

    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExist())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        questObjective.Hide();
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        TakeItem(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void TakeItem(Quest quest)
    {
        for (int i = 0; i < quest.info.itemTaken.item.Count; i++)
        {
            Debug.Log("Cato!");
            playerInventory.SubtractItem(quest.info.itemTaken.item[i], quest.info.itemTaken.amount[i]);
        }
    }

    private void ClaimRewards(Quest quest)
    {
        GameController.instance.money.AddMoney(quest.info.moneyReward);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuest = Resources.LoadAll<QuestInfoSO>("_DATA");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuest)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate quest " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID NOT FOUND ON QUEST MAP " + id);
        }
        return quest;
    }
}
