using UnityEngine;
using System;
using System.Collections.Generic;
using Inventory_Models;

[System.Serializable]
public class ItemTaken
{
    [SerializeField] List<ItemSO> Item;
    [SerializeField] List<int> Amount;

    public List<ItemSO> item
    {
        get { return Item; }
    }

    public List<int> amount
    {
        get { return Amount; }
    }

}
