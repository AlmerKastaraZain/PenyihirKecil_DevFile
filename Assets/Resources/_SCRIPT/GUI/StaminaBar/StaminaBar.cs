using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    private float maxStamina = 100;
    public float currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;
    public static StaminaBar instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }

    public void useStamina(int amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaSlider.value = currentStamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());
        }
    }

    public void RefreshStamina()
    {
        staminaSlider.value = currentStamina;
    }

    public void AddStamina(float val)
    {
        if (currentStamina + val > maxStamina)
        {
            currentStamina = maxStamina;
        }
        else currentStamina += val;
    }

    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaSlider.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }
}
