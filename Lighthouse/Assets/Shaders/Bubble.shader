Shader "Unlit/Bubble"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", COLOR) = (0, 0, 0, 0)
		_Rim("Rim Color", COLOR) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

		Blend One One
		ZWrite Off
		Cull Off

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
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos : SCREEN;
				float depth : DEPTH;
				float3 normal : NORMAL;
				float3 wpos : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Rim;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.scrPos = ComputeScreenPos(o.vertex);
				o.depth = (UnityObjectToClipPos(v.vertex) * -1).z * _ProjectionParams.w;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = _Color * _Color.a;
				half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos))); //depth
				half4 foamLine = 1 - saturate(2 * (depth - i.scrPos.w));
				half3 viewDirection = normalize(i.wpos - _WorldSpaceCameraPos);
				half rim = (1.0 -  abs(dot(i.normal, -viewDirection))) * 2.4;
				fixed4 col = foamLine * _Color.a * 2;
				col += rim * _Rim * _Color.a;
				return col;
			}
			ENDCG
		}
	}
}
