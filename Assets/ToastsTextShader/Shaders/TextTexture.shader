Shader "ToastsText/Text Texture" {
	Properties{
		_MainTex("Font Texture (Don't Modify)", 2D) = "white" {}
		_AlbedoTex("Albedo Texture", 2D) = "white" {}
		_AlbedoSpeed("Albedo Speed", Vector) = (0,0,0,0)
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
				uniform float4 _MainTex_ST;
				uniform float4 _AlbedoTex_ST;
				uniform float4 _ColorTint;
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
					float4 col = i.color;
					col *= _ColorTint;
					col *= tex2D(_AlbedoTex, i.texcoord.xy * _AlbedoTex_ST.xy + _AlbedoTex_ST.zw + _AlbedoSpeed * _Time.y);
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
