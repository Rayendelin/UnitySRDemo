Shader "SRPlugin/GetCameraDepth"
{
    Properties { }

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes 
            {
                float4 positionOS : POSITION; 
            };

            struct Varyings 
            {
                float4 positionCS : SV_POSITION; 
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
                float depth = SampleSceneDepth(UV);
                return float4(depth, depth, depth, 1);
            }
            ENDHLSL
        }
    }
}