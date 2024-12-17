using System;
using System.Collections;
using System.Collections.Generic;
using Time_Management;
using Inventory;
using Inventory_Models;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractableObject
{
    public class CollectablePlant : MonoBehaviour, Interactable
    {
        [SerializeField]
        private InventorySO inventoryData;
        [SerializeField]
        private ItemSO itemData;
        [SerializeField]
        private AudioClip collectSound;
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private int quantityCollected = 1;
        private bool collectable = true;

        public static event Action<ItemSO> PlantCollected;
        private Vector3 originTransform;
        private Renderer renderer;


        void Awake()
        {
            renderer = GetComponent<Renderer>();
            originTransform = transform.localScale;
        }

        void Interactable.Interact()
        {
            if (collectable == false) return;

            inventoryData.AddItem(itemData, quantityCollected);
            collectable = false;
            PlantCollected?.Invoke(itemData);

            StartCoroutine(AnimatePickup());
        }


        [SerializeField]
        private float AnimationDuration = 0.3f;

        private IEnumerator AnimatePickup()
        {
            audioSource.PlayOneShot(collectSound);

            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.zero;
            float currentTime = 0;
            while (currentTime < AnimationDuration)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / AnimationDuration);
                yield return null;
            }

            PlantToggle(false);
        }

        private void PlantToggle(bool val)
        {
            renderer.enabled = val;
        }

        public static Action resetAllPlants;

        public void Start()
        {

            TimeManager.onSleep += ResetPlant;
        }

        public void ResetPlant()
        {
            Debug.Log("Reset Plant called");
            PlantToggle(true);
            collectable = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}