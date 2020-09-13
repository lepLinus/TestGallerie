
#include "../OceanConstants.hlsl"
#include "OceanGlobals.hlsl"
#include "OceanHelpersNew.hlsl"

void CrestNodeApplyCaustics_float
(
	in const half3 i_sceneColour,
	in const float3 i_scenePos,
	in const float i_waterSurfaceY,
	in const half3 i_lightDir,
	in const float i_sceneZ,
	in const Texture2D<float4> i_texture,
	in const half i_textureScale,
	in const half i_textureAverage,
	in const half i_strength,
	in const half i_focalDepth,
	in const half i_depthOfField,
	in const Texture2D<float4> i_distortion,
	in const half i_distortionStrength,
	in const half i_distortionScale,
	out half3 o_sceneColour
)
{
	o_sceneColour = i_sceneColour;

	half sceneDepth = i_waterSurfaceY - i_scenePos.y;

	// Compute mip index manually, with bias based on sea floor depth. We compute it manually because if it is computed automatically it produces ugly patches
	// where samples are stretched/dilated. The bias is to give a focusing effect to caustics - they are sharpest at a particular depth. This doesn't work amazingly
	// well and could be replaced.
	float mipLod = log2(i_sceneZ) + abs(sceneDepth - i_focalDepth) / i_depthOfField;

	// Project along light dir, but multiply by a fudge factor reduce the angle bit - compensates for fact that in real life
	// caustics come from many directions and don't exhibit such a strong directonality
	float2 surfacePosXZ = i_scenePos.xz + i_lightDir.xz * sceneDepth / (4.0*i_lightDir.y);
	float2 cuv1 = surfacePosXZ / i_textureScale + float2(0.044*_CrestTime + 17.16, -0.169*_CrestTime);
	float2 cuv2 = 1.37*surfacePosXZ / i_textureScale + float2(0.248*_CrestTime, 0.117*_CrestTime);
//
//	if (i_underwater)
//	{
//		// Add distortion if we're not getting the refraction
//		half2 causticN = _CausticsDistortionStrength * UnpackNormal(tex2D(i_normals, surfacePosXZ / _CausticsDistortionScale)).xy;
//		cuv1.xy += 1.30 * causticN;
//		cuv2.xy += 1.77 * causticN;
//	}

	half causticsStrength = i_strength;
//#if _SHADOWS_ON
//	{
//		real2 causticShadow = 0.0;
//		// As per the comment for the underwater code in ScatterColour,
//		// LOD_1 data can be missing when underwater
//		if (i_underwater)
//		{
//			const float3 uv_smallerLod = WorldToUV(surfacePosXZ);
//			SampleShadow(_LD_TexArray_Shadow, uv_smallerLod, 1.0, causticShadow);
//		}
//		else
//		{
//			// only sample the bigger lod. if pops are noticeable this could lerp the 2 lods smoothly, but i didnt notice issues.
//			float3 uv_biggerLod = WorldToUV_BiggerLod(surfacePosXZ);
//			SampleShadow(_LD_TexArray_Shadow, uv_biggerLod, 1.0, causticShadow);
//		}
//		causticsStrength *= 1.0 - causticShadow.y;
//	}
//#endif // _SHADOWS_ON

	o_sceneColour.xyz *= 1.0 + causticsStrength * (
		0.5 * i_texture.SampleLevel(sampler_Crest_linear_repeat, cuv1, mipLod).xyz +
		0.5 * i_texture.SampleLevel(sampler_Crest_linear_repeat, cuv2, mipLod).xyz
		- i_textureAverage);
}

float LinearToDeviceDepth(float linearDepth, float4 zBufferParam)
{
	//linear = 1.0 / (zBufferParam.z * device + zBufferParam.w);
	float device = (1.0 / linearDepth - zBufferParam.w) / zBufferParam.z;
	return device;
}

// HB - pull these two functions in, because the ComputeClipSpacePosition flips the UV and it kills the position :(
float4 CrestComputeClipSpacePosition(float2 positionNDC, float deviceDepth)
{
	return float4(positionNDC * 2.0 - 1.0, deviceDepth, 1.0);
}
float3 CrestComputeWorldSpacePosition(float2 positionNDC, float deviceDepth, float4x4 invViewProjMatrix)
{
	float4 positionCS = CrestComputeClipSpacePosition(positionNDC, deviceDepth);
	float4 hpositionWS = mul(invViewProjMatrix, positionCS);
	return hpositionWS.xyz / hpositionWS.w;
}

// We take the unrefracted scene colour (i_sceneColourUnrefracted) as input because having a Scene Colour node in the graph
// appears to be necessary to ensure the scene colours are bound?
void CrestNodeSceneColour_half
(
	in const half i_refractionStrength,
	in const half3 i_scatterCol,
	in const half3 i_normalTS,
	in const float4 i_screenPos,
	in const float i_pixelZ,
	in const half3 i_sceneColourUnrefracted,
	in const float i_sceneZ,
	out half3 o_sceneColour,
	out float o_sceneDistance,
	out float3 o_scenePositionWS
)
{
	//#if _TRANSPARENCY_ON

	// View ray intersects geometry surface either above or below ocean surface

	const bool i_underwater = false; // TODO

	// Depth fog & caustics - only if view ray starts from above water
	if (!i_underwater)
	{
		const half2 refractOffset = i_refractionStrength * i_normalTS.xy * min(1.0, 0.5*(i_sceneZ - i_pixelZ)) / i_sceneZ;
		float4 screenPosRefract = i_screenPos + float4(refractOffset, 0.0, 0.0);
		const float sceneZRefractDevice = SHADERGRAPH_SAMPLE_SCENE_DEPTH(screenPosRefract.xy);
		float sceneZRefract = LinearEyeDepth(sceneZRefractDevice, _ZBufferParams);

		// Compute depth fog alpha based on refracted position if it landed on an underwater surface, or on unrefracted depth otherwise
		if (sceneZRefract > i_pixelZ)
		{
			o_sceneDistance = sceneZRefract - i_pixelZ;

			o_sceneColour = SHADERGRAPH_SAMPLE_SCENE_COLOR(screenPosRefract.xy);

			o_scenePositionWS = CrestComputeWorldSpacePosition(screenPosRefract.xy, sceneZRefractDevice, UNITY_MATRIX_I_VP);
		}
		else
		{
			// It seems that when MSAA is enabled this can sometimes be negative
			o_sceneDistance = max(i_sceneZ - i_pixelZ, 0.0);

			o_sceneColour = i_sceneColourUnrefracted;

			float sceneZRefractDevice = LinearToDeviceDepth(i_sceneZ, _ZBufferParams);
			o_scenePositionWS = CrestComputeWorldSpacePosition(i_screenPos.xy, sceneZRefractDevice, UNITY_MATRIX_I_VP);
		}
	}
	//	else
	//	{
	//		// No fog behind water interface as we're under the water, so behind interface is air
	//		const half2 refractOffset = _RefractionStrength * i_n_pixel.xz;
	//		const half2 uvBackgroundRefract = uvBackground + refractOffset;
	//
	//		o_sceneColour = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uvBackgroundRefract).rgb;
	//		// Long ray from water surface to air - this is approx the max value of a half.
	//		o_rayLength = 65000.0;
	//
	//		// Not hooked up yet
	//		o_scenePositionWS = 0.0;
	//	}

	//#endif // _TRANSPARENCY_ON
}
