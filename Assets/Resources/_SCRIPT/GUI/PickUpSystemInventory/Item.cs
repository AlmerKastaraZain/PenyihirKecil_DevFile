using System;
using System.Collections;
using System.Collections.Generic;
using Inventory_Models;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO inventoryItem { get; private set; }

    [field: SerializeField]
    public int quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;
    [SerializeField]
    private TextMeshProUGUI text;
    private void Awake()
    {
        text.text = inventoryItem.name;
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = inventoryItem.itemImage;
    }

    public void Initialize(InventoryItem inventoryItemm)
    {
        text.text = inventoryItemm.item.name;
        GetComponent<SpriteRenderer>().sprite = inventoryItemm.item.itemImage;
        quantity = inventoryItemm.quantity;
        inventoryItem = inventoryItemm.item;
    }


    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
