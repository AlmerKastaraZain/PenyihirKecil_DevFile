using System.Collections;
using System.Collections.Generic;
using Crafting_Action;
using Inventory_UI;
using Shop_Action;
using Time_Management;
using UnityEngine;
using UnityEngine.UIElements;

public class BlackBackgroundEffect : MonoBehaviour
{
    [SerializeField] private GameObject Background;

    // Start is called before the first frame update
    void Start()
    {
        //Dialogue
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            ShowBackground();
        };
        DialogueManager.Instance.OnHideDialogue += () =>
        {
            HideBackground();
        };

        //Inventory
        UI_InventoryAction.ShowInventory += () =>
        {
            ShowBackground();
        };
        UI_InventoryAction.HideInventory += () =>
        {
            HideBackground();
        };

        UI_ShopAction.GoToSellShop += () =>
        {
            ShowBackground();
        };
        UI_ShopAction.GoToBuyShop += () =>
        {
            ShowBackground();
        };
        UI_ShopAction.ShopToFreeRoam += () =>
        {
            HideBackground();
        };

        UI_CraftingAction.GoToCrafting += () =>
        {
            ShowBackground();
        };
        UI_CraftingAction.GoToCraftingJournal += () =>
        {
            ShowBackground();
        };
        UI_CraftingAction.CraftingToFreeRoam += () =>
        {
            HideBackground();
        };

        HideBackground();

    }

    // Update is called once per frame
    void ShowBackground()
    {
        Background.gameObject.SetActive(true);
    }

    void HideBackground()
    {
        Background.gameObject.SetActive(false);
    }
}