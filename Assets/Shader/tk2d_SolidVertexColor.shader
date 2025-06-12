Shader "tk2d/SolidVertexColor" {
//	Properties {
//		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
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

//    Properties {
//        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
//    }
//    
//    SubShader {
//        Tags { 
//            "Queue"="Geometry" 
//            "RenderType"="Opaque"
//            "IgnoreProjector"="True"
//        }
//        
//        // 不透明物体设置
//        ZWrite On
//        Cull Back
//        
//        Pass {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//            
//            struct appdata {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//                fixed4 color : COLOR; // 添加顶点颜色输入
//            };
//            
//            struct v2f {
//                float4 pos : SV_POSITION;
//                float2 uv : TEXCOORD0;
//                fixed4 color : COLOR; // 传递顶点颜色
//            };
//            
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//            
//            v2f vert (appdata v) {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex); // 使用现代变换函数
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 处理纹理缩放/偏移
//                o.color = v.color; // 传递顶点颜色
//                return o;
//            }
//            
//            fixed4 frag (v2f i) : SV_Target {
//                // 采样纹理并乘以顶点颜色
//                fixed4 tex = tex2D(_MainTex, i.uv);
//                return tex * i.color; // 实现顶点颜色混合
//            }
//            ENDCG
//        }
//    }
//}

Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags {"IgnoreProjector"="True" "RenderType"="Opaque"}
		Blend Off Lighting Off Cull Off Fog { Mode Off }
		LOD 110
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_vct
			#pragma fragment frag_mult 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct vin_vct 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			v2f_vct vert_vct(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag_mult(v2f_vct i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				return col;
			}
			
			ENDCG
		} 
	}
}