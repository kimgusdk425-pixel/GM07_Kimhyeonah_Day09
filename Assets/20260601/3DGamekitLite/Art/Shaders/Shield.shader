Shader "Custom/URP_Shield"
{
    Properties
    {
        [HDR] _BaseColor("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _ShieldSpeed("Shield Speed", Float) = 200
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            // 코드의 Blend SrcAlpha One 효과 (Additive)
            Blend One One 
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float _Smoothness;
            float _Metallic;
            float _ShieldSpeed;

            Varyings vert (Attributes input)
            {
                Varyings output;
                // 기존의 v.vertex.xyz += v.normal.xyz * 0.01; 효과 적용
                float3 posOS = input.positionOS.xyz + (input.normalOS * 0.01);
                output.positionCS = TransformObjectToHClip(posOS);
                output.uv = input.uv;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(TransformObjectToWorld(posOS));
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float3 normal = normalize(input.normalWS);
                float3 viewDir = normalize(input.viewDirWS);
                
                // 프레넬 연산: saturate(1-pow(dot(o.Normal, IN.viewDir), 0.1))
                float fresnel = saturate(1.0 - pow(saturate(dot(normal, viewDir)), 0.1));
                
                // 깜빡임 연산: ((sin(_Time.x * 200)+1)/2)
                float blink = (sin(_Time.y * _ShieldSpeed) + 1.0) / 2.0;
                
                half4 color = fresnel * _BaseColor * blink;
                
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}