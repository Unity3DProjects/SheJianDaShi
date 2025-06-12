Shader "Custom/MaskSpineMulti" {
//	Properties {
//		_MainTex ("Texture", 2D) = "white" {}
//		_MaskedFill0 ("MaskedTextureFill0", 2D) = "black" {}
//		_MaskedCut0 ("MaskedTextureCut0", 2D) = "black" {}
//		_MaskedFill1 ("MaskedTextureFill1", 2D) = "black" {}
//		_MaskedCut1 ("MaskedTextureCut1", 2D) = "black" {}
//	}
//	//DummyShaderTextExporter
//	SubShader{
////		Tags { "RenderType"="Opaque" }
//		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
//ZWrite Off
//
//
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
    
    
// Properties {
//        _MainTex ("Texture", 2D) = "white" {}
//        _MaskedFill0 ("MaskedTextureFill0", 2D) = "black" {}
//        _MaskedCut0 ("MaskedTextureCut0", 2D) = "black" {}
//        _MaskedFill1 ("MaskedTextureFill1", 2D) = "black" {}
//        _MaskedCut1 ("MaskedTextureCut1", 2D) = "black" {}
//    }
//    SubShader {
//        Tags { 
//            "RenderType"="Transparent" 
//            "Queue"="Transparent" 
//        }
//        ZWrite Off
//        Blend SrcAlpha OneMinusSrcAlpha
//        Cull Off // 关键：禁用背面剔除
//
//        Pass {
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//
//            struct appdata {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            sampler2D _MaskedFill0;
//            sampler2D _MaskedCut0;
//            sampler2D _MaskedFill1;
//            sampler2D _MaskedCut1;
//
//            v2f vert (appdata v) {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = v.uv;
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target {
//                fixed4 mainColor = tex2D(_MainTex, i.uv);
//                fixed4 fill0 = tex2D(_MaskedFill0, i.uv);
//                fixed4 cut0 = tex2D(_MaskedCut0, i.uv);
//                fixed4 fill1 = tex2D(_MaskedFill1, i.uv);
//                fixed4 cut1 = tex2D(_MaskedCut1, i.uv);
//                
//                // 简化的遮罩混合逻辑 - 根据实际需求修改
//                fixed4 result = mainColor;
//                result.rgb = lerp(result.rgb, fill0.rgb, cut0.a);
//                result.rgb = lerp(result.rgb, fill1.rgb, cut1.a);
//                
//                return result;
//            }
//            ENDHLSL
//        }
//    }
//}

//Properties {
//        _MainTex ("Base Texture", 2D) = "white" {}
//        _MaskedFill0 ("Mask Fill 0", 2D) = "black" {}
//        _MaskedCut0 ("Mask Cut 0", 2D) = "black" {}
//        _MaskedFill1 ("Mask Fill 1", 2D) = "black" {}
//        _MaskedCut1 ("Mask Cut 1", 2D) = "black" {}
//    }
//    SubShader {
//        Tags { 
//            "RenderType" = "Transparent" 
//            "Queue" = "Transparent" 
//        }
//        
//        ZWrite Off
//        Blend SrcAlpha OneMinusSrcAlpha
//        Cull Off // 禁用背面剔除，确保双面渲染
//        
//        Pass {
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//
//            struct appdata {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            sampler2D _MaskedFill0;
//            sampler2D _MaskedCut0;
//            sampler2D _MaskedFill1;
//            sampler2D _MaskedCut1;
//            
//            // 纹理的平铺和偏移设置（从材质面板获取）
//            float4 _MainTex_ST;
//            float4 _MaskedFill0_ST;
//            float4 _MaskedCut0_ST;
//            float4 _MaskedFill1_ST;
//            float4 _MaskedCut1_ST;
//
//            v2f vert (appdata v) {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                
//                // 应用纹理的平铺和偏移设置
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target {
//                // 1. 采样主纹理
//                fixed4 mainColor = tex2D(_MainTex, i.uv);
//                
//                // 2. 采样遮罩纹理（使用各自的ST设置）
//                fixed4 cut0 = tex2D(_MaskedCut0, i.uv * _MaskedCut0_ST.xy + _MaskedCut0_ST.zw);
//                fixed4 fill0 = tex2D(_MaskedFill0, i.uv * _MaskedFill0_ST.xy + _MaskedFill0_ST.zw);
//                
//                fixed4 cut1 = tex2D(_MaskedCut1, i.uv * _MaskedCut1_ST.xy + _MaskedCut1_ST.zw);
//                fixed4 fill1 = tex2D(_MaskedFill1, i.uv * _MaskedFill1_ST.xy + _MaskedFill1_ST.zw);
//                
//                // 3. 应用遮罩逻辑
//                // 方法1：使用遮罩的alpha通道作为混合因子
//                fixed4 result = mainColor;
//                
//                // 先应用第一个遮罩层
//                result.rgb = lerp(result.rgb, fill0.rgb, cut0.a);
//                
//                // 再应用第二个遮罩层
//                result.rgb = lerp(result.rgb, fill1.rgb, cut1.a);
//                
//                // 保持原始透明度
//                result.a = mainColor.a;
//                
//                return result;
//            }
//            ENDHLSL
//        }
//    }
//    FallBack "Diffuse"
//}

 Properties {
        _MainTex ("Base Texture", 2D) = "white" {}
        // 保留属性但不使用（为了兼容材质设置）
        _MaskedFill0 ("Mask Fill 0 (Unused)", 2D) = "black" {}
        _MaskedCut0 ("Mask Cut 0 (Unused)", 2D) = "black" {}
        _MaskedFill1 ("Mask Fill 1 (Unused)", 2D) = "black" {}
        _MaskedCut1 ("Mask Cut 1 (Unused)", 2D) = "black" {}
    }
    SubShader {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // 禁用背面剔除，确保双面渲染
        
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST; // 纹理的平铺和偏移设置

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // 应用纹理的平铺和偏移设置
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // 只采样主纹理
                return tex2D(_MainTex, i.uv);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}