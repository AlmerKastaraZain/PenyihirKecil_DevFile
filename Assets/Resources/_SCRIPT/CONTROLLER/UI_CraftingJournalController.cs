using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crafting_Action;
using Crafting_Models;
using Crafting_UI;
using Inventory;
using Inventory_Models;
using Inventory_UI;
using Money_UI;
using Name;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CraftingJournalController : MonoBehaviour
{
    public ListOfCraftableSO craftingList;
    public InventorySO inventory;
    public InventorySO recipeInfo;
    [SerializeField] private UI_CraftingJournal craftingJournal;
    [SerializeField] private UI_CraftingJournalInfo infoJournal;
    [SerializeField] public GameObject Crafting;
    [SerializeField] public GameObject Journal;
    [SerializeField] private UI_Money UI_Money;

    private void Start()
    {
        PrepareUI();
        PrepareInventoryData();

        UI_CraftingAction.CraftingToFreeRoam += HideJournal;
        UI_CraftingAction.GoToJournalInfo += ShowInfo;
        UI_CraftingAction.GoToCrafting += HideJournal;
        UI_CraftingAction.GoToCraftingJournal += ShowJournal;

        craftingJournal.clickedItem += SelectItem;
    }

    private void SelectItem()
    {
        ResetRecipe();

        if (craftingJournal.currentlySelectedIndex != -1)
        {
            Debug.Log("Kaisar!");
            ShowInfo();
            UI_Money.UpdateUI(craftingList.craftableItems[craftingJournal.currentlySelectedIndex].ItemMarketValue);
            infoJournal.UpdateJournalInfo(craftingJournal.currentlySelectedIndex);
            UpdateRecipeInfo(recipeInfo.GetCurrentInventoryState());
        }
    }

    private void ResetRecipe()
    {
        if (!recipeInfo.inventoryItems[0].isEmpty)
        {
            recipeInfo.RemoveItem(0, recipeInfo.inventoryItems[0].quantity);
        }
        if (!recipeInfo.inventoryItems[1].isEmpty)
        {
            recipeInfo.RemoveItem(1, recipeInfo.inventoryItems[1].quantity);
        }
        if (!recipeInfo.inventoryItems[2].isEmpty)
        {
            recipeInfo.RemoveItem(2, recipeInfo.inventoryItems[2].quantity);
        }

        CraftableItems item = craftingList.craftableItems[craftingJournal.currentlySelectedIndex];

        //Fix here
        foreach (ItemSO recipe in item.recipe)
        {
            recipeInfo.AddNonStackableItem(recipe, 1);
        }
    }

    public void PrepareInventoryData()
    {
        inventory.OnInventoryUpdated += UpdateJournalUI;
        inventory.Initalize();


        foreach (ItemSO item in craftingList.craftableItems)
        {
            inventory.AddItem(item, 1);
        }

        UpdateJournalUI(inventory.GetCurrentInventoryState());
    }


    private void ShowJournal()
    {
        Journal.SetActive(true);
        infoJournal.gameObject.SetActive(false);
        //Journal.SetActive(false);
    }

    private void ShowInfo()
    {
        Journal.SetActive(false);
        infoJournal.gameObject.SetActive(true);
    }

    private void HideJournal()
    {
        Journal.SetActive(false);
        infoJournal.gameObject.SetActive(false);
    }

    public void PrepareUI()
    {
        craftingJournal.InitializeInventoryUI(inventory.Size);

        infoJournal.InitializeJournalUI();
    }

    public void UpdateJournalUI(Dictionary<int, InventoryItem> inventoryState)
    {
        craftingJournal.ResetAllItems();
        foreach (var item in inventoryState)
        {
            craftingJournal.UpdateData(item.Key, item.Value.item.itemImage,
                item.Value.quantity);
        }
    }


    public void UpdateRecipeInfo(Dictionary<int, InventoryItem> inventoryState)
    {
        Debug.Log("Cato!");
        infoJournal.ResetAllItems();

        foreach (var item in inventoryState)
        {
            infoJournal.UpdateData(item.Key, item.Value.item.itemImage,
                item.Value.quantity);
            Debug.Log("Delta!");
        }
    }
}
