Shader "Custom/FlowerSway" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_RampTex("Ramp Texture", 2D) = "white" {}
	_SpecTex("Specular Map", 2D) = "white" {}
	_NormalTex("Normal Map", 2D) = "white" {}
	_RimTex("Rim Map", 2D) = "white" {}
	_RimColor("Rim Color", Color) = (0,0,0,0)
		_RimPower("Rim Power", Range(0, 10)) = 1
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_SpecPower("Specular Power", Range(0, 100)) = 0
		_RandomTex("Random Texture", 2D) = "white" {}
	_Strength("Wind Strength", Range(0, 2)) = 0.05
		_ObjectPoint("Object Point", Vector) = (0, 0, 0, 0)
		_MaxDistance("Max Distance", Float) = 1.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf ToonRamp fullforwardshadows vertex:vert addshadow

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
			diff -= step(0.99, diff) * 0.01;
			diff += step(diff, 0.01) * 0.01;

			half3 h = normalize(lightDir + viewDir);
			half maxNdotL = max(0, dot(s.Normal, lightDir));
			float nh = max(0, dot(s.Normal, h));
			float spec = pow(nh, _SpecPower);
			float3 specLighting = _LightColor0.rgb * step(0.1, spec) *_Smoothness * s.Specular;

			color.rgb = s.Albedo * _LightColor0.rgb * atten * (tex2D(_RampTex, float2(diff, 0)).rgb + 0.2) + specLighting;
			color.a = s.Alpha;
			return color;
		}

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _SpecTex;
		sampler2D _RimTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_SpecTex;
			float2 uv_RimTex;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		fixed4 _Color;
		float _RimPower;
		fixed4 _RimColor;
		sampler2D _RandomTex;
		half _Glossiness;
		half _Metallic;
		half _Strength;
		float4 _ObjectPoint;
		float _MaxDistance;

		void vert(inout appdata_full v, out Input o) {
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			float rand = tex2Dlod(_RandomTex, float4(_Time[0] * 2 + worldPos.x / 5, _Time[0] * 2 + worldPos.z / 5, 0, 0)).r;
			rand = rand * 2.0 - 1.0;
			worldPos.xz += /*step(15.0f, v.vertex.y)*/(v.vertex.y + 15)/30 * float2(rand, rand) * _Strength;
			worldPos.y +=  step(10, v.vertex.y) * (v.vertex.y + 15) / 30 * rand * _Strength * 1.2;

			float dist = distance(worldPos, _ObjectPoint);
			if (dist < _MaxDistance) {
				float invert = 1 - (dist / _MaxDistance);
				float3 differenceVector = worldPos - _ObjectPoint;
				worldPos.xz += (step(0, v.vertex.y) * normalize(differenceVector) * (_MaxDistance * invert)).xz;
			}

			v.vertex = mul(unity_WorldToObject, worldPos);
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutput o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Alpha = c.a;
		o.Specular = tex2D(_SpecTex, IN.uv_SpecTex).r;
		if (tex2D(_NormalTex, IN.uv_NormalTex).r <= 0.99)
		{
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
		}
		float rimAmount = tex2D(_RimTex, IN.uv_RimTex).r;
		o.Emission = _RimColor * pow(1.0 - saturate(dot(IN.viewDir, o.Normal)), _RimPower) * rimAmount;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
