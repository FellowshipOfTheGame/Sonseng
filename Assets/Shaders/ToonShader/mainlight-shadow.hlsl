

void GetLightingInformation_float(float3 WorldPos ,out float3 Direction, out float3 Color,
    out half DistanceAtten, out half ShadowAtten)
{
    #ifdef SHADERGRAPH_PREVIEW
        Direction = float3(-0.5,0.5,-0.5);
        Color = float3(1,1,1);
        DistanceAtten = 1;
        ShadowAtten = 1;
    #else
        #if SHADOWS_SCREEN
            half4 clipPos = TransformWorldToHClip(WorldPos);
            half4 shadowCoord = ComputeScreenPos(clipPos);
        #else
            half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
        #endif
        Light light = GetMainLight(shadowCoord);
        Direction = light.direction;
        DistanceAtten = light.distanceAttenuation;
        Color = light.color;
        ShadowAtten = light.shadowAttenuation;
    #endif
}