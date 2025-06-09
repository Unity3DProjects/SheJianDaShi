Shader "tk2d/BlendVertexColor" {
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
Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_MainTex_ST("MainTex ST", Vector) = (1,1,0,0)
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// 变换矩阵
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			// 纹理和采样器
			Texture2D _MainTex;
			SamplerState sampler_MainTex;
			float4 _MainTex_ST; // (Tiling x,y, Offset z,w)

			struct VertexInput
			{
				float3 pos : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct VertexOutput
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			VertexOutput vert(VertexInput input)
			{
				VertexOutput output;

				// 计算 MVP（Object -> World -> VP）
				float4 worldPos = mul(unity_ObjectToWorld, float4(input.pos, 1.0));
				output.pos = mul(unity_MatrixVP, worldPos);

				// 手动实现 TRANSFORM_TEX
				output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;

				output.color = input.color;
				return output;
			}

			float4 frag(VertexOutput input) : SV_TARGET
			{
				float4 texColor = _MainTex.Sample(sampler_MainTex, input.uv);
				return texColor * input.color;
			}

			ENDHLSL
		}
	}
}