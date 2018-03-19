Shader "Unlit/simpleWaterShader"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,1)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Range(0, 20)) = 1.0
		_WaveLength("Wave length", Float) = 0.5
		_WaveHeight("Wave height", Float) = 0.5
		_WaveSpeed("Wave speed", Float) = 1.0
		_RandomHeight("Random height", Float) = 0.5
		_RandomSpeed("Random Speed", Float) = 0.5
	}
	SubShader
	{

		Pass
		{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "LightMode" = "ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma vertex vert
		#pragma fragment frag

		float rand(float3 co)
		{
			return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
		}

		float rand2(float3 co)
		{
			return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
		}

		float _WaveLength;
		float _WaveHeight;
		float _WaveSpeed;
		float _RandomHeight;
		float _RandomSpeed;

		uniform float4 _LightColor0;

		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 norm : NORMAL;
			float3 diffuseColor : TEXCOORD1;
			float3 specularColor : TEXCOORD2;
			float3 wpos : WORLD;
		};

		v2f vert(appdata_full v)
		{
			float4 v0 = mul(unity_ObjectToWorld, v.vertex);

			float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
			float phase0_1 = (_RandomHeight)*sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));

			v0.y += phase0 + phase0_1;

			v.vertex = mul(unity_WorldToObject, v0);

			float4x4 modelMatrix = unity_ObjectToWorld;
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 normalDirection = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
			float3 viewDirection = normalize(_WorldSpaceCameraPos - mul(modelMatrix, float4(v.vertex)).xyz);
			float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			float attenuation = 1.0;

			float3 ambientLighting =
				UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

			float3 diffuseReflection =
				attenuation * _LightColor0.rgb * _Color.rgb
				* saturate(dot(v.normal, -lightDirection));

			float3 specularReflection;
			if (dot(v.normal, lightDirection) < 0.0) {
				specularReflection = float3(0.0, 0.0, 0.0);
			}
			else {
				specularReflection = attenuation * _LightColor0.rgb
					* _SpecColor.rgb * saturate(pow(max(0.0, dot(
						reflect(-lightDirection, v.normal),
						viewDirection)), _Shininess));
			}

			v2f OUT;
			OUT.pos = UnityObjectToClipPos(v.vertex);
			OUT.norm = UnityObjectToWorldNormal(v.normal);
			OUT.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
			OUT.diffuseColor = ambientLighting + diffuseReflection;
			OUT.specularColor = specularReflection;
			return OUT;
		}


		fixed4 frag(v2f i) : SV_Target
		{
			half3 viewDirection = normalize(i.wpos - _WorldSpaceCameraPos);
			float3 lightPos = float3(unity_4LightPosX0[0], unity_4LightPosY0[0], unity_4LightPosZ0[0]);
			float3 lightDir = normalize(lightPos - i.wpos);
			float3 lightDistance = lightPos - i.wpos;
			float nDotL = saturate(dot(i.norm, lightDir));
			float attenuation = 1.0 / (1 + unity_4LightAtten0[0] * pow(dot(lightDistance, lightDistance), 2));
			float4 pointLight = nDotL * unity_LightColor[0] * attenuation;
			i.diffuseColor = _LightColor0.rgb * _Color.rgb * saturate(dot(i.norm, normalize(_WorldSpaceLightPos0.xyz)));
			return float4(i.specularColor + i.diffuseColor, 1.0);//, 1.3 - saturate(0.1f + dot(i.norm, -viewDirection))) + pointLight;
		}
			ENDCG
		}
	}
}
