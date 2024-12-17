using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory_UI
{
    public static class UI_InventoryAction
    {
        public static Action ShowInventory;

        public static Action HideInventory;
        public static Action<int> OnItemActionRequested;
    }
}