using System.Collections;
using System.Collections.Generic;
using InteractableObject;
using Inventory_Models;
using Quest_Models;
using UnityEngine;

public class CollectMushroomQuestStep : QuestStep
{
    private int mushroomCollected = 0;

    private int mushroomToComplete = 5;
    public ItemSO TakeraMushroom;
    public InventorySO playerInventory;

    private void OnEnable()
    {
        CollectablePlant.PlantCollected += plantCollected;
        UpdateUIQuestObjective();
    }

    private void OnDisable()
    {
        CollectablePlant.PlantCollected -= plantCollected;
        UpdateUIQuestObjective();
    }

    private void UpdateUIQuestObjective()
    {
        GameController.instance.questEvents.UpdateQuestObjectiveUI(GetQuestDescriptor());
    }

    public string GetQuestDescriptor()
    {
        if (mushroomCollected >= mushroomToComplete)
        {
            return "";
        }

        return $"Collect {mushroomCollected}/{mushroomToComplete} {TakeraMushroom.name}";
    }

    private void plantCollected(ItemSO item)
    {
        mushroomCollected = playerInventory.CheckForInstanceOfAnItem(item);

        if (mushroomCollected >= mushroomToComplete)
        {
            FinishQuestStep();
        }

        UpdateUIQuestObjective();
    }
}
