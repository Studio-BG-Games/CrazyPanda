Shader "ToastsText/Text Blink" {
	Properties{
		_MainTex("Font Texture (Don't Modify)", 2D) = "white" {}
		_AlbedoTex("Albedo Texture", 2D) = "white" {}
		_AlbedoSpeed("Albedo Speed", Vector) = (0,0,0,0)
		_DistAlbedoTex("Distortion Albedo", 2D) = "black" {}
		_DistAlbedoValues("Distortion Albedo Values", Vector) = (1,1,0.5,2)
		_BlinkMin("Blink Min", Float) = 0
		_BlinkMax("Blink Max", Float) = 1
		_BlinkFreq1("Blink Frequency 1", Float) = 2
		_BlinkFreq2("Blink Frequency 2", Float) = 2
		_TimeTintTex("Tint Over Time (Color Ramp)", 2D) = "white" {}
		_TimeTintSpeed("Tint Over Time Speed", Float) = 1
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
				sampler2D _TimeTintTex;
				sampler2D _DistAlbedoTex;
				uniform float4 _MainTex_ST;
				uniform float4 _AlbedoTex_ST;
				uniform float4 _ColorTint;
				uniform float4 _DistAlbedoTex_ST;
				float4 _AlbedoSpeed, _DistAlbedoValues;
				float _BlinkMin, _BlinkMax, _BlinkFreq1, _BlinkFreq2, _TimeTintSpeed;

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
					//Distort Albedo UV
					float2 offsetAlbedo = tex2D(_DistAlbedoTex, i.texcoord.xy * _DistAlbedoTex_ST.xy + _DistAlbedoTex_ST.zw + _Time.y * _DistAlbedoValues.xy).rg;
					offsetAlbedo -= float2(_DistAlbedoValues.z, _DistAlbedoValues.z);
					offsetAlbedo *= _DistAlbedoValues.w * 0.01;

					//Blink
					float blinkValue = ((sin(_Time.y * _BlinkFreq1) + 1) / 4) + ((sin(_Time.y * _BlinkFreq2) + 1) / 4);
					blinkValue = lerp(_BlinkMin, _BlinkMax, blinkValue);

					//Tint over Time
					float4 timeTint = tex2D(_TimeTintTex, float2(_TimeTintSpeed, _TimeTintSpeed) * _Time.y);

					float4 col = i.color;
					col *= blinkValue;
					col *= timeTint;
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
