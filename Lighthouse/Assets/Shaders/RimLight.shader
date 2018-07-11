Shader "Custom/RimLight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex("Ramp Texture", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0, 10)) = 1
		//_RampAmount("Ramp Amount", Range(-1, 1)) = 0
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_SpecPower("Specular Power", Range(0, 100)) = 0
		//_ShadowColor("Shadow Color", Color) = (1,1,1,1)
		//_DarkShadowAmount("Dark Shadow Amount", Range(-1, 5)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf ToonRamp

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		half _RampAmount;
		//fixed4 _ShadowColor;
		//half _DarkShadowAmount;
		half _Smoothness;
		half _SpecPower;
		sampler2D _RampTex;

		half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			s.Normal = normalize(s.Normal);
			half NdotL = dot(s.Normal, lightDir);
			half4 color;
			half diff = NdotL * 0.5 + 0.5;

			half3 h = normalize(lightDir + viewDir);
			half maxNdotL = max(0, dot(s.Normal, lightDir));
			float nh = max(0, dot(s.Normal, h));
			float spec = pow(nh, _SpecPower);

			float3 specLighting = _LightColor0.rgb * step(0.1, spec) *_Smoothness;

			color.rgb = s.Albedo * _LightColor0.rgb * atten * (tex2D(_RampTex, float2(diff, 0)).rgb + 0.2) + specLighting;
			//if (_RampAmount - 0.2f >= NdotL) {//darkest shadow
			//	color.rgb = (s.Albedo * _LightColor0.rgb) * -0.7f;//_DarkShadowAmount;
			//}
			//if(_RampAmount >= NdotL){//ramp shadow
			//	color.rgb = (s.Albedo * _LightColor0.rgb) * 0.1;//(step(_RampAmount, NdotL));
			//}
			//else if (_Smoothness >= NdotL) {//midtone
			//	color.rgb = s.Albedo * _LightColor0.rgb * atten;
			//}
			//if(_Smoothness < NdotL) {//highlight
			//	color.rgb = atten * half3(1, 1, 1) * _LightColor0.rgb  * s.Albedo + .2 * atten;
			//}
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
