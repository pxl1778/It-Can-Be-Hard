Shader "Unlit/Glitch"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "white" {}
		_Displacement("Displacement", Float) = 0
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float _Intensity;
			float _Displacement;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float gTime = 1.0f;//step(0.95, sin(_Time.w));
				float4 glitch = tex2D(_NoiseTex, i.uv + (_Time.w * .005));
				float2 uv = frac(i.uv + glitch.xy * ((_Displacement + _Time.w * .005) * gTime)/5);
				float4 source = tex2D(_MainTex, uv);
				return source;
			}
			ENDCG
		}
	}
}
