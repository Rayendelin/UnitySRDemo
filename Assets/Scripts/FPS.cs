using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setFPS : MonoBehaviour
{
    public float globalFrameRate = 120f;

    private void start()
    {
        Application.targetFrameRate = (int)globalFrameRate;
    }
}