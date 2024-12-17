using System.Collections;
using System.Collections.Generic;
using Inventory_UI;
using NPC_Models;
using Shop;
using Shop_Action;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Controller
{
    public class TraderController : MonoBehaviour, Interactable
    {
        [SerializeField]
        private ShopKeeperNPCSO NPCModel;
        private UI_ShopBuyController ShopBuyController;
        private void Awake()
        {
            ShopBuyController = FindObjectOfType<UI_ShopBuyController>();
            NPCModel.audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void Interact()
        {
            if (GameController.state != GameState.FreeRoam)
                return;

            DialogueManager.Instance.ChangeName(NPCModel.NPCName);
            DialogueManager.Instance.ChangePotrait(NPCModel.NPCPotrait);
            GameController.state = GameState.Dialogue;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(NPCModel.dialogue, NPCModel.audioSource));
            StartCoroutine(ShopActivationAfterDialogue());

        }

        private IEnumerator ShopActivationAfterDialogue()
        {

            Debug.Log("Waiting for you xxx");

            while (GameController.state == GameState.Dialogue)
                yield return null;

            Debug.Log("Pass");
            ActivateShop();
        }

        public void ActivateShop()
        {
            if (NPCModel.DayPassed < NPCModel.objectSO.DaysUntilRestock)
                UpdateStore();

            if (NPCModel.DayPassed >= NPCModel.objectSO.DaysUntilRestock)
                RefreshStore();

            UI_ShopAction.GoToSellShop.Invoke();
        }

        private void UpdateStore()
        {
            Debug.Log(NPCModel);

            Debug.Log(NPCModel.objectSO);
            ShopBuyController.InsertNewInventoryData(NPCModel.objectSO);
        }

        public void RefreshStore()
        {
            NPCModel.DayPassed = 0;
            ShopBuyController.RefreshInventoryDataStock(NPCModel.objectSO);
        }
    }
}