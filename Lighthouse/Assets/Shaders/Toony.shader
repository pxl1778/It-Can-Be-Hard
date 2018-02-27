// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Toony"
{
	Properties
	{
		_Color ("Diffuse", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			uniform float4 _LightColor0;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
			};

			float4 _Color;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color;
				float NdotL1 = 0;
				if (0.0 == _WorldSpaceLightPos0.w) {
					NdotL1 = saturate(dot(i.normal, normalize(_WorldSpaceLightPos0.xyz)));
				}
				if (NdotL1 > .8) {
					NdotL1 = 1.0;
				}
				else if (NdotL1 > .6) {
					NdotL1 = .6;
				}
				else if (NdotL1 > .3) {
					NdotL1 = .3;
				}
				else {
					NdotL1 = 0;
				}
				
				float4 lightColor = (float4(NdotL1, NdotL1, NdotL1, 1)*float4(_LightColor0.rgb, 1));
				return (col * lightColor) + (UNITY_LIGHTMODEL_AMBIENT.rgba * _Color);
			}
			ENDCG
		}
	}
}
