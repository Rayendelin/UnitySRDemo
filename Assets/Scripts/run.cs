using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Run : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Button;

    public void Start()
    {
        pause();
    }

    public void Cameras()
    {
        if (GameIsPaused == false)
        {
            pause();
        }
        else
        {
            Resume();
        }
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    public void pause()
    {
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

}