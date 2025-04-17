using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

[Serializable]
public sealed class SuperResolutionFeature : ScriptableRendererFeature
{
    SuperResolutionRenderPass scriptablePass_;
    public override void Create()
    {
        scriptablePass_ = new SuperResolutionRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTargetHandle;
        scriptablePass_.Setup(src);
        renderer.EnqueuePass(scriptablePass_);
    }

    protected override void Dispose(bool disposing)
    {
        scriptablePass_.Dispose();
    }
}
