using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private GameObject currentItem;
    public Item item;

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem != null)
            {

                int reminder = inventoryData.AddItem(item.inventoryItem, item.quantity);
                if (reminder == 0)
                {
                    item.DestroyItem();
                }
                else item.quantity = reminder;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Item>(out item))
        {
            currentItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Item>(out item))
        {
            currentItem = null;
        }
    }
}
