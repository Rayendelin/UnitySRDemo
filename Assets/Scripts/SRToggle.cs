using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TENCENT.SRPLUGIN;


public class SRToggle : MonoBehaviour
{
    Camera present_camera;
    // Start is called before the first frame update
    private void Start()
    {
        present_camera = Camera.main;

        var cam_data = present_camera.GetUniversalAdditionalCameraData();
        var renderer = cam_data?.scriptableRenderer;

        bool enable = renderer.m_SRMode == SRPlugin.SRMode.SuperResolution;
        var toggle = GetComponent<Toggle>();
        toggle.isOn = enable;
    }

    public void OnToggleSR(bool is_enable)
    {
        Debug.Log("SR enable: " + is_enable + ", target FPS: " + Application.targetFrameRate);
        var cam_data = present_camera.GetUniversalAdditionalCameraData();

        var renderer = cam_data?.scriptableRenderer;
        if (is_enable)
            renderer.m_SRMode = SRPlugin.SRMode.SuperResolution;
        else
            renderer.m_SRMode = SRPlugin.SRMode.CommonRender;
    }

    private void Awake()
    {
    }
}