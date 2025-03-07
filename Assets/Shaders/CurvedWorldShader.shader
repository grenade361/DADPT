﻿Shader "Custom/CurvedWorld"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveStrength ("Curve Strength", Float) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            float _CurveStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                v.vertex.y += _CurveStrength * v.vertex.z * v.vertex.z; // Làm cong trục Y theo Z^2
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
