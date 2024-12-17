using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory_Models
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory/InventorySO", order = 1)]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        public List<InventoryItem> inventoryItems;
        public int Size = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initalize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public void ReplaceData(InventorySO Model)
        {
            inventoryItems = Model.inventoryItems;
        }

        public void AddItemWithoutStacking(InventoryItem item, int quantity)
        {
            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item.item, newQuantity);
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            if (item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        public bool IsInventoryFull()
            => inventoryItems.Where(item => item.isEmpty).Any() == false;

        public int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity
            };
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        public int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                    continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake =
                        inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity >= amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i]
                           .ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }

            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }

            return quantity;
        }


        public void RemoveItem(int itemIndex, int quantity)
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].isEmpty) return;
                int reminder = inventoryItems[itemIndex].quantity - quantity;
                if (reminder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex]
                       .ChangeQuantity(reminder);
                }
                InformAboutChange();
            }
        }

        public int AddItemToFirstFreeSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
            InformAboutChange();
        }

        public void AddOneItem(ItemSO item)
        {
            AddItem(item, 1);
            InformAboutChange();
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public void SubtractItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                    continue;
                Debug.Log("DELTA");
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake =
                        inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity <= amountPossibleToTake)
                    {
                        Debug.Log("Alda");

                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(0);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        Debug.Log("Lambda");

                        inventoryItems[i] = inventoryItems[i]
                           .ChangeQuantity(inventoryItems[i].quantity - quantity);
                        InformAboutChange();
                        return;
                    }
                }

                if (quantity > 0)
                {
                    Debug.LogWarning("Quantity Taken Exceded Item");
                }
            }
        }

        public int CheckForInstanceOfAnItem(ItemSO item)
        {
            int instanceDetected = 0;
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                    continue;

                if (inventoryItems[i].item.ID == item.ID)
                {
                    instanceDetected += inventoryItems[i].quantity;
                }

            }
            return instanceDetected;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        public void AddItemAt(InventoryItem item, int index)
        {


            if (inventoryItems[index].isEmpty)
            {
                inventoryItems[index] = item;
                InformAboutChange();
                return;
            }
            int quantity = item.quantity;

            //TODO: FIX THIS CODE
            int amountLeft = inventoryItems[index].item.MaxStackSize - (quantity + inventoryItems[index].quantity);
            Debug.Log(amountLeft);
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


            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item.item, newQuantity);
            }

            InformAboutChange();
        }

        public int HandleItemStack(InventoryItem item, int index1)
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

            InformAboutChange();
            return quantity;
        }


        public void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0
        };
    }
}