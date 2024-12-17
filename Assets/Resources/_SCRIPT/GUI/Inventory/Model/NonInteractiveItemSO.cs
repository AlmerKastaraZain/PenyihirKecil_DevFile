using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory_Models
{

    [CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/Items/NonInteractiveItemSO", order = 1)]
    public class NonInteractiveItemSO : ItemSO, IDestroyableItem
    {
        public interface IDestroyableItem
        {

        }
    }
}