Shader "ToastsText/Text Time Distort" {
	Properties{
		_MainTex("Font Texture", 2D) = "white" {}
		_AlbedoTex("Albedo Texture", 2D) = "white" {}
		_AlbedoSpeed("Albedo Speed", Vector) = (0,0,0,0)
		_DistTex("Distortion Texture", 2D) = "black" {}
		_Scale("Distortion Scale", Float) = 1
		_SwingScale("Distortion Swing Frequency", Float) = 0
		_ColorTinty("Tint", Color) = (1,1,1,1)


		// required for UI.Mask
		_StencilComp("Stencil Comparison (Ignore)", Float) = 8
		_Stencil("Stencil ID (Ignore)", Float) = 0
		_StencilOp("Stencil Operation (Ignore)", Float) = 0
		_StencilWriteMask("Stencil Write Mask (Ignore)", Float) = 255
		_StencilReadMask("Stencil Read Mask (Ignore)", Float) = 255
		_ColorMask("Color Mask (Ignore)", Float) = 15
	}
		SubShader{

			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			// required for UI.Mask
		 Stencil
		 {
			 Ref[_Stencil]
			 Comp[_StencilComp]
			 Pass[_StencilOp]
			 ReadMask[_StencilReadMask]
			 WriteMask[_StencilWriteMask]
		 }
		  ColorMask[_ColorMask]

			Pass {
//UNITY_SHADER_NO_UPGRADE
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#include "UnityCG.cginc"

				struct appdata_my {
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 color : COLOR0;
					float2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				sampler2D _AlbedoTex;
				sampler2D _DistTex;
				uniform float4 _MainTex_ST;
				uniform float4 _AlbedoTex_ST;
				uniform float4 _DistTex_ST;
				uniform float4 _ColorTinty;
				uniform float _Scale;
				uniform float _SwingScale;
				float4 _AlbedoSpeed;

				v2f vert(appdata_my v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.color = v.color;
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float2 offset = tex2D(_DistTex, i.texcoord.xy * _DistTex_ST.xy + _DistTex_ST.zw).rg;
					offset -= float2(0.2, 0.2);
					offset *= _Scale * saturate(sin(_SwingScale* _Time.y)) * 0.1;

					float4 col = i.color;
					col *= _ColorTinty;
					col *= tex2D(_AlbedoTex, i.texcoord.xy* _AlbedoTex_ST.xy + _AlbedoTex_ST.zw + _Time.y * _AlbedoSpeed.xy);
					col.a *= tex2D(_MainTex, i.texcoord + offset.xy).a;
					return col;
				}
				ENDCG
			}
		}

			SubShader{
				Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
				Lighting Off Cull Off ZTest Always ZWrite Off Fog { Mode Off }
				Blend SrcAlpha OneMinusSrcAlpha
				Pass {
					Color[_Color]
					SetTexture[_MainTex] {
						combine primary, texture * primary
					}
				}
				}
}
