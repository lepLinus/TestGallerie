
// TODO break out more bits

#include "OceanGlobals.hlsl"
//#include "../OceanConstants.hlsl"
//#include "../OceanInternalInputs.hlsl"
#include "OceanHelpersNew.hlsl"
//#include "../OceanLODData.hlsl"

//#include "../OceanFoam.hlsl"
//#include "../OceanEmission.hlsl"
//#include "../OceanReflection.hlsl"

void OceanFragment_half
(
	in const float3 i_oceanPosScale0,
	in const float3 i_oceanPosScale1,
	in const float4 i_oceanParams0,
	in const float4 i_oceanParams1,
	in const float i_sliceIndex0,
	in const half3 i_diffuse,
	in const float3 i_positionWS,
	in const real2 i_positionSS,
	in const float i_viewZ,
	in const float i_sceneZ01,
	in const half3 i_mainLightDir,
	in const half3 i_mainLightCol,
	in const float i_lodAlpha,
	in const float2 i_positionXZWSUndisplaced,
	in const half i_oceanDepth,
	in const half i_foam,
	in const half2 i_shadow,
	in const half2 i_flow,
	out float3 o_albedo,
	out float3 o_emission,
	out float3 o_specular,
	out float o_smoothness,
	out float o_alpha
)
{
	o_albedo = 0.0;
	o_emission = i_diffuse;
	o_specular = 1.0;
	o_smoothness = 0.8;
	o_alpha = 1.0;

	const bool underwater = false; // IsUnderwater(facing);
	real3 view = normalize(_WorldSpaceCameraPos - i_positionWS);
	float sceneZ = LinearEyeDepth(i_sceneZ01, _ZBufferParams);

	// Feather alpha as depth of ray through water approaches 0 length
	//o_alpha = saturate((sceneZ - i_viewZ) / 0.2);

	const real2 shadow = 1.0
#if _SHADOWS_ON
		- input.n_shadow.zw
#endif
		;
//
//	// Foam - underwater bubbles and whitefoam
//	real3 bubbleCol = (half3)0.;
//////#if _FOAM_ON
////	real4 whiteFoamCol;
////#if !_FLOW_ON
////	ComputeFoam(i_foam, i_positionXZWSUndisplaced, i_positionWS.xz, n_pixel, i_viewZ, sceneZ, view, i_mainLightDir, i_mainLightCol, shadow.y, i_lodAlpha, bubbleCol, whiteFoamCol);
////#else
////	ComputeFoamWithFlow(input.flow, i_foam, i_positionXZWSUndisplaced, i_positionWS.xz, n_pixel, i_viewZ, sceneZ, view, i_mainLightDir, i_mainLightCol, shadow.y, i_lodAlpha, bubbleCol, whiteFoamCol);
////#endif // _FLOW_ON
//////#endif // _FOAM_ON
//
//	// Compute color of ocean - in-scattered light + refracted scene
//	real3 scatterCol = ScatterColour(i_oceanDepth, _WorldSpaceCameraPos, i_mainLightDir, view, shadow.x, underwater, true, i_mainLightCol, sss);
//	o_emission = OceanEmission(view, n_pixel, i_mainLightDir, i_positionSS, i_viewZ, i_positionSS, sceneZ, i_sceneZ01, bubbleCol, /*_Normals, _CameraDepthTexture,*/ underwater, scatterCol);
//
//	// Light that reflects off water surface
////#if _UNDERWATER_ON
////	if (underwater)
////	{
////		ApplyReflectionUnderwater(view, n_pixel, i_mainLightDir, shadow.y, input.foam_screenPosXYW.yzzw, scatterCol, reflAlpha, col);
////	}
////	else
////#endif
////	{
////		ApplyReflectionSky(view, n_pixel, i_mainLightDir, shadow.y, /*input.foam_screenPosXYW.yzzw,*/ i_viewZ, reflAlpha, col);
////	}
//
////	// Override final result with white foam - bubbles on surface
////#if _FOAM_ON
////	col = lerp(col, whiteFoamCol.rgb, whiteFoamCol.a);
////#endif
////
}
