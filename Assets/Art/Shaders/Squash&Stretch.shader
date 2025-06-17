Shader "Custom/SquashStretch"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _BaseMap("Base Map", 2D) = "white" {}
        _DeformationAmount("Deformation Amount", Range(0,1)) = 0
        _DeformationDirection("Deformation Direction", Vector) = (0,1,0,0)
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _ShadowHardness("Shadow Hardness", Range(0,10)) = 0.5
        _MinLight("Minimum Light", Range(0,2)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : NORMAL;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
                float _DeformationAmount;
                float3 _DeformationDirection;
                float _Smoothness;
                float _ShadowHardness;
                float _MinLight;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                
                Varyings OUT;
                
                // Deformación modificada para evitar artefactos
                float3 deformation = IN.normalOS * _DeformationAmount;
                deformation *= saturate(dot(IN.normalOS, _DeformationDirection));
                
                float3 positionOS = IN.positionOS.xyz + deformation * 0.5;
                
                OUT.positionHCS = TransformObjectToHClip(positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                
                // Iluminación básica URP
                float3 lightDir = normalize(_MainLightPosition.xyz);    //Usar vector constante para que no cambie la sombra de posición
                float NdotL = saturate(dot(IN.normalWS, lightDir));
                NdotL = pow(NdotL, _ShadowHardness);
                NdotL = max(NdotL, _MinLight);
                color.rgb *= NdotL * _MainLightColor.rgb;
                
                color.a = 1;
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}