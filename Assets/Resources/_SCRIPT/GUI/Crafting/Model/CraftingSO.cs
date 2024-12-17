using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using UnityEngine;

namespace Crafting_Models
{
    [CreateAssetMenu(fileName = "Crafting", menuName = "ScriptableObjects/Crafting/CraftingSO", order = 1)]
    public class CraftingSO : InventorySO
    {
        [SerializeField] private InventorySO _inventory;

        new public void Initalize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < 3; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        new public void AddItemAt(InventoryItem item, int index)
        {
            int quantity = item.quantity;

            if (inventoryItems[index].isEmpty)
            {
                inventoryItems[index] = item;
                InformAboutChange();
                return;
            }

            //TODO: FIX THIS CODE
            int amountLeft = inventoryItems[index].item.MaxStackSize - quantity;
            if (quantity >= amountLeft)
            {
                inventoryItems[index] = inventoryItems[index].ChangeQuantity(item.item.MaxStackSize);
                quantity -= amountLeft + quantity;
            }
            else
            {
                inventoryItems[index] = item
                   .ChangeQuantity(quantity + inventoryItems[index].quantity);
                quantity = 0;
                InformAboutChange();
            }

            if (quantity > 0 && IsInventoryFull())
            {
                _inventory.AddItemToFirstFreeSlot(item.item, quantity);
                InformAboutChange();
                _inventory.InformAboutChange();
                return;
            }

            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.item.MaxStackSize);
                quantity -= newQuantity;
                AddItemAt(item, index);
            }

            InformAboutChange();
        }

        new public int HandleItemStack(InventoryItem item, int index1)
        {
            int quantity = item.quantity;

            int amountLeft = inventoryItems[index1].item.MaxStackSize - (quantity + inventoryItems[index1].quantity);
            Debug.Log(amountLeft);
            if (quantity >= amountLeft)
            {
                inventoryItems[index1] = inventoryItems[index1].ChangeQuantity(item.item.MaxStackSize);
                quantity -= amountLeft + quantity;
            }
            else
            {
                inventoryItems[index1] = item
                   .ChangeQuantity(quantity + inventoryItems[index1].quantity);
                quantity = 0;
                InformAboutChange();
            }

            if (quantity > 0 && IsInventoryFull())
            {
                _inventory.AddItemToFirstFreeSlot(item.item, quantity);
                InformAboutChange();
                _inventory.InformAboutChange();
            }

            InformAboutChange();
            return quantity;
        }


        new public void AddItemWithoutStacking(InventoryItem item, int quantity)
        {
            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item.item, newQuantity);
                InformAboutChange();
            }

            if (IsInventoryFull() == true)
            {
                _inventory.AddItem(item.item, quantity);
                _inventory.InformAboutChange();
            }
        }


        new public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }
    }
}

