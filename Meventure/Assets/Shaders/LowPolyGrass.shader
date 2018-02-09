Shader "Custom/LowPolyGrass"
{
	Properties
	{
		_Length ("Length", Float) = 1.0
		_Width ("Width", Float) = .5
		_Gravity ("Gravity", Float) = 1.0
		[PerRendererData]_Color("Overall Diffuse Color Filter", Color) = (1,1,1,1)
		[PerRendererData]_BottomColor("Bottom Diffuse Color", Color) = (1, 1, 1, 1)
		_RandomTex ("Random Texture", 2D) = "white" {}
		_MaxDistance("Max Distance", Float) = 1.0
		_ObjectPoint("Object Point", Vector) = (0, 0, 0, 0)
		_GlowObjectPoint("Glow Object Point", Vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		LOD 100

		Pass
		{
		Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			// make fog work
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

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
			uniform float4 _Color; 
			uniform float4 _BottomColor; 

			sampler2D _RandomTex;
			float _Length;
			float _Width;
			float _Gravity;
			float4 _ObjectPoint;
			float4 _GlowObjectPoint;

			float _MaxDistance;

			float rand1(float3 co)
			{
				return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
			}
			
			float rand2(float3 co)
			{
				return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
			}
			
			v2g vert (appdata_full v)
			{
				v2g o;
				o.pos = v.vertex;
				o.uv = v.texcoord;
				o.normal = float4(v.normal, 0.0f);
				return o;
			}

			[maxvertexcount(9)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream){
				float4 p1 = i[0].pos;
				float4 p2 = i[1].pos;
				float4 p3 = i[2].pos;

				float4 N1 = i[0].normal;
				float4 N2 = i[1].normal;
				float4 N3 = i[2].normal;

				float4 P = (p1 + p2 + p3)/3.0;
				float4 N = (N1 + N2 + N3)/3.0f;
				float4 topPoint = (N * _Length) + P;

				float3 worldPosition = mul(unity_ObjectToWorld, P).xyz;
				float4 topColor = _Color;

				float dist = distance(worldPosition, _ObjectPoint);
				float glowDist = distance(worldPosition, _GlowObjectPoint);
				if(dist < _MaxDistance){
					float invert = 1 - (dist/_MaxDistance);
					float3 differenceVector = worldPosition - _ObjectPoint;
					worldPosition += normalize(differenceVector) * (_MaxDistance * invert);
					float3 offset = mul(unity_WorldToObject, float4(worldPosition, 1.0));
					topPoint = float4(offset.x, offset.y, topPoint.z, topPoint.w);
				}
				if(glowDist < 1.7){
				if(length(_GlowObjectPoint) != 0.0){
				
				
					float invert = 1 - (glowDist/1.7);
					float3 differenceVector = worldPosition - _GlowObjectPoint;
					worldPosition += normalize(differenceVector) * (.6 * invert);
					float3 offset = mul(unity_WorldToObject, float4(worldPosition, 1.0));
					topPoint = float4(offset.x, offset.y, topPoint.z, topPoint.w);
					if(invert > .05){
						topColor = float4(1, 1, 1, 1)*8*invert;
					}
				}}
				//topPoint = topPoint + (float4(sin(_Time[1] * 50 * P.x * P.z),sin(_Time[1] * rand1(P.xyz)), 0, 0)/20);

				topPoint = topPoint + float4(rand1(P.xyz)-.5, rand2(P.xyz)-.5, rand2(p1), 0.0)/10;// + (tex2Dlod(_RandomTex, float4(P.xy, 0, 0)/float4(512, 512, 0.0, 0.0)) - float4(.5, .5, 0, 0));
				float2 PUV = ((P.xy + 1.0)/2); ///iffy wind randomness that only kinda works

				float4 col = tex2Dlod(_RandomTex, float4((P.x+_Time[1]/5),P.y + _Time[1]/5, 0, 0));
				col = col - float4(.3, .3, 0, 0);
				col = col/4;
				
				topPoint = topPoint + float4(col.x, col.y, 0, 0);

				p1 = p1 + (P-p1)*_Width;
				p2 = p2 + (P-p2)*_Width;
				p3 = p3 + (P-p3)*_Width;

				float4 positions[3] = {p1, p2, p3};
				
				g2f OUT;
				for(int i = 0; i < 3; i++){
					for(int j=0; j < 3; j++){
						if(j == 2){
							OUT.pos = UnityObjectToClipPos(topPoint);
							OUT.col = topColor;
						}
						else{
							float index = fmod(i+j, 3);
							OUT.pos = UnityObjectToClipPos(positions[index]);
							OUT.col = _BottomColor;
						}
						triStream.Append(OUT);
					}
				}				
			}
			
			fixed4 frag (g2f i) : SV_Target
			{
				fixed4 col = i.col * UNITY_LIGHTMODEL_AMBIENT.rgba;
				return col;
			}
			ENDCG
		}

		/*Pass
		{
			Tags { "LightMode" = "ForwardAdd" }
			Blend One One 


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd
			
			#include "UnityCG.cginc"
            #include "AutoLight.cginc"

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
			uniform float4 _Color; 
			uniform float4 _BottomColor; 

			sampler2D _RandomTex;
			float _Length;
			float _Width;
			float _Gravity;
			float4 _ObjectPoint;

			float _MaxDistance;

			float rand1(float3 co)
			{
				return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
			}
			
			float rand2(float3 co)
			{
				return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
			}
			
			v2g vert (appdata_full v)
			{
				v2g o;
				o.pos = v.vertex;
				o.uv = v.texcoord;
				o.normal = float4(v.normal, 0.0f);
				return o;
			}

			[maxvertexcount(9)]
			void geom(triangle v2g i[3], inout TriangleStream<g2f> triStream){
				float4 p1 = i[0].pos;
				float4 p2 = i[1].pos;
				float4 p3 = i[2].pos;

				float4 N1 = i[0].normal;
				float4 N2 = i[1].normal;
				float4 N3 = i[2].normal;

				float4 P = (p1 + p2 + p3)/3.0;
				float4 N = (N1 + N2 + N3)/3.0f;
				float4 topPoint = (N * _Length) + P;

				float3 worldPosition = mul(unity_ObjectToWorld, P).xyz;

				float dist = distance(worldPosition, _ObjectPoint);
				
				if(dist < _MaxDistance){
					float invert = 1 - (dist/_MaxDistance);
					float3 differenceVector = worldPosition - _ObjectPoint;
					worldPosition += normalize(differenceVector) * (_MaxDistance * invert);
					float3 offset = mul(unity_WorldToObject, float4(worldPosition, 1.0));
					topPoint = float4(offset.x, offset.y, topPoint.z, topPoint.w);
				}
				//topPoint = topPoint + (float4(sin(_Time[1] * 50 * P.x * P.z),sin(_Time[1] * rand1(P.xyz)), 0, 0)/20);

				topPoint = topPoint + float4(rand1(P.xyz)-.5, rand2(P.xyz)-.5, rand2(p1), 0.0)/10;// + (tex2Dlod(_RandomTex, float4(P.xy, 0, 0)/float4(512, 512, 0.0, 0.0)) - float4(.5, .5, 0, 0));
				float2 PUV = ((P.xy + 1.0)/2); ///iffy wind randomness that only kinda works

				float4 col = tex2Dlod(_RandomTex, float4(P.x+_Time[1]/5,P.y + _Time[1]/5, 0, 0));
				col = col - float4(.3, .3, 0, 0);
				col = col/4;
				
				topPoint = topPoint + float4(col.x, col.y, 0, 0);

				p1 = p1 + (P-p1)*_Width;
				p2 = p2 + (P-p2)*_Width;
				p3 = p3 + (P-p3)*_Width;

				float4 positions[3] = {p1, p2, p3};
				
				g2f OUT;
				for(int i = 0; i < 3; i++){
					float3 normalDirection;
					if(i == 0){
						normalDirection = normalize(cross(p2 - topPoint, p1- topPoint));
					}
					if(i == 1){
						normalDirection = normalize(cross(p3 - topPoint, p2 - topPoint));
					}
					else{
						normalDirection = normalize(cross(p1 - topPoint, p3 - topPoint));
					}
					normalDirection = normalize(mul(float4(normalDirection, 0.0), unity_WorldToObject).xyz);
					for(int j=0; j < 3; j++){
						if(j == 2){
							OUT.pos = UnityObjectToClipPos(topPoint);
							OUT.col = _Color;
						}
						else{
							float index = fmod(i+j, 3);
							OUT.pos = UnityObjectToClipPos(positions[index]);
							OUT.col = _BottomColor;
						}
						float3 lightDirection;
						float attenuation;
						float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - mul(unity_ObjectToWorld, OUT.pos).xyz;
						float distance = length(vertexToLightSource);
						attenuation = 1.0 / distance; // linear attenuation 
						lightDirection = normalize(vertexToLightSource);
						float3 diffuseReflection = attenuation * _LightColor0.rgb * OUT.col.rgb * saturate( dot( normalDirection, lightDirection ) );
						OUT.col = float4(diffuseReflection, 1.0);
						triStream.Append(OUT);
					}
				}				
			}
			
			fixed4 frag (g2f i) : COLOR
			{
				fixed4 col = i.col;
				/*float3 posWorld = mul(unity_ObjectToWorld, i.pos);
				
				float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - posWorld );
				float3 lightDirection;
				float3 normalDirection = float3(0, 1, 0);
				float atten;
				if(0.0 == _WorldSpaceLightPos0.w){//dierctional light
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else{//point or spot light
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - posWorld;
					float distance = length(vertexToLightSource);
					atten = 1.0 / distance; 
					lightDirection = normalize(vertexToLightSource);
				}
				float3 diffuseReflection = atten * _LightColor0.rgb * saturate( dot( normalDirection, lightDirection ) );

				col.rgb += _LightColor0.rgb * (1/distance(_WorldSpaceLightPos0.xyz, posWorld));*/
				
				//col.rgb += unity_LightColor[0].rgb * (1/distance(unity_LightPosition[0].xyz, mul(unity_ObjectToWorld, i.pos)));
				// apply fog
				//return (col + float4(diffuseReflection, 0));*/
				//return col;
			//}
			//ENDCG

		//}*/
	}
}
