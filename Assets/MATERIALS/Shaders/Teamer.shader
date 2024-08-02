Shader "Custom/Teamer"
{
    Properties
    {
        _PlayerView ("Our Player", Int) = 3
        _PlayerNumber ("Player Number", Range(-1, 7)) = 3
        _AllyColor ("Ally Color", Color) = (0, 1, 0, 1) 
        _EnemyColor ("Enemy Color", Color) = (1, 0, 0, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        int _PlayerView;
        int _PlayerNumber;
        fixed4 _AllyColor;
        fixed4 _EnemyColor;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 baseColor = (_PlayerView == _PlayerNumber) ? _AllyColor : _EnemyColor;
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * baseColor;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
