Shader "Unlit/unlitToon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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


			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : WORLD;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = normalize(mul(unity_ObjectToWorld, v.vertex).xyz);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				i.normal = normalize(i.normal);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos);
				float NdotL = max(0.0, dot(i.normal, lightDirection));

				half3 h = normalize(lightDirection + viewDirection);
				float nh = max(0, dot(i.normal, h));
				float spec = pow(nh, 10);

				float4 specLighting = float4(1.0, 1.0, 1.0, 1.0) * step(0.5, spec) * 0.4;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * step(0.01, NdotL) + (0.3 * tex2D(_MainTex, i.uv)) + specLighting;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
