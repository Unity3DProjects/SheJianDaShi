Shader "tk2d/BlendVertexColorA8" {
//	Properties {
//		_MainTex ("Value (A)", 2D) = "white" {}
//	}
//	//DummyShaderTextExporter
//	SubShader{
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//
//		Pass
//		{
//			HLSLPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//
//			float4x4 unity_MatrixMVP;
//
//			struct Vertex_Stage_Input
//			{
//				float3 pos : POSITION;
//			};
//
//			struct Vertex_Stage_Output
//			{
//				float4 pos : SV_POSITION;
//			};
//
//			Vertex_Stage_Output vert(Vertex_Stage_Input input)
//			{
//				Vertex_Stage_Output output;
//				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
//				return output;
//			}
//
//			Texture2D<float4> _MainTex;
//			SamplerState sampler_MainTex;
//
//			struct Fragment_Stage_Input
//			{
//				float2 uv : TEXCOORD0;
//			};
//
//			float4 frag(Fragment_Stage_Input input) : SV_TARGET
//			{
//				return _MainTex.Sample(sampler_MainTex, float2(input.uv.x, input.uv.y));
//			}
//
//			ENDHLSL
//		}
//	}
//}


   Properties {
        _MainTex ("Value (A)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }
    
    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        
        // 启用混合模式
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            
            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color; // 合并顶点颜色和材质颜色
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                // 读取纹理并使用A通道作为Alpha
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                // 实现A8格式支持：使用纹理的alpha通道控制透明度
                fixed4 col = fixed4(i.color.rgb, i.color.a * tex.a);
                
                return col;
            }
// fixed4 frag (v2f i) : SV_Target {
//     fixed4 tex = tex2D(_MainTex, i.uv);
//     return fixed4(1,1,1, tex.a); // 白色+纹理alpha
// }
            
            ENDCG
        }
    }
}