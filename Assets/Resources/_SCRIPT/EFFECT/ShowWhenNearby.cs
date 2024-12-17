using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ShowWhenNearby : MonoBehaviour
{
    [SerializeField] private GameObject InteractableText;

    private void Awake()
    {
        InteractableText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractableText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractableText.SetActive(false);
        }
    }

}

