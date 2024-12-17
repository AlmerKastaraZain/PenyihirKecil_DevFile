using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory_Models
{
    public class CraftableItems : ItemSO
    {
        [SerializeField]
        public ItemSO[] recipe = new ItemSO[3];
    }
}