using System;
using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Unity.VisualScripting;
using UnityEngine;


namespace InteractableObject
{
    public class CraftingTableScript : MonoBehaviour, Interactable
    {
        public void Interact()
        {
            UI_CraftingAction.GoToCrafting.Invoke();
        }
    }
}