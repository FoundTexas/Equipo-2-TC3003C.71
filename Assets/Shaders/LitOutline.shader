Shader "Custom/LitOutline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _levels ("Levels", float) = 2
        _brightness ("Brightness", float) = 0
        _strength ("Strength", float) = 1
        _OutColor ("Outline Color", Color) = (1,1,1,1)
        _BorderWidth ("Rim effect", Range(-1, 1)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf SimpleLambert fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;

            float3 worldNormal;
            float3 viewDir;
        };

        fixed4 _Color;
        float _levels;
        float _brightness;
        float _strength;

        float _BorderWidth;
        fixed4 _OutColor;

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            float border = 1 - (abs(dot(IN.viewDir, IN.worldNormal)));
            
            float alpha = (border * (1-_BorderWidth));
            o.Alpha = c.a * alpha;
        }

        half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            float mult = _levels;
            half NdotL =  max(0, round(dot(s.Normal*mult, lightDir))/mult);
            half4 color;

            color.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            color.rgb += _brightness;
            
            float3 intensity = dot(color.rgb, lightDir);
            color.rgb = lerp(intensity, color.rgb, _strength);
            color.a = s.Alpha;

            return color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}