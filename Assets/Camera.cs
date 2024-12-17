using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(800, 600, FullScreenMode.FullScreenWindow);
    }

    private float lastWidth;
    private float lastHeight;

    void Update()
    {
        if (lastWidth != Screen.width)
        {
            Screen.SetResolution(Screen.width, Screen.width * (8 / 6), FullScreenMode.FullScreenWindow);
        }
        else if (lastHeight != Screen.height)
        {
            Screen.SetResolution(Screen.height * (6 / 8), Screen.height, FullScreenMode.FullScreenWindow);
        }

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}
