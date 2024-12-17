using System;
using System.Collections;
using System.Collections.Generic;
using Time_Management;
using InteractableObject;
using UnityEngine;

namespace Time_Models
{

    [CreateAssetMenu(fileName = "Time", menuName = "ScriptableObjects/Time/TimeModelSO", order = 1)]
    public class TimeModelSO : ScriptableObject
    {
        public int day = 0;
        public DayCycle nightOrDay = 0;
        public enum DayCycle
        {
            Day,
            Night
        }
    }
}