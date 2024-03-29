﻿//将物体变透明。可改变透明程度
Shader "Skode/AlphaShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaScale ("Alpha Scale", Range(0, 1)) = 1
    }
    SubShader
    {
        // 透明度混合队列为Transparent，所以Queue=Transparent
        // RenderType标签让Unity把这个Shader归入提前定义的组中，以指明该Shader是一个使用了透明度混合的Shader
        // IgonreProjector为True表明此Shader不受投影器（Projectors）影响
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            // 只渲染背面
            Cull Front
            // 关闭深度写入
            ZWrite Off
            // 开启混合模式，并设置混合因子为SrcAlpha和OneMinusSrcAlpha
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            // 用于决定调用clip函数时进行的透明度测试使用的判断条件
            fixed _AlphaScale;
            
            v2f vert (a2v v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldPos = normalize(i.worldPos);
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                // 纹素值
                fixed4 texColor = tex2D(_MainTex, i.uv);
                // 反射率
                fixed3 albedo =  texColor.rgb * _Color.rgb;
                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;
                // 漫反射
                fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
                // 返回颜色，透明度部分乘以我们设定的值
                return fixed4(ambient + diffuse, texColor.a * _AlphaScale);
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            
            // 只渲染前面
            Cull Back 
            // 关闭深度写入
            ZWrite Off
            // 开启混合模式，并设置混合因子为SrcAlpha和OneMinusSrcAlpha
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            // 用于决定调用clip函数时进行的透明度测试使用的判断条件
            fixed _AlphaScale;
            
            v2f vert (a2v v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldPos = normalize(i.worldPos);
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                // 纹素值
                fixed4 texColor = tex2D(_MainTex, i.uv);
                // 反射率
                fixed3 albedo =  texColor.rgb * _Color.rgb;
                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;
                // 漫反射
                fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
                // 返回颜色，透明度部分乘以我们设定的值
                return fixed4(ambient + diffuse, texColor.a * _AlphaScale);
            }
            ENDCG
        }
    }
}