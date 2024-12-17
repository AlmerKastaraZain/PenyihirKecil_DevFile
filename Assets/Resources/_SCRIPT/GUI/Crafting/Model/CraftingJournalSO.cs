using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using UnityEngine;

namespace Crafting_Models
{
    [CreateAssetMenu(fileName = "Crafting", menuName = "ScriptableObjects/Crafting/CraftingJournalSO", order = 1)]
    public class CraftingJournalSO : InventorySO
    {
        [SerializeField]
        private ListOfCraftableSO listOfCraftableSO;

        private void Start()
        {
            Size = listOfCraftableSO.craftableItems.Count;
        }
    }
}