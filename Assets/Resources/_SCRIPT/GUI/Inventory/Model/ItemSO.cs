using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory_Models
{
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }
        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }
        [field: SerializeField]
        public string Effect { get; set; }

        [field: SerializeField]
        public int ItemMarketValue { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite itemImage { get; set; }

    }
}