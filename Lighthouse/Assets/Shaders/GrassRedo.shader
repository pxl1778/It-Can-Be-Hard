Shader "Custom/GrassRedo" {
	Properties {
		_TopColor ("Color", Color) = (1,1,1,1)
		_BottomColor("Color", Color) = (0,0,0,0)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RandomTex("Random Texture", 2D) = "white" {}
		_Strength("Wind Strength", Range(0, 2)) = 0.05
	}
	SubShader {
		Tags { "RenderType"="Opaque" "DisableBatching"="True" }
		LOD 200


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#include "UnityCG.cginc"
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 vertexColor;
		};

		sampler2D _RandomTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _TopColor;
		fixed4 _BottomColor;
		half _Strength;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)

		void vert(inout appdata_full v, out Input o) {
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			float rand = tex2Dlod(_RandomTex, float4(_Time[0]*2 + worldPos.x/5, _Time[0]*2 + worldPos.z/5, 0 , 0)).r;
			rand = rand * 2.0 - 1.0;
			worldPos.xz += step(0, v.vertex.y) * float2(rand, rand) * _Strength;
			v.vertex = mul(unity_WorldToObject, worldPos);
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertexColor = (step(0, v.vertex.y) * _TopColor) + (step(v.vertex.y, 0) * _BottomColor);
		}
		// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = IN.vertexColor;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
