Shader "Unlit/Distortion"
{
	Properties
	{
		_DistortTex ("Distortion", 2D) = "white" {}
		_PerlinTex ("Perlin", 2D) = "white" {}
		_Color("Base Color", Color) = (0, 0, 0, 0)
		_AccentColor("Accent Color", Color) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

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

			sampler2D _DistortTex;
			sampler2D _PerlinTex;
			float4 _Color;
			float4 _AccentColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _PerlinTex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 Distort = tex2D(_DistortTex, i.uv/2);
				fixed4 col = tex2D(_PerlinTex, fixed2(i.uv.x - Distort.g/2 - _Time.x, i.uv.y - Distort.r - _Time.x * 5));
				col.a = (1-i.uv.y)*(col.r + .7);
				if (col.a > .5) {
					col.rgb = lerp(_Color.rgb, _Color.rgb + 0.9f, saturate(i.uv.y-.2f));
					col.a = 1;
				}
				else {
					if (col.a > .42) {
						col.rgb = _AccentColor.rgb;
						col.a = 1;
					}
					else {
						col.a = 0;
					}
				}
				return col;
			}
			ENDCG
		}
	}
}
