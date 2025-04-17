using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public int Height = 720;
    void Awake()
    {
        int width = (int)((float)Screen.width / (float)Screen.height * Height);
        Screen.SetResolution(width, Height, true);
        //Debug.Log("render resolution, width: " + width + ", height: " + Height);
    }

}
