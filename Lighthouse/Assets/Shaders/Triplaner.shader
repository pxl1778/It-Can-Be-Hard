// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Triplaner" {
	Properties {
		_MainTex ("Albedo1 (RGB)", 2D) = "white" {}
		_SecondaryTex("Albedo2 (RGB)", 2D) = "white" {}
		_RandomTex("Random", 2D) = "white" {}
		_GrassSpread("Grass Spread", Range(-2, 2)) = 0.0
		_EdgeWidth("Edge Width", Range(-1, 1)) = 0.0
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _SecondaryTex;
		float4 _SecondaryTex_ST;
		sampler2D _RandomTex;
		float4 _RandomTex_ST;
		float _GrassSpread;
		float _EdgeWidth;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos; // world position built-in value
			float3 worldNormal; // world normal built-in value
			float3 viewDir;// view direction built-in value we're using for rimlight
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			//blended normal to modify textures to look projected onto the mesh.
			float3 blendedNormal = saturate(pow(IN.worldNormal.xyz * 1.4, 4));

			//randomTexture
			float3 rX = tex2D(_RandomTex, IN.worldPos.zy * _RandomTex_ST.xy);
			float3 rY = tex2D(_RandomTex, IN.worldPos.xz * _RandomTex_ST.xy);
			float3 rZ = tex2D(_RandomTex, IN.worldPos.xy * _RandomTex_ST.xy);

			float3 noiseTexture = rZ;
			noiseTexture = lerp(noiseTexture, rX, blendedNormal.x);
			noiseTexture = lerp(noiseTexture, rY, blendedNormal.y);

			//secondaryTexture
			float3 sX = tex2D(_SecondaryTex, IN.worldPos.zy * _SecondaryTex_ST.xy);
			float3 sY = tex2D(_SecondaryTex, IN.worldPos.xz* _SecondaryTex_ST.xy);
			float3 sZ = tex2D(_SecondaryTex, IN.worldPos.xy * _SecondaryTex_ST.xy);

			float3 topTexture = sZ;
			topTexture = lerp(topTexture, sX, blendedNormal.x);
			topTexture = lerp(topTexture, sY, blendedNormal.y);
			
			//mainTexture
			float3 mX = tex2D(_MainTex, IN.worldPos.zy * _MainTex_ST.xy);
			float3 mY = tex2D(_MainTex, IN.worldPos.xz* _MainTex_ST.xy);
			float3 mZ = tex2D(_MainTex, IN.worldPos.xy * _MainTex_ST.xy);

			float3 mainTexture = mZ;
			mainTexture = lerp(mainTexture, mX, blendedNormal.x);
			mainTexture = lerp(mainTexture, mY, blendedNormal.y);

			//get randomized normal value to make line between the two textures less hard
			float randomDot = dot(o.Normal + (noiseTexture.y + (noiseTexture * 0.5)), IN.worldNormal.y);

			//Results
			float3 topTextureResult = topTexture * step(_GrassSpread, randomDot);
			float3 mainTextureResult = mainTexture * step(randomDot, _GrassSpread);
			float3 edgeResult = (step(_GrassSpread, randomDot) * step(randomDot, _GrassSpread - _EdgeWidth) * -.15);

			o.Albedo = topTextureResult + mainTextureResult + edgeResult;

			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
