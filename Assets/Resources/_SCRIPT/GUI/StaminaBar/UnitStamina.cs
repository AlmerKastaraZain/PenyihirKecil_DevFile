using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStamina
{
    float currentStamina;
    float currentMaxStamina;
    float staminaRegenSpeed;

    bool pauseStaminaRegen = false;

    public float stamina
    {
        get { return currentStamina; }
        set { currentStamina = value; }
    }

    public float staminaMax
    {
        get { return currentMaxStamina; }
        set { currentMaxStamina = value; }
    }

    public float staminaRegen
    {
        get { return staminaRegenSpeed; }
        set { staminaRegenSpeed = value; }
    }

    public bool staminaPause
    {
        get { return pauseStaminaRegen; }
        set { pauseStaminaRegen = value; }
    }

    public UnitStamina(float stamina, float staminaMax, float staminaRegenSpeed, bool pauseStamin)
    {
        currentStamina = stamina;
        currentMaxStamina = staminaMax;
        staminaRegenSpeed = staminaRegen;
        pauseStaminaRegen = staminaPause;
    }

    public void UseStamina(float staminaUsed)
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaUsed * Time.deltaTime;
        }
    }

    public void RegenStamina()
    {
        if (currentStamina < currentMaxStamina && !pauseStaminaRegen)
        {
            currentStamina += staminaRegenSpeed * Time.deltaTime;
        }
    }
}
