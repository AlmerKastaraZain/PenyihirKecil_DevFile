using System;
using System.Collections;
using System.Collections.Generic;
using Quest_Models;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Models
{
    [CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC/NPCList", order = 1)]
    public class NPCListSO : ScriptableObject
    {
        [field: SerializeField]
        public BaseNPC[] NPCList = new BaseNPC[5];

    }
}