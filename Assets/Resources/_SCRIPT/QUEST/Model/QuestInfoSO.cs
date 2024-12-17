using UnityEngine;

namespace Quest_Models
{
    [CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
    public class QuestInfoSO : ScriptableObject
    {
        [field: SerializeField] public string id { get; private set; }

        private void OnValidate()
        {
#if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        [Header("General")]
        public string displayName;

        public QuestInfoSO[] questPrerequistes;

        [Header("Steps")]
        public GameObject[] questSteps;

        [Header("Rewards")]
        public int moneyReward;
        [Header("ItemTaken")]
        public ItemTaken itemTaken;
    }
}