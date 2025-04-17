using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine.Rendering.Universal
{
    public enum EffectType
    {
        Blur,
        Gray,
        Sharpening,
        NoEffect,
        CGSS
    };

    [Serializable]
    public sealed class EffectTypeParameter : VolumeParameter<EffectType>
    {
        public EffectTypeParameter(EffectType value, bool overrideState = false) : base(value, overrideState)
        { }
    }

    [Serializable, VolumeComponentMenu("SuperResolution/Haisi CGSS")]
    public class SuperResolutionVolume : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("是否启用超分")]
        public BoolParameter enableSuperResolution = new BoolParameter(true);

        [Tooltip("效果类型选择")]
        public EffectTypeParameter effectType = new EffectTypeParameter(EffectType.Gray);

        [Tooltip("超分sharpness")]
        public ClampedFloatParameter sharpness = new ClampedFloatParameter(0.2f, 0f, 1f);

        public bool IsActive() => enableSuperResolution == true;

        public bool IsTileCompatible() => false;
    }
}

