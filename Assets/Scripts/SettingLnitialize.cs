using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEditor;

public class SettingLnitialize : MonoBehaviour
{
    public GameObject panel;
    public int Height = 1080;
    public float FPS = 60;
    public Material[] materials02;
    public Material[] materials03;
    public Material[] materials06;
    [SerializeField] private Volume volume;
    public ParticleSystem[] particleSystems;
    public Camera MainCamera;
    public GameObject[] HighObjects;
    public GameObject[] LowObjects;
    public Toggle[] TrueToggle;
    public Toggle[] FalseToggle;
    public Material[] SetBatchesMaterials;
    public Toggle Batches;
    // Start is called before the first frame update
    void Start()
    {
        // Application.targetFrameRate = 61;
        foreach (var material02 in materials02)
        {
            material02.SetFloat("_EnableWind" , 1);
            material02.SetFloat("_WindForce" , 0.2f);
        }
        foreach (var material03 in materials03)
        {
            material03.SetFloat("_EnableWind" , 1);
            material03.SetFloat("_WindForce" , 0.3f);
        }
        foreach (var material06 in materials06)
        {
            material06.SetFloat("_EnableWind" , 1);
            material06.SetFloat("_WindForce" , 0.6f);
        }
        // 尝试从 Volume 的 profile 中获取 Bloom 效果。
        if (volume.profile.TryGet(out Bloom bloom))
        {
            // 如果获取成功，设置 Bloom 效果的强度为 15。
            bloom.intensity.value = 15f;
        }
        // 启用粒子特效
         foreach (var particleSystem in particleSystems)
        {
            particleSystem.gameObject.SetActive(true);
        }
        //启用阴影
        foreach (var light in FindObjectsOfType<Light>())
        {
            light.shadows = LightShadows.Soft;
        }
        //启用后处理效果
        volume.GetComponent<Volume>().enabled = true;
        //最大LOD级别设置为0
        QualitySettings.maximumLODLevel = 0;
                foreach (var HighObjects in HighObjects)
                {
                    HighObjects.SetActive(true);
                }
                foreach (var LowObjects in LowObjects)
                {
                    LowObjects.SetActive(false);
                }
                foreach (var TrueToggle in TrueToggle)
                {
                    TrueToggle.isOn = true;
                }
                foreach (var SetBatchesMaterials in SetBatchesMaterials)
                {
                    SetBatchesMaterials.enableInstancing = true;
                }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
