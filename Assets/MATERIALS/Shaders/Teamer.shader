Shader "Custom/Teamer"
{
    Properties
    {
        _PlayerView ("Our Player", Int) = 3
        _PlayerNumber ("Player Number", Range(-1, 7)) = 3
        _AllyColor ("Ally Color", Color) = (0, 0, 1, 1)
        _EnemyColor ("Enemy Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

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
            fixed4 finalColor = (_PlayerView == _PlayerNumber) ? _AllyColor : _EnemyColor;
            o.Albedo = finalColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
