using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crafting_Action;
using Inventory_Models;
using Inventory_UI;
using UI;
using UnityEngine;
using UnityEngine.Windows;


namespace Crafting_Models
{
    [CreateAssetMenu(fileName = "Crafting", menuName = "ScriptableObjects/Crafting/ListOfCraftableSO", order = 1)]
    public class ListOfCraftableSO : ScriptableObject
    {
        [SerializeField] public List<CraftableItems> craftableItems;


        //TODO: BUAT KODE INI AUTOMATIC, JIKA ADA WAKTU....
        public ItemSO GetCraftableItem(ItemSO item1, ItemSO item2, ItemSO item3)
        {
            foreach (CraftableItems item in craftableItems)
            {
                if (item.recipe.Count() == 3 &&
                    item.recipe[0].ID == item1.ID &&
                    item.recipe[1].ID == item2.ID &&
                    item.recipe[2].ID == item3.ID)
                {
                    Debug.Log("Recipe is valid");
                    return item;
                }
            }
            Debug.Log("Recipe is invalid");

            return null;
        }

        public bool CheckIfCraftingDataIsInvalid(ItemSO item1, ItemSO item2, ItemSO item3)
        {
            foreach (CraftableItems item in craftableItems)
            {
                if (item.recipe.Count() == 3 &&
                    item.recipe[0].ID == item1.ID &&
                    item.recipe[1].ID == item2.ID &&
                    item.recipe[2].ID == item3.ID)
                {
                    return false;
                }
            }

            return true;
        }

        public ItemSO[] GetRecipe(ItemSO item1, ItemSO item2, ItemSO item3)
        {
            foreach (CraftableItems item in craftableItems)
            {
                if (item.recipe.Count() == 3 &&
                    item.recipe[0].ID == item1.ID &&
                    item.recipe[1].ID == item2.ID &&
                    item.recipe[2].ID == item3.ID)
                {
                    Debug.Log("Recipe is valid");
                    return item.recipe;
                }
            }
            Debug.Log("Recipe is invalid");

            return null;
        }
    }
}
