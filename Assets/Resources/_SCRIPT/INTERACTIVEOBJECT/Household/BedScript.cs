using System.Collections;
using System.Collections.Generic;
using Time_Management;
using Time_Models;
using Unity.VisualScripting;
using UnityEngine;


namespace InteractableObject
{
    public class BedScript : MonoBehaviour, Interactable
    {
        [SerializeField]
        private SleepInput input;

        public void Interact()
        {
            input.Show();
        }
    }
}