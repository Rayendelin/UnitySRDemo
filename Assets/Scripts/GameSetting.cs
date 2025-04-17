using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEditor;

public class GameSetting : MonoBehaviour
{
    public GameObject panel;
    public int Height = 720;
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


    public void TimeScale0()
    {
        Time.timeScale = 0;
    }
    public void TimeScale1()
    {
        Time.timeScale = 1;
    }

    public void ResolutionSetting630(bool isOn)
    {
        if(isOn == true)
        {
            Height = 720;
            SetResolution();
        }
    }
    public void ResolutionSetting720(bool isOn)
    {
        if(isOn == true)
        {
            Height = 980;
            SetResolution();
        }
    }
    public void ResolutionSetting1080(bool isOn)
    {
        if(isOn == true)
        {
            Height = 1080;
            SetResolution();
        }
    }
    public void SetFps(bool isOn)
    {
        if(isOn == true)
        {
            Debug.Log("60");
            Application.targetFrameRate = 61;
        }
        else
        {
            Debug.Log("30");
            Application.targetFrameRate = 30;
        }
    }
    public void SetVertexAnim(bool isOn)
    {
        if(isOn == true)
            {
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
            }
            else
            {
                foreach (var material02 in materials02)
                {
                    material02.SetFloat("_EnableWind" , 0);
                    material02.SetFloat("_WindForce" , 0f);
                }
                foreach (var material03 in materials03)
                {
                    material03.SetFloat("_EnableWind" , 0);
                    material03.SetFloat("_WindForce" , 0f);
                }
                foreach (var material06 in materials06)
                {
                    material06.SetFloat("_EnableWind" , 0);
                    material06.SetFloat("_WindForce" , 0f);
                }
            }
    }
    public void SetBloom(bool isOn)
    {
        if(isOn == true)
        {
            // 尝试从 Volume 的 profile 中获取 Bloom 效果。
            if (volume.profile.TryGet(out Bloom bloom))
            {
                // 如果获取成功，设置 Bloom 效果的强度为 15。
                bloom.intensity.value = 15f;
            }
        }
        else
        {
            // 尝试从 Volume 的 profile 中获取 Bloom 效果。
            if (volume.profile.TryGet(out Bloom bloom))
            {
                // 如果获取成功，设置 Bloom 效果的强度为 0。
                bloom.intensity.value = 0f;
            }
        }
    }
    public void SetParticle(bool isOn)
    {
        if(isOn == true)
        {
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.gameObject.SetActive(false);
            }            
        }
    }
    public void SetShadow(bool isOn)
    {
        if(isOn == true)
        {
            foreach (var light in FindObjectsOfType<Light>())
            {
                light.shadows = LightShadows.Soft;
            }
        }
        else
        {
            foreach (var light in FindObjectsOfType<Light>())
            {
                light.shadows = LightShadows.None;
            }
        }
    }
    public void SetPostProcessing(bool isOn)
    {
        if(isOn == true)
        {
            volume.GetComponent<Volume>().enabled = true;
        }
        else
        {
            volume.GetComponent<Volume>().enabled = false;
        }
    }
    public void SetMaxLOD(bool isOn)
    {
        if (isOn == true)
        {
            QualitySettings.maximumLODLevel = 0;
            // if (Batches.isOn == true)
            // {
            //     QualitySettings.maximumLODLevel = 2;
            // }         
        }
        else
        {
            QualitySettings.maximumLODLevel = 2;
            // if (Batches.isOn == false)
            // {
            //     QualitySettings.maximumLODLevel = 0;
            // }     
        }
    }
    public void SetBatches01(bool isOn)
    {
        if(isOn == true)
            {
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
                    SetBatchesMaterials.enableInstancing = false;
                }
                var pipelineAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
                if (pipelineAsset != null)
                {
                    // 关闭UseSRPBatcher
                    pipelineAsset.useSRPBatcher = true;
                    Debug.Log("SRP Batcher enabled.");
                }
            }
    }
    public void SetBatches02(bool isOn)
    {
        if(isOn == true)
            {
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
                // var pipelineAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
                // if (pipelineAsset != null)
                // {
                //     // 启用UseSRPBatcher
                //     pipelineAsset.useSRPBatcher = false;
                //     Debug.Log("SRP Batcher dis.");
                // }
            }
    }
    public void SetBatches03(bool isOn)
    {
        if(isOn == true)
            {
                foreach (var HighObjects in HighObjects)
                {
                    HighObjects.SetActive(false);
                }
                foreach (var LowObjects in LowObjects)
                {
                    LowObjects.SetActive(true);
                }
                foreach (var TrueToggle in TrueToggle)
                {
                    TrueToggle.isOn = true;
                }
                foreach (var SetBatchesMaterials in SetBatchesMaterials)
                {
                    SetBatchesMaterials.enableInstancing = true;
                }
                // var pipelineAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
                // if (pipelineAsset != null)
                // {
                //     // 启用UseSRPBatcher
                //     pipelineAsset.useSRPBatcher = false;
                //     Debug.Log("SRP Batcher dis.");
                // }
            }
    }
    public void SetBatches04(bool isOn)
    {
        if(isOn == true)
            {
                foreach (var HighObjects in HighObjects)
                {
                    HighObjects.SetActive(false);
                }
                foreach (var LowObjects in LowObjects)
                {
                    LowObjects.SetActive(true);
                }
                foreach (var FalseToggle in FalseToggle)
                {
                    FalseToggle.isOn = true;
                }
                foreach (var SetBatchesMaterials in SetBatchesMaterials)
                {
                    SetBatchesMaterials.enableInstancing = true;
                }
                // var pipelineAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
                // if (pipelineAsset != null)
                // {
                //     // 启用UseSRPBatcher
                //     pipelineAsset.useSRPBatcher = false;
                //     Debug.Log("SRP Batcher dis.");
                // }
            }
    }






    void SetResolution()
    {
        int width = (int)((float)Screen.width / (float)Screen.height * Height);
        Screen.SetResolution(width, Height, true);
        Debug.Log("render resolution, width: " + width + ", height: " + Height);
    }
    





    // private void Start()
    // {
    //     foreach (var material02 in materials02)
    //     {
    //         material02.SetFloat("_EnableWind" , 1);
    //         material02.SetFloat("_WindForce" , 0.2f);
    //     }
    //     foreach (var material03 in materials03)
    //     {
    //         material03.SetFloat("_EnableWind" , 1);
    //         material03.SetFloat("_WindForce" , 0.3f);
    //     }
    //     foreach (var material06 in materials06)
    //     {
    //         material06.SetFloat("_EnableWind" , 1);
    //         material06.SetFloat("_WindForce" , 0.6f);
    //     }
    //     // 尝试从 Volume 的 profile 中获取 Bloom 效果。
    //     if (volume.profile.TryGet(out Bloom bloom))
    //     {
    //         // 如果获取成功，设置 Bloom 效果的强度为 15。
    //         bloom.intensity.value = 15f;
    //     }
    //     // 启用粒子特效
    //      foreach (var particleSystem in particleSystems)
    //     {
    //         particleSystem.gameObject.SetActive(true);
    //     }
    //     //启用阴影
    //     foreach (var light in FindObjectsOfType<Light>())
    //     {
    //         light.shadows = LightShadows.Soft;
    //     }
    //     //启用后处理效果
    //     volume.GetComponent<Volume>().enabled = true;
    //     //最大LOD级别设置为0
    //     QualitySettings.maximumLODLevel = 0;
    //             foreach (var HighObjects in HighObjects)
    //             {
    //                 HighObjects.SetActive(true);
    //             }
    //             foreach (var LowObjects in LowObjects)
    //             {
    //                 LowObjects.SetActive(false);
    //             }
    //             foreach (var TrueToggle in TrueToggle)
    //             {
    //                 TrueToggle.isOn = true;
    //             }
    //             foreach (var SetBatchesMaterials in SetBatchesMaterials)
    //             {
    //                 SetBatchesMaterials.enableInstancing = true;
    //             }
    // }
}
