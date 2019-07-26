// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/MoveableObject" {
	Properties{
		_MainTex("Albedo1 (RGB)", 2D) = "white" {}
		_SecondaryTex("Albedo2 (RGB)", 2D) = "white" {}
		_EffectColor("Effect Color", Color) = (1, 1, 1, 1)
		_EffectPower("Effect Power", Range(1, 5)) = 2
		_EffectSpeed("Effect Speed", Range(0, 2)) = 1
		_RampTex("Ramp Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_SpecPower("Specular Power", Range(0, 100)) = 0
		_RampAmount("Ramp Amount", Range(-1, 1)) = 0
		_ShadowColor("Shadow Color", Color) = (1,1,1,1)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Ramp fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			half _RampAmount;
			fixed4 _ShadowColor;
			half _Smoothness;
			half _SpecPower;
			sampler2D _RampTex;
			fixed4 _EffectColor;

			half4 LightingRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {

				half NdotL = dot(s.Normal, lightDir);
				half4 color;
				half diff = NdotL * 0.5 + 0.5;

				half3 h = normalize(lightDir + viewDir);
				half maxNdotL = max(0, dot(s.Normal, lightDir));
				float nh = max(0, dot(s.Normal, h));
				float spec = pow(nh, _SpecPower);

				float3 specLighting = _LightColor0.rgb * step(0.1, spec) *_Smoothness;

				color.rgb = s.Albedo * _LightColor0.rgb * atten * (tex2D(_RampTex, float2(diff, 0)).rgb + 0.2) + specLighting;
				color.a = s.Alpha;
				return color;
			}

			sampler2D _MainTex;
			//float4 _MainTex_ST;
			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;
			half _EffectPower;
			half _EffectSpeed;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos; // world position built-in value
				float3 worldNormal; // world normal built-in value
				float3 viewDir;// view direction built-in value we're using for rimlight
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutput o) {

				//mainTexture
				float3 effectTexture = tex2D(_SecondaryTex, (IN.worldPos.xy * _SecondaryTex_ST.xy) + (_Time[2] * _EffectSpeed));

				o.Albedo = (effectTexture * _EffectColor * _EffectPower) + tex2D(_MainTex, IN.uv_MainTex).rgb;

				o.Alpha = 1;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
