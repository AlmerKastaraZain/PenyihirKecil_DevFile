using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using Shop.Models;
using UnityEngine;

namespace Shop_Action
{
    public static class UI_ShopAction
    {
        public static Action GoToSellShop;

        public static Action GoToBuyShop;

        public static Action ShopToFreeRoam;

        public static Action<int> BuyItem;
        public static Action<int> SellItem;

        public static Action ShowBuyItemUI;
        public static Action HideBuyItemUI;
        public static Action ShowSellItemUI;
        public static Action HideSellItemUI;


    }
}