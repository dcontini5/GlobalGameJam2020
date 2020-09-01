Shader "PostProcess"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_VSoft("Vignette Softness", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
			float4 _Color;
			float _VRadius;
			float _VSoft;

            fixed4 frag (v2f_img input) : SV_Target
            {
				float4 base = tex2D(_MainTex, input.uv);

				float distFromCenter = distance(input.uv.xy, float2(0.5, 0.5));

				float vignette = smoothstep(_VRadius, _VRadius - _VSoft, distFromCenter);

				base = saturate(base * vignette);

				base *= _Color;
				return base;
            }
            ENDCG
        }
    }
}
