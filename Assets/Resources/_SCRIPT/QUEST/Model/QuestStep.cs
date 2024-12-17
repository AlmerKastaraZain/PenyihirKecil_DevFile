using Unity.VisualScripting;
using UnityEngine;

namespace Quest_Models
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;
        private string questId;

        public void InitializeQuestStep(string questId)
        {
            this.questId = questId;
        }

        protected void FinishQuestStep()
        {
            if (!isFinished)
            {
                isFinished = true;
                GameController.instance.questEvents.AdvanceQuest(questId);
                Destroy(this.gameObject);
            }
        }
    }
}