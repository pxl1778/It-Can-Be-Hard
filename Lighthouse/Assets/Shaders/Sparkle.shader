Shader "Unlit/Sparkle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Scale("Scale", Float) = 1
		_Intensity("Intensity", Float) = 50
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 wPos:TEXCOORD1;
				float3 wNormal:TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _Scale;
			float _Intensity;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				//v.vertex = v.vertex + float4((v.normal * sin(v.vertex.y*10 + _Time[2])), 0); //fun weird wobbly thing
				v.vertex = v.vertex + float4(v.normal * saturate(sin(_Time[2] + v.vertex.y*80)*.1), 0);
				v.vertex = v.vertex + float4(-v.normal * saturate(sin(_Time[2] + v.vertex.x * 40)*.05), 0);
				//o.vertex = UnityObjectToClipPos(v.vertex+(v.normal * (sin(_Time[2])+.5)/15));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.wNormal = UnityObjectToWorldNormal(v.normal);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);

				fixed3 sparklemap = tex2D(_NoiseTex, i.uv*_Scale);
				sparklemap -= half3(0.5, 0.5, 0.5);
				sparklemap = normalize(normalize(sparklemap) + i.wNormal);
				half3 viewDirection = normalize(i.wPos - _WorldSpaceCameraPos);
				half sparkle = dot(-viewDirection, sparklemap);
				sparkle = pow(saturate(sparkle), _Intensity);
				half fres = 1 - dot(i.wNormal, -viewDirection)*2.4;
				fres = fres / 2;
				col += half4(fres, fres, 0, (.5 - (fres * 5)));
				if (col.a < 0) {
					col.a = 0;
				}
				col += half4(sparkle, sparkle, sparkle, 0);
				col = saturate(col);
				col.rgb = col.rgb * 1.8;
				return col;
			}
			ENDCG
		}
	}
}
