using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFogo : MonoBehaviour
{
    public int sortingOrder = 100;
    public Renderer vfxRenderer;
    public string layer;

    private void OnValidate()
    {
        vfxRenderer = GetComponent<Renderer>();
        if(vfxRenderer) 
        {
            vfxRenderer.sortingOrder = sortingOrder;
            vfxRenderer.sortingLayerName = layer;
        }
    }
}
