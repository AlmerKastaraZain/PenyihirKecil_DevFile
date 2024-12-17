using System.Collections;
using System.Collections.Generic;
using NPC_Models;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Controller
{
    public class NPCQuestController : MonoBehaviour, Interactable
    {
        [SerializeField]
        private QuestNPCSO NPCModel;

        private void Awake()
        {
            NPCModel.audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void Interact()
        {
            if (GameController.state != GameState.FreeRoam)
                return;

            DialogueManager.Instance.ChangeName(NPCModel.NPCName);
            DialogueManager.Instance.ChangePotrait(NPCModel.NPCPotrait);
            StartCoroutine(DialogueManager.Instance.ShowDialogue(NPCModel.dialogue, NPCModel.audioSource));
        }
    }
}