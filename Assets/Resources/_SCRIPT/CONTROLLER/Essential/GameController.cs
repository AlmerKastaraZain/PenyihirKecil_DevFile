using Time_Management;
using Inventory;
using Inventory_UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shop_UI;
using Shop_Action;
using Crafting_Action;
using StaminaBar_Controller;
using Shop;
using Money_Models;
//using Shop_UI;

public enum GameState
{
    FreeRoam,
    Dialogue,
    Inventory,
    Sleeping,
    ShoppingBuy,
    ShoppingSell,
    Crafting,
    CraftingJournal,
    InventoryInput,
}
public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    PickUpSystem PickUpSystem;
    [SerializeField]
    UI_InventoryController inventoryController;
    [SerializeField] private UI_Inventory inventoryUI;

    public static GameController gameController { get; private set; }
    public UnitStamina playerStamina = new UnitStamina(100f, 100f, 30f, false);

    public static GameState state = GameState.FreeRoam;
    public static TimeManager timeManager;

    private UI_ShopBuyController shopBuyController;
    private UI_ShopSellController shopSellController;

    private UI_CraftingController CraftingController;
    public MoneySO money;
    public QuestEvents questEvents;
    public bool disableMovement = false;
    private void Awake()
    {
        instance = gameObject.GetComponent<GameController>();

        questEvents = new QuestEvents();

        timeManager = GetComponent<TimeManager>();
        shopBuyController = gameObject.GetComponent<UI_ShopBuyController>();
        shopSellController = gameObject.GetComponent<UI_ShopSellController>();
        CraftingController = gameObject.GetComponent<UI_CraftingController>();
    }


    private void Start()
    {
        //Dialogue
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };
        DialogueManager.Instance.OnHideDialogue += () =>
        {
            if (state == GameState.Dialogue)
                state = GameState.FreeRoam;
        };

        //Inventory
        UI_InventoryAction.ShowInventory += () =>
        {
            state = GameState.Inventory;
        };
        UI_InventoryAction.HideInventory += () =>
        {
            state = GameState.FreeRoam;
        };

        //Sleep
        TimeManager.onSleep += () =>
        {
            state = GameState.Sleeping;
        };

        TimeManager.onSleepOver += () =>
        {
            state = GameState.FreeRoam;
        };

        //Shop
        UI_ShopAction.GoToSellShop += () =>
        {
            state = GameState.ShoppingSell;
        };
        UI_ShopAction.GoToBuyShop += () =>
        {
            state = GameState.ShoppingBuy;
        };
        UI_ShopAction.ShopToFreeRoam += () =>
        {
            state = GameState.FreeRoam;
        };

        UI_ShopAction.HideBuyItemUI += () =>
        {
            state = GameState.InventoryInput;
        };
        UI_ShopAction.ShowBuyItemUI += () =>
        {
            state = GameState.InventoryInput;
        };
        UI_ShopAction.HideSellItemUI += () =>
        {
            state = GameState.ShoppingSell;
        };
        UI_ShopAction.HideBuyItemUI += () =>
        {
            state = GameState.ShoppingBuy;
        };

        //Crafting
        UI_CraftingAction.GoToCrafting += () =>
        {
            state = GameState.Crafting;
        };
        UI_CraftingAction.GoToCraftingJournal += () =>
        {
            state = GameState.CraftingJournal;
        };
        UI_CraftingAction.CraftingToFreeRoam += () =>
        {
            state = GameState.FreeRoam;
        };

        UI_CraftingAction.HideCraftingItemUI += () =>
        {
            state = GameState.Crafting;
        };
        UI_CraftingAction.ShowCraftingItemUI += () =>
        {
            state = GameState.InventoryInput;
        };

        SleepInput.onShowSleepInput += () =>
        {
            state = GameState.InventoryInput;
        };
        SleepInput.onHideSleepInput += () =>
        {
            state = GameState.FreeRoam;
        };
    }
    public void Update()
    {
        if (state != GameState.FreeRoam)
        {
            //Disallow Player Movement
            playerController.velocity = Vector2.zero;
        }

        Debug.Log(state);
        switch (state)
        {
            case GameState.FreeRoam:
                if (!disableMovement)
                {
                    playerController.HandleUpdate();
                }
                PickUpSystem.HandleUpdate();
                inventoryController.HandleUpdate();
                break;
            case GameState.Dialogue:
                DialogueManager.Instance.HandleUpdate();
                break;
            case GameState.Inventory:
                playerController.HandleUpdate();
                PickUpSystem.HandleUpdate();
                inventoryController.HandleUpdate();
                break;
            case GameState.Sleeping:
                break;
            case GameState.ShoppingSell:
                shopSellController.HandleUpdate();
                break;
            case GameState.ShoppingBuy:
                shopBuyController.HandleUpdate();
                break;
            case GameState.Crafting:
                CraftingController.HandleUpdate();
                break;
            case GameState.CraftingJournal:
                break;
            case GameState.InventoryInput:
                break;
            default:
                throw new InvalidOperationException("Invalid state: Check GameController");
        }
    }
}
