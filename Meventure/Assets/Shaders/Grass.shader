Shader "Custom/Grass"
{
	Properties
	{
		_Length ("Length", Float) = 1.0
		_Width ("Width", Float) = .5
		_Gravity ("Gravity", Float) = 1.0
		_Steps ("Steps", Int) = 4.0
		_Color ("Overall Diffuse Color Filter", Color) = (1,1,1,1)

		_Noise ("Noise Texture", 2D) = "white" {}
		_DirectionIntensity ("Direction Random Intensity", Float) = 1.0
		_LengthIntensity ("Length Random Intensity", Float) = 1.0
	}
	SubShader
	{
		Tags {"LightMode" = "ForwardBase"}
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2g
			{
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
				float4 pos : SV_POSITION;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float4 col : TEXCOORD0;
				//float4 tex : TEXCOORD0;
				//float3 diffuseColor : TEXCOORD1;
				//float3 specularColor : TEXCOORD2;
			};

			uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
			// User-specified properties
			uniform float4 _Color; 
			uniform float4 _SpecColor; 
			uniform float _Shininess;

			// Randomization properties
			sampler2D _Noise;
			sampler2D _Noise_ST;
			float _DirectionIntensity;
			float _LengthIntensity;

			float _Length;
			float _Width;
			float _Gravity;
			float _Steps;
			
			float4 getColor(float3 normalDir){
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				return float4(_LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDir, lightDir)), 1.0);
			}
			
			v2g vert (appdata_full v)
			{
				v2g o;
				o.pos = v.vertex;
				o.uv = v.texcoord;
				o.normal = float4(v.normal, 0.0f);
				return o;
			}

			[maxvertexcount(120)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream){
				float4 p1 = i[0].pos;
				float4 p2 = i[1].pos;
				float4 p3 = i[2].pos;

				float4 N1 = i[0].normal;
				float4 N2 = i[1].normal;
				float4 N3 = i[2].normal;

				float4 P = (p1 + p2 + p3)/3.0;
				float4 N = (N1 + N2 + N3)/3.0f;
				float4 T = float4(normalize((p2-p1).xyz), 0.0);

				_Noise_ST = _Noise;

				float3 noise = tex2Dlod(_Noise, float4(P.xy, 0, 0)).xyz;

				float l = _Length + noise.r * _LengthIntensity;

				float3 noiseNormal = (noise * 2 - 1) * _DirectionIntensity;
				//N = normalize(float4((N+noiseNormal).xyz, 0));

				for(int i=0; i<_Steps; i++){
					float t0 = (float)i / _Steps;
					float t1 = (float)(i+1) / _Steps;

					float4 y0 = normalize(N - (float4(0, 0, _Length * t0, 0) * _Gravity * t0)) * (_Length * t0);
					float4 y1 = normalize(N - (float4(0,  0, _Length * t1,0) * _Gravity * t1)) * (_Length * t1);

					float4 w0 = T * lerp(_Width, 0, t0);
					float4 w1 = T * lerp(_Width, 0, t1);

					g2f OUT;
					OUT.pos = UnityObjectToClipPos((y0-w0) + P);
					OUT.col = getColor(normalize(mul(cross( (y0+w0) - (y0-w0), (y1-w1) - (y0-w0)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		  
												 
					OUT.pos = UnityObjectToClipPos((y0+w0) + P);
					OUT.col = getColor(normalize(mul(cross((y1-w1) - (y0+w0), (y0-w0) - (y0+w0)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		   
												  
					OUT.pos = UnityObjectToClipPos((y1-w1) + P);
					OUT.col = getColor(normalize(mul(cross( (y0-w0) - (y1-w1), (y0+w0) - (y1-w1)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		   
												  
					OUT.pos = UnityObjectToClipPos((y1+w1) + P);
					OUT.col = getColor(normalize(mul(cross( (y1-w1) - (y1+w1), (y0+w0) - (y1+w1)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		  
												  
					triStream.RestartStrip();	   
												   
					OUT.pos = UnityObjectToClipPos((y0-w0) + P);
					OUT.col = getColor(normalize(mul(cross((y0+w0) - (y0-w0), (y1-w1) - (y0-w0)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		 
												 
					OUT.pos = UnityObjectToClipPos((y1-w1) + P);
					OUT.col = getColor(normalize(mul(cross((y0-w0) - (y1-w1), (y0+w0) - (y1-w1)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		  
												   
					OUT.pos = UnityObjectToClipPos((y0+w0) + P);
					OUT.col = getColor(normalize(mul(cross((y1-w1) - (y0+w0), (y0-w0) - (y0+w0)), unity_WorldToObject).xyz));
					triStream.Append(OUT);		   
												   
					OUT.pos = UnityObjectToClipPos((y1+w1) + P);
					OUT.col = getColor(normalize(mul(cross((y1-w1) - (y1+w1), (y0+w0) - (y1+w1)), unity_WorldToObject).xyz));
					triStream.Append(OUT);

					triStream.RestartStrip();
				}
			}
			
			fixed4 frag (g2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = i.col;
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
