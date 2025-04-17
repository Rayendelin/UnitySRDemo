using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static bool IsIphonePlatform()
    {
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.OSXPlayer
            || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            return true;

        }
        else
        {
            return false;

        }
    }
}
