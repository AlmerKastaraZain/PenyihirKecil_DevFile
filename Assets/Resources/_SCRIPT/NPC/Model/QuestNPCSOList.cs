using System;
using System.Collections;
using System.Collections.Generic;
using Quest_Models;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Models
{
    [CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC/QuestNPCList", order = 1)]
    public class QuestNPCListSO : ScriptableObject
    {
        [field: SerializeField]
        public QuestNPCSO[] NPCList = new QuestNPCSO[5];

    }
}