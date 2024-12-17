using System;
using System.Collections;
using System.Collections.Generic;
using Shop.Models;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Models
{
    [CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC/BaseNPC", order = 1)]
    public abstract class BaseNPC : ScriptableObject
    {
        [field: SerializeField] public Dialogue dialogue { get; set; }
        public int ID => GetInstanceID();

        [field: SerializeField] public string NPCName { get; set; }

        [field: SerializeField] public AudioSource audioSource { get; set; }

        [field: SerializeField] public Sprite NPCPotrait { get; set; }


    }
}