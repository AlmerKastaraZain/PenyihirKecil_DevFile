using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlackCoverEffect : MonoBehaviour
{
    private Image image;
    private Color col;
    public float AnimationDuration = 100f;
    public float WaitDuration = 2.5f;

    public static Action animationEffectOver, coverFullyBlack;

    private void Awake()
    {
        image = GetComponent<Image>();
        col = image.color;
        col.a = 0;

        Toggle(false);
    }

    private enum CoverPhase
    {
        StartEffect,
        FreezeEffect,
        EndEffect
    }

    public void StartEffectCoroutine()
    {
        Toggle(true);
        StartCoroutine(StartEffect());
    }

    private CoverPhase Phase = 0;
    public IEnumerator StartEffect()
    {
        float currentTime = 0f;
        while (currentTime < AnimationDuration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(col, Color.black, currentTime / AnimationDuration);
            yield return null;
        }
        Phase++;

        currentTime = 0f;
        while (currentTime < WaitDuration)
        {
            coverFullyBlack.Invoke();
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(Color.black, Color.black, currentTime / WaitDuration);
            yield return null;
        }
        Phase++;

        currentTime = 0f;
        while (currentTime < AnimationDuration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(Color.black, col, currentTime / AnimationDuration);
            yield return null;
        }

        Phase = 0;
        animationEffectOver.Invoke();
        Toggle(false);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
