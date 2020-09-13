// Crest Ocean System for URP

// Copyright 2019 Huw Bowles

Shader "Crest/Inputs/Dynamic Waves/Add Bump"
{
	Properties
	{
		_Amplitude( "Amplitude", float ) = 1
		_Radius( "Radius", float) = 3
	}

	SubShader
	{
		Pass
		{
			Blend One One
		
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerObject)
			float _Radius;
			float _SimCount;
			float _SimDeltaTime;
			float _Amplitude;
			CBUFFER_END

			struct Attributes
			{
				float3 positionOS : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 worldOffsetScaled : TEXCOORD0;
			};

			Varyings Vert(Attributes input)
			{
				Varyings o;
				o.positionCS = TransformObjectToHClip(input.positionOS);

				float3 worldPos = TransformObjectToWorld(input.positionOS);
				float3 centerPos = unity_ObjectToWorld._m03_m13_m23;
				o.worldOffsetScaled.xy = worldPos.xz - centerPos.xz;

				// shape is symmetric around center with known radius - fix the vert positions to perfectly wrap the shape.
				o.worldOffsetScaled.xy = sign(o.worldOffsetScaled.xy);
				float4 newWorldPos = float4(centerPos, 1.0);
				newWorldPos.xz += o.worldOffsetScaled.xy * _Radius;
				o.positionCS = mul(UNITY_MATRIX_VP, newWorldPos);

				return o;
			}

			float4 Frag(Varyings input) : SV_Target
			{
				// power 4 smoothstep - no normalize needed
				// credit goes to stubbe's shadertoy: https://www.shadertoy.com/view/4ldSD2
				float r2 = dot(input.worldOffsetScaled.xy, input.worldOffsetScaled.xy);
				if (r2 > 1.0)
					return (float4)0.0;

				r2 = 1.0 - r2;

				float y = r2 * r2;
				y = pow(y, 0.05);
				y *= _Amplitude;

				if (_SimCount > 0.0) // user friendly - avoid nans
					y /= _SimCount;

				// accelerate velocities
				return float4(0.0, _SimDeltaTime * y, 0.0, 0.0);
			}

			ENDHLSL
		}
	}
}
