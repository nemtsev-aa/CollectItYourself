Shader "Custom/Outline" 
{
    Properties 
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 10)) = 0.005
    }
    
    SubShader 
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float4 outline = tex2D(_MainTex, i.uv + float2(0, _OutlineWidth)) + // top
                                 tex2D(_MainTex, i.uv + float2(0, -_OutlineWidth)) + // bottom
                                 tex2D(_MainTex, i.uv + float2(-_OutlineWidth, 0)) + // left
                                 tex2D(_MainTex, i.uv + float2(_OutlineWidth, 0)); // right
                outline /= 4.0;
                
                return lerp(outline, col, step(0.1, col.a));
            }
            
            ENDCG
        }
    }
 
    FallBack "Diffuse"
}

