// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ObjectOutline"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "black" {}
    }

    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            sampler2D _MainTex;
            float2 _MainTex_TexelSize;

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvs : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvs = o.pos.xy / 2 + 0.5;

                return o;
            }

            half4 frag(v2f input) : COLOR
            {
                if (tex2D(_MainTex, input.uvs.xy).r > 0)
                {
                    discard;
                }

                int numIters = 9;

                float tx_x = _MainTex_TexelSize.x;
                float tx_y = _MainTex_TexelSize.y;

                float colorIntensityInRadius;

                for (int i = 0; i < numIters; i++)
                {
                    for (int j = 0; j < numIters; j++)
                    {
                        colorIntensityInRadius += tex2D(_MainTex, input.uvs.xy + float2((i - numIters / 2) * tx_x, (j - numIters / 2) * tx_y)).r;
                    }
                }

                return colorIntensityInRadius * half4(0, 1, 1, 1);
            }

            ENDCG
        }
    }
}
