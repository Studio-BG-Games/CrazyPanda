Shader "ToastsText/Text Texture Distort" {
	Properties{
		_MainTex("Font Texture (Don't Modify)", 2D) = "white" {}
		_DistFontTex("Distortion Font", 2D) = "black" {}
		_DistFontValues("Distortion Font Values", Vector) = (1,1,0,0)
		_AlbedoTex("Albedo Texture", 2D) = "white" {}
		_AlbedoSpeed("Albedo Speed", Vector) = (0,0,0,0)
		_DistAlbedoTex("Distortion Albedo", 2D) = "black" {}
		_DistAlbedoValues("Distortion Albedo Values", Vector) = (1,1,0.5,2)
		_ColorTint("Tint", Color) = (1,1,1,1)


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
				sampler2D _DistFontTex;
				sampler2D _DistAlbedoTex;
				uniform float4 _MainTex_ST;
				uniform float4 _AlbedoTex_ST;
				uniform float4 _ColorTint;
				uniform float4 _DistFontTex_ST;
				uniform float4 _DistAlbedoTex_ST;
				float4 _AlbedoSpeed, _DistFontValues, _DistAlbedoValues;

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
					//Distort Font UV
					float2 offsetFont = tex2D(_DistFontTex, i.texcoord.xy * _DistFontTex_ST.xy + _DistFontTex_ST.zw + _Time.y * _DistFontValues.xy).rg;
					offsetFont -= float2(_DistFontValues.z, _DistFontValues.z);
					offsetFont *= _DistFontValues.w * 0.01;
					i.texcoord += offsetFont;
					
					//Distort Albedo UV
					float2 offsetAlbedo = tex2D(_DistAlbedoTex, i.texcoord.xy * _DistAlbedoTex_ST.xy + _DistAlbedoTex_ST.zw + _Time.y * _DistAlbedoValues.xy).rg;
					offsetAlbedo -= float2(_DistAlbedoValues.z, _DistAlbedoValues.z);
					offsetAlbedo *= _DistAlbedoValues.w * 0.01;

					//Get Albedo Color
					float4 col = i.color;
					col *= _ColorTint;
					col *= tex2D(_AlbedoTex, i.texcoord.xy * _AlbedoTex_ST.xy + _AlbedoTex_ST.zw + _AlbedoSpeed * _Time.y + offsetAlbedo);
					col.a *= tex2D(_MainTex, i.texcoord).a;
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
