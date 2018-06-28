﻿Shader "Custom/RimLight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0, 10)) = 1
		_RampAmount("Ramp Amount", Range(-1, 1)) = 0
		_ShadowColor("Shadow Color", Color) = (1,1,1,1)
		_DarkShadowAmount("Dark Shadow Amount", Range(-1, 5)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Ramp fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		half _RampAmount;
		fixed4 _ShadowColor;
		half _DarkShadowAmount;

		half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot(s.Normal, lightDir);
			half4 color;
			//if (_RampAmount - 0.2f >= NdotL) {//darkest shadow
			//	color.rgb = (s.Albedo * _LightColor0.rgb) * -0.7f;//_DarkShadowAmount;
			//}
			if(_RampAmount >= NdotL){//ramp shadow
				color.rgb = (s.Albedo * _LightColor0.rgb) * 0.1;//(step(_RampAmount, NdotL));
			}
			else if (0.96 >= NdotL) {//midtone
				color.rgb = s.Albedo * _LightColor0.rgb;
			}
			else {//highlight
				color.rgb = half3(1, 1, 1) * _LightColor0.rgb  * s.Albedo + .2;
			}
			color.a = s.Alpha;
			return color;
		}

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		fixed4 _Color;
		float _RimPower;
		fixed4 _RimColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
			o.Emission = _RimColor * pow(1.0 - saturate(dot(IN.viewDir, o.Normal)), _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
