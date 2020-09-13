// Crest Ocean System for URP

// Copyright 2019 Huw Bowles

Shader "Crest/Inputs/Foam/Add From Texture"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Strength( "Strength", float ) = 1
	}

	SubShader
	{
		// base simulation runs on the Geometry queue, before this shader.
		// this shader adds interaction forces on top of the simulation result.
		Tags { "Queue" = "Transparent" }
		Blend One One

		Pass
		{
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			sampler2D _MainTex;

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerObject)
			float4 _MainTex_ST;
			float _Radius;
			float _Amplitude;
			CBUFFER_END

			struct Attributes
			{
				float3 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Varyings Vert(Attributes input)
			{
				Varyings o;
				o.positionCS = TransformObjectToHClip(input.positionOS);
				o.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return o;
			}

			float4 Frag(Varyings input) : SV_Target
			{
				return tex2D(_MainTex, input.uv);
			}

			ENDHLSL
		}
	}
}
