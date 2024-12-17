using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using UnityEngine;

namespace Shop.Models
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory/ShopInventoryDataSO", order = 1)]
    public class ShopInventoryDataSO : InventorySO
    {

        [field: SerializeField]
        public List<InventoryItem> TraderStock { get; set; }
        [field: SerializeField]
        public int DaysUntilRestock { set; get; }
    }
}

