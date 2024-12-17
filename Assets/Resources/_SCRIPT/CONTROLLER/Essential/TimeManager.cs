using System;
using System.Collections;
using System.Collections.Generic;
using Time_Models;
using Unity.VisualScripting;
using UnityEngine;

namespace Time_Management
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField]
        private TimeModelSO timeModel;
        [SerializeField]
        private GameObject lightObject;

        public static Action onSleep, onSleepOver, DayHasPassed;

        private void Start()
        {
            BlackCoverEffect.animationEffectOver += AlertSleepIsOver;
            BlackCoverEffect.coverFullyBlack += UpdateLight;

            UpdateLight();
        }

        private void AlertSleepIsOver()
        {
            onSleepOver.Invoke();
        }

        private void UpdateLight()
        {
            if (timeModel.nightOrDay == TimeModelSO.DayCycle.Day)
            {
                lightObject.SetActive(false);
            }
            else if (timeModel.nightOrDay == TimeModelSO.DayCycle.Night)
            {
                lightObject.SetActive(true);
            }
        }
        public void NextDay()
        {
            if (timeModel.nightOrDay == TimeModelSO.DayCycle.Day)
            {
                timeModel.day++;

                DayHasPassed?.Invoke();
            }

            timeModel.nightOrDay = TimeModelSO.DayCycle.Day;
            onSleep?.Invoke();
        }

        public void NextNight()
        {
            if (timeModel.nightOrDay == TimeModelSO.DayCycle.Night)
            {
                timeModel.day++;

                DayHasPassed?.Invoke();
            }

            timeModel.nightOrDay = TimeModelSO.DayCycle.Night;
            onSleep?.Invoke();
        }
    }
}
