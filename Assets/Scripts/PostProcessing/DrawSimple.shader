// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DrawSimple"
{
    SubShader
    {
        ZWrite Off
        ZTest Always
        Lighting Off
        Pass
        {
            CGPROGRAM
            #pragma vertex VShader
            #pragma fragment FShader

            struct VertexToFragment
            {
                float4 pos : SV_POSITION;
            };

            VertexToFragment VShader(VertexToFragment i)
            {
                VertexToFragment o;
                o.pos = UnityObjectToClipPos(i.pos);
                return o;
            }

            half4 FShader() : COLOR0
            {
                return half4(1, 1, 1, 1);
            }

            ENDCG
        }
    }
}
