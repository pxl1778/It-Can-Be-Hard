Shader "Custom/AntlersInvisible" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_ObjectPoint("Object Point", Vector) = (0, 0, 0, 0)
		_Radius("Radius", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#pragma target 3.0

			sampler2D _CameraDepthTexture;
			fixed4 _Color;
			float4 _ObjectPoint;
			half _Radius;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 scrPos : SCREEN;
				float depth : DEPTH;
				float3 normal : NORMAL;
				float3 wpos : TEXCOORD1;
			};


			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				o.depth = (UnityObjectToClipPos(v.vertex) * -1).z * _ProjectionParams.w;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag (v2f i) : SV_TARGET
			{
				//half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
				//half4 foamLine = 1 - saturate(2 * (depth - i.scrPos.w));
				float dist = distance(i.wpos, _ObjectPoint);
				fixed4 col = _Color * step(dist, _Radius + 0.05);
				col = col * step(_Radius - 0.05, dist);
				return col;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
