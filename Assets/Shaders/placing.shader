// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/placing"
{
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _OccludedColor("Occluded Color", Color) = (1,1,1,1)
    }
        SubShader{

            Pass
            {
                Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
                ZTest Greater
                ZWrite Off

                CGPROGRAM
                #pragma target 3.0
                #pragma glsl
                #pragma vertex vert            
                #pragma fragment frag
                //#pragma fragmentoption ARB_precision_hint_fastest
                

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
                fixed4 _Color;  

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
                }

                ENDCG
            }

            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1"}
            LOD 200
            ZWrite On
            ZTest LEqual

            CGPROGRAM
                    // Physically based Standard lighting model, and enable shadows on all light types
                    #pragma surface surf Standard fullforwardshadows

                    // Use shader model 3.0 target, to get nicer looking lighting
                    #pragma target 3.0

                    sampler2D _MainTex;

                    struct Input {
                        float2 uv_MainTex;
                    };

                    half _Glossiness;
                    half _Metallic;
                    fixed4 _Color;

                    void surf(Input IN, inout SurfaceOutputStandard o) {
                        // Albedo comes from a texture tinted by color
                        fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                        o.Albedo = c.rgb;
                        // Metallic and smoothness come from slider variables
                        o.Metallic = _Metallic;
                        o.Smoothness = _Glossiness;
                        o.Alpha = c.a;
                    }
            ENDCG
        }
            FallBack "Diffuse"
}