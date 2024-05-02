Shader "Custom/BranchShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NoiseTexture1("Noise Texture 1", 2D) = "white" {}
        _NoiseTexture2("Noise Texture 2", 2D) = "white" {}
        _NoiseScale1("Noise Scale 1", Range(0, 1)) = 0.1
        _NoiseScale2("Noise Scale 2", Range(0, 1)) = 0.1
        _NoiseStrength("Noise Strength", Range(0, 1)) = 0.5
        _MaterialColor("Material Color", Color) = (1, 1, 1, 1)
        _NoiseColor("Noise Color", Color) = (1, 1, 1, 1)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTexture1;
            sampler2D _NoiseTexture2;
            float _NoiseScale1;
            float _NoiseScale2;
            float _NoiseStrength;
            fixed4 _MaterialColor;
            fixed4 _NoiseColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float noise1 = tex2D(_NoiseTexture1, i.uv * _NoiseScale1).r * 2.0 - 1.0;
                float noise2 = tex2D(_NoiseTexture2, i.uv * _NoiseScale2).r * 2.0 - 1.0;
                float noise = (noise1 + noise2) * 0.5; // Combine noise textures
                col.rgb += noise * _NoiseStrength * _NoiseColor.rgb; // Apply noise to color
                col.rgb *= _MaterialColor.rgb; // Apply material color
                return col;
            }
            ENDCG
        }
    }
}
