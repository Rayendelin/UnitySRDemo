using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Runtime.InteropServices;
using AOT;
using System.IO;

sealed class SuperResolutionRenderPass : ScriptableRenderPass, IDisposable
{
    [DllImport("RenderingPlugin")]
    private static extern IntPtr GetRenderEventAndDataFunc();


    [DllImport("RenderingPlugin")]
    private static extern void CleanUp();

    [DllImport("RenderingPlugin")]
    private static extern void Initialize();

    [StructLayout(LayoutKind.Sequential)]
    struct PassData
    {
        public IntPtr colorBuffer;
        public EffectType effectType;
        public float inputWidth;
        public float inputHeight;
        public float outputWidth;
        public float outputHeight;
        public float sharpness;
    };

    private PassData passData_;

    const string RenderPassName = "Haisi_Super_Resolution";

    readonly ProfilingSampler profilingSampler_;
    readonly SuperResolutionVolume volume_;

    RenderTexture srcRT_ = null;
    RenderTexture dstRT_ = null;

    private RenderTargetIdentifier passSource_ { get; set; } // Ô´Í¼Ïñ

    bool isActive => (volume_ != null && volume_.IsActive());

    private EffectType effectType_;

    // debug save rt texture
    private int saveRTCount = 0;
    public SuperResolutionRenderPass()
    {
        renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        profilingSampler_ = new ProfilingSampler(RenderPassName);

        var volumeStack = VolumeManager.instance.stack;
        volume_ = volumeStack.GetComponent<SuperResolutionVolume>();

        Debug.Log("init my render pass");
    }
    public void Dispose()
    {
        RenderTexture.ReleaseTemporary(dstRT_);
        RenderTexture.ReleaseTemporary(srcRT_);
        Debug.Log("my render pass dispose");
    }

    public void Setup (RenderTargetIdentifier source)
    {
        this.passSource_ = source;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        effectType_ = (EffectType)volume_.effectType;
        float sharpness = (float)volume_.sharpness;

        var isPostProcessEnabled = renderingData.cameraData.postProcessEnabled;
        var isSceneViewCamera = renderingData.cameraData.isSceneViewCamera;
        
        if (!isActive || !isPostProcessEnabled || isSceneViewCamera || !Application.isPlaying)
        {
            Debug.Log("post process is not enabled or it's the scene view camera or application is not playing");
            return;
        }
        
        var cmd = CommandBufferPool.Get(RenderPassName);
        cmd.Clear();
        using (new ProfilingScope(cmd, profilingSampler_))
        {
            var cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTargetDescriptor.depthBufferBits = 0;

            if (srcRT_ == null || dstRT_ == null)
            {
                srcRT_ = RenderTexture.GetTemporary(cameraTargetDescriptor);

                var dstDescriptor = new RenderTextureDescriptor(
                    Screen.width,
                    Screen.height
                    );

                dstRT_ = RenderTexture.GetTemporary(dstDescriptor);
            }

            cmd.Blit(passSource_, srcRT_);
            cmd.Blit(passSource_, dstRT_);

            cmd.SetRenderTarget(dstRT_, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            cmd.ClearRenderTarget(false, true, Color.gray);
            passData_ = new PassData();
            passData_.colorBuffer = srcRT_.colorBuffer.GetNativeRenderBufferPtr();
            passData_.effectType = effectType_;
            passData_.inputWidth = cameraTargetDescriptor.width;
            passData_.inputHeight = cameraTargetDescriptor.height;
            passData_.outputWidth = Screen.width;
            passData_.outputHeight = Screen.height;
            passData_.sharpness = sharpness;
            GCHandle gcPassDataHandle = GCHandle.Alloc(passData_, GCHandleType.Pinned);

            // Call Native Plugin 
            if (volume_.IsActive())
            {
                Debug.Log("super resolution enabled");
                cmd.IssuePluginEventAndData(GetRenderEventAndDataFunc(), 1, gcPassDataHandle.AddrOfPinnedObject());
            }

            if (saveRTCount > 0)
            {
                SaveRenderTextureToPNG(dstRT_, "CaptureTexture", "test");
                saveRTCount--;
            }
            
            cmd.Blit(dstRT_, passSource_);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    private bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Debug.Log(rt.width.ToString());
        Debug.Log(rt.height.ToString());
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
        {
            Directory.CreateDirectory(contents);
        }
        Debug.Log(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;
    }
}
