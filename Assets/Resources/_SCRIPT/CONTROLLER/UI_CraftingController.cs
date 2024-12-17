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
using Name;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

public enum CraftingUIState
{
    Crafting,
    Brewing
}
public class UI_CraftingController : InventoryController
{
    [SerializeField]
    public UI_Crafting UI_CraftInv;

    [SerializeField]
    public UI_CraftingPanel UI_CraftPanel;
    public CraftingSO craftingData;
    [SerializeField] public GameObject Crafting;
    [SerializeField] public GameObject Journal;
    [SerializeField] public ListOfCraftableSO listOfCraftableSO;
    public CraftingUIState controllerState;

    private void Start()
    {
        PrepareUI();

        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        craftingData.OnInventoryUpdated += UpdateCraftingUI;
        UI_CraftingAction.CraftingToFreeRoam += HideCrafting;
        UI_CraftingAction.GoToCraftingJournal += HideCrafting;
        UI_CraftingAction.GoToJournalInfo += HideCrafting;
        UI_CraftingAction.GoToCrafting += ShowCrafting;

        UI_CraftPanel.clickedItem += CheckForUserInputForDivide;
        UI_CraftInv.clickedItem += CheckForUserInputForDivide;

        UI_CraftInv.OnStackItem += HandleStacking;
        UI_CraftPanel.OnStackItem += HandleStacking;

        UI_CraftPanel.OnStackDifferentInventoryItem += HandleDifferentInventoryStacking;
        UI_CraftInv.OnStackDifferentInventoryItem += HandleDifferentInventoryStacking;
    }



    private void HandleDifferentInventoryStacking(int index1, int index2)
    {
        string comesFrom = UI_CraftPanel.currentlyDraggedItemIndex == -1 ? "Inventory" : "Crafting";

        if (comesFrom == "Inventory")
        {
            InventoryItem inventoryItem = inventoryData.inventoryItems[index1];
            inventoryData.RemoveItem(index1, inventoryData.inventoryItems[index1].quantity);
            int amountLeft = craftingData.HandleItemStack(inventoryItem, index2);
            inventoryItem.quantity = amountLeft;

            if (inventoryItem.quantity <= 0) return;

            inventoryData.AddItemAt(inventoryItem, index1);
        }
        else
        {
            InventoryItem inventoryItem = craftingData.inventoryItems[index1];
            craftingData.RemoveItem(index1, craftingData.inventoryItems[index1].quantity);
            int amountLeft = inventoryData.HandleItemStack(inventoryItem, index2);
            inventoryItem.quantity = amountLeft;

            if (inventoryItem.quantity <= 0) return;
            craftingData.AddItemAt(inventoryItem, index1);
        }
    }


    private void ShowCrafting()
    {
        Crafting.SetActive(true);
        craftingData.InformAboutChange();
        inventoryData.InformAboutChange();
        //Journal.SetActive(false);
    }

    private void HideCrafting()
    {
        Crafting.SetActive(false);
        craftingData.InformAboutChange();
        inventoryData.InformAboutChange();
        //Journal.SetActive(false);
    }

    public void UpdateCraftingUI(Dictionary<int, InventoryItem> inventoryState)
    {
        UI_CraftPanel.ResetAllItems();
        foreach (var item in inventoryState)
        {
            UI_CraftPanel.UpdateData(item.Key, item.Value.item.itemImage,
                item.Value.quantity);
        }
    }


    public void PrepareUI()
    {
        UI_CraftInv.InitializeInventoryUI(inventoryData.Size);
        UI_CraftInv.OnSwapItem += HandleSwapItems;
        UI_CraftInv.OnStartDragging += HandleDragging;
        UI_CraftInv.OnSwapDifferentItem += HandleDifferentSwapItems;

        UI_CraftPanel.InitializeCraftingUI();
        UI_CraftPanel.OnSwapItem += HandleSwapItemsCrafting;
        UI_CraftPanel.OnSwapDifferentItem += HandleDifferentSwapItems;
        UI_CraftPanel.OnStartDragging += HandleCraftingDragging;
    }

