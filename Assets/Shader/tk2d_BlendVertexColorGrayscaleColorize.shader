Shader "tk2d/BlendVertexColorGrayscaleColorize" {
//	Properties {
//		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
//		_GrayscaleFactor ("GrayscaleFactor", Range(0, 1)) = 0
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
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_GrayscaleFactor ("Grayscale Factor", Range(0, 1)) = 0
		_VertexColorWeight ("Vertex Color Weight", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _GrayscaleFactor;
			float _VertexColorWeight;

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv     : TEXCOORD0;
				float4 color  : COLOR; // tk2dSprite 会写入这里
			};

			struct VertexOutput {
				float4 pos   : SV_POSITION;
				float2 uv    : TEXCOORD0;
				float4 color : COLOR;
			};

			VertexOutput vert(VertexInput v) {
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			float4 frag(VertexOutput i) : SV_Target {
				float4 texColor = tex2D(_MainTex, i.uv);

				// 混合贴图颜色和顶点颜色（tk2dSprite.color）
				float3 mixedColor = lerp(texColor.rgb, i.color.rgb, _VertexColorWeight);

				// 灰度处理
				float gray = dot(mixedColor, float3(0.299, 0.587, 0.114));
				mixedColor = lerp(mixedColor, gray.xxx, _GrayscaleFactor);

				// alpha 也混合控制（防止过黑或透明错误）
				float alpha = texColor.a * i.color.a;

				return float4(mixedColor, alpha);
			}
			ENDHLSL
		}
	}
}
	
//Properties {
//		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
//		_GrayscaleFactor ("Grayscale Factor", Range(0, 1)) = 0
//	}
//	SubShader {
//		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//		LOD 200
//
//		Pass {
//			Blend SrcAlpha OneMinusSrcAlpha
//			ZWrite Off
//			Cull Off
//
//			HLSLPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//			#include "UnityCG.cginc"
//
//			sampler2D _MainTex;
//			float4 _MainTex_ST;
//			float _GrayscaleFactor;
//
//			struct VertexInput {
//				float4 vertex : POSITION;
//				float2 uv     : TEXCOORD0;
//				fixed4 color  : COLOR;
//			};
//
//			struct VertexOutput {
//				float4 pos   : SV_POSITION;
//				float2 uv    : TEXCOORD0;
//				fixed4 color : COLOR;
//			};
//
//			VertexOutput vert(VertexInput v) {
//				VertexOutput o;
//				o.pos = UnityObjectToClipPos(v.vertex);
//				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//				o.color = v.color;
//				return o;
//			}
//
//			fixed4 frag(VertexOutput i) : SV_Target {
//	fixed4 texColor = tex2D(_MainTex, i.uv);
//
//	// 判断是否为黑色：RGB三个通道都小于 0.01
//	bool isBlack = (i.color.r < 0.01 && i.color.g < 0.01 && i.color.b < 0.01);
//
//	fixed3 resultColor;
//	if (isBlack) {
//		resultColor = texColor.rgb; // 不叠加 vertex color
//	} else {
//		// 混合颜色但不叠加得太重
//		fixed3 mixColor = texColor.rgb * (i.color.rgb * 0.5 + 0.5);
//		resultColor = mixColor;
//	}
//
//	// 灰度处理（可选）
//	fixed gray = dot(resultColor, fixed3(0.299, 0.587, 0.114));
//	resultColor = lerp(resultColor, gray.xxx, _GrayscaleFactor);
//
//	// Alpha 使用贴图 alpha 和 vertex alpha 混合
//	return fixed4(resultColor, texColor.a * i.color.a);
//}
//			ENDHLSL
//		}
//	}
//}