Shader "Custom/LiquidShader" {
	Properties{
		_Color("Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_BackColor("Back Color", Color) = (1,1,1,1)
		_RampColor("Ramp Color", Color) = (0.7, 0.7, 0.7, 0.7)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FillHeight ("Fill Height", Range(-1, 1)) = 0.0
		_FillRamp ("Fill Ramp", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Lighting Off
		Cull Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos; // world position built-in value
			float facing : VFACE;
		};

		half _Glossiness;
		half _Metallic;
		half _FillHeight;
		half _FillRamp;
		fixed4 _Color;
		fixed4 _BackColor;
		fixed4 _RampColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
			localPos = localPos + (localPos.x * sin(_Time[2])/10) + (localPos.z * cos(_Time[2] * 3) / 9);
			if (localPos.y > _FillHeight) {
				discard;
			}

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float colorDiff = step(0, IN.facing); //if it's back facing
			float rampDiff = step(_FillHeight - _FillRamp, localPos.y); //color below the top of the liquid

			o.Albedo = (c.rgb * colorDiff * (1 - rampDiff) + (_RampColor * rampDiff * colorDiff)) + (_BackColor * (1 - colorDiff));

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
