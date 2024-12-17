using System;

public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string id)
    {
        if (onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvancedQuest;

    public void AdvanceQuest(string id)
    {
        if (onAdvancedQuest != null)
        {
            onAdvancedQuest(id);
        }
    }

    public event Action<string> onFinishQuest;

    public void FinishQuest(string id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

    public event Action<string> onUpdateQuestObjectiveUI;

    public void UpdateQuestObjectiveUI(string val)
    {
        if (onUpdateQuestObjectiveUI != null)
        {
            onUpdateQuestObjectiveUI(val);
        }
    }
}