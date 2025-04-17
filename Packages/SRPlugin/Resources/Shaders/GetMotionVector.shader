Shader "SRPlugin/GetMotionVector"
{
    Properties { }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D_X(_MotionVectorTexture);
            SAMPLER(sampler_MotionVectorTexture);

            struct Attributes
            {
                float4 positionOS   : POSITION;     // 物体空间坐标
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;      // 裁剪空间坐标
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 UV = IN.positionCS.xy / _ScaledScreenParams.xy;
                float2 motion = SAMPLE_TEXTURE2D_X(_MotionVectorTexture, sampler_MotionVectorTexture, UV).xy;
                // NOTE: motion.x and motion.y are in [-1, 1]
                return float4(motion, 0, 1);  
            }
            ENDHLSL
        }
    }
}