Shader "Custom/FixedPixelSize"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Float) = 1.0
        _Color ("Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelSize;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Розраховуємо кількість пікселів на об'єкт
                float2 pixelUV = i.uv * _MainTex_ST.xy * _PixelSize;
                
                // Округлюємо до найближчого пікселя
                pixelUV = floor(pixelUV) / _PixelSize;
                
                fixed4 col = tex2D(_MainTex, pixelUV / _MainTex_ST.xy) * _Color;
                return col;
            }
            ENDCG
        }
    }
}