    private void HandleDifferentSwapItems(int itemIndex_1, int itemIndex_2)
    {
        string comesFrom = UI_CraftPanel.currentlyDraggedItemIndex == -1 ? "Inventory" : "Crafting";

        if (comesFrom == "Inventory")
        {
            InventoryItem item1 = inventoryData.GetItemAt(itemIndex_1);
            InventoryItem item2 = craftingData.GetItemAt(itemIndex_2);

            inventoryData.RemoveItem(itemIndex_1, item1.quantity);
            craftingData.RemoveItem(itemIndex_2, item2.quantity);

            inventoryData.AddItemAt(item2, itemIndex_1);
            craftingData.AddItemAt(item1, itemIndex_2);
        }
        else
        {
            InventoryItem item1 = inventoryData.GetItemAt(itemIndex_1);
            InventoryItem item2 = craftingData.GetItemAt(itemIndex_2);

            inventoryData.RemoveItem(itemIndex_1, item1.quantity);
            craftingData.RemoveItem(itemIndex_2, item2.quantity);

            inventoryData.AddItemAt(item2, itemIndex_1);
            craftingData.AddItemAt(item1, itemIndex_2);
        }
    }

    private void HandleCraftingDragging(int itemIndex)
    {
        InventoryItem inventoryItem = craftingData.GetItemAt(itemIndex);
        if (inventoryItem.isEmpty)
            return;
        UI_CraftInv.CreateDraggedItem(inventoryItem.item.itemImage, inventoryItem.quantity);
    }

    private void HandleSwapItemsCrafting(int itemIndex_1, int itemIndex_2)
    {
        craftingData.SwapItems(itemIndex_1, itemIndex_2);
    }

    new public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Crafting.SetActive(false);
            //Journal.SetActive(false);
            UI_CraftInv.ResetDraggedItem();
            UI_CraftingAction.CraftingToFreeRoam.Invoke();
        }
    }

    new private void CheckForUserInputForDivide()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (UI_CraftInv.currentlySelectedIndex != -1 || UI_CraftPanel.currentlySelectedIndex != -1))
        {
            inputItemDivide.InsertController(gameObject.GetComponent<InventoryController>(), UI_CraftInv);
            InputItemDivide.ShowDivideInput.Invoke();
        }
    }


    public void AddStackableItemToCraftingEmptySlot(InventoryItem inventoryItem, int quantity)
    {
        craftingData.AddItemWithoutStacking(inventoryItem, quantity);
    }

    new public void HandleStacking(int index1, int index2)
    {
        string comesFrom = UI_CraftPanel.currentlyDraggedItemIndex == -1 ? "Inventory" : "Crafting";

        if (comesFrom == "Inventory")
        {
            InventoryItem inventoryItem = inventoryData.inventoryItems[index1];
            inventoryData.RemoveItem(index1, inventoryData.inventoryItems[index1].quantity);
            int amountLeft = inventoryData.HandleItemStack(inventoryItem, index2);
            inventoryItem.quantity = amountLeft;

            if (inventoryItem.quantity <= 0) return;
            inventoryData.AddItemAt(inventoryItem, index1);
        }
        else
        {
            InventoryItem inventoryItem = craftingData.inventoryItems[index1];
            craftingData.RemoveItem(index1, craftingData.inventoryItems[index1].quantity);
            int amountLeft = craftingData.HandleItemStack(inventoryItem, index2);
            inventoryItem.quantity = amountLeft;

            if (inventoryItem.quantity <= 0) return;
            craftingData.AddItemAt(inventoryItem, index1);
        }
    }

    public void BrewAPotion()
    {
        ItemSO createdItem = listOfCraftableSO.GetCraftableItem(craftingData.inventoryItems[0].item, craftingData.inventoryItems[1].item, craftingData.inventoryItems[2].item);
        if (createdItem == null) return;

        craftingData.RemoveItem(0, 1);
        craftingData.RemoveItem(1, 1);
        craftingData.RemoveItem(2, 1);

        UpdateCraftingUI(craftingData.GetCurrentInventoryState());
        inventoryData.AddItem(createdItem, 1);
    }

    public void BrewMultiplePotions(int quantity)
    {
        ItemSO createdItem = listOfCraftableSO.GetCraftableItem(craftingData.inventoryItems[0].item, craftingData.inventoryItems[1].item, craftingData.inventoryItems[2].item);
        if (createdItem == null) return;

        craftingData.RemoveItem(0, quantity);
        craftingData.RemoveItem(1, quantity);
        craftingData.RemoveItem(2, quantity);
        UpdateCraftingUI(craftingData.GetCurrentInventoryState());


        inventoryData.AddItem(createdItem, quantity);
    }
}
