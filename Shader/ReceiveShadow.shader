//体积光渲染条件：体积光照射到的地方要有物体，否则体积光不渲染
//关于VR渲染时，希望接收体积光，但会拖影，只要让eye不渲染该物体即可。
Shader "Skode/ReceiveShadow" {
 
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
 
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
    LOD 200
    Blend Zero SrcColor
 
CGPROGRAM
 
#pragma surface surf ShadowOnly alphatest:_Cutoff
 
fixed4 _Color;
 
struct Input {
    float2 uv_MainTex;
};
 
inline fixed4 LightingShadowOnly (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
    fixed4 c;
    c.rgb = s.Albedo*atten;
    c.a = s.Alpha;
 
    return c;
}
 
void surf (Input IN, inout SurfaceOutput o) 
{
    fixed4 c = _Color; 
    o.Albedo = c.rgb;
    o.Alpha = 1;
}
 
ENDCG
}
 
Fallback "Transparent/Cutout/VertexLit"
}