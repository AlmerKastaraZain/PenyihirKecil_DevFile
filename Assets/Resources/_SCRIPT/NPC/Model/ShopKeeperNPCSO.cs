using System;
using System.Collections;
using System.Collections.Generic;
using NPC_Models;
using Shop.Models;
using Shop_UI;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC_Models
{
    [CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NPC/ShopKeeperNPCSO", order = 1)]
    public class ShopKeeperNPCSO : BaseNPC
    {
        [SerializeField]
        public ShopInventoryDataSO objectSO;
        public static Action aDayPassed;
        public int DayPassed;
    }
}