Shader "Compass"
{
    Properties
    {
		_Compass ("Compass", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		//Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _Compass;
			float4 _PlayerPos;
			float4 _StandPos;
			float4 _PlayerDir;

            float4 frag (v2f i) : SV_Target
            {
				float distance = length(_PlayerPos.xyz - _StandPos.xyz);

				float redDot = saturate(distance / 100.0f);

				float4 col = float4(1.0f, 1.0f, 1.0f, 1.0f);

				float2 dirPos = normalize(_PlayerPos.xyz - _StandPos.xyz).xz;

				float2 dirCam = normalize(float2(_PlayerDir.x, _PlayerDir.z));

				float angle = atan2(dirPos.y*dirCam.x - dirPos.x*dirCam.y, dirPos.x*dirCam.x + dirPos.y*dirCam.y);

				float2 pos = float2(0.0f, -redDot);

				float2x2 rot = float2x2(cos(angle), -sin(angle),
					sin(angle), cos(angle));

				pos = mul(rot, pos);

				float2 canvas = i.uv.xy * 2.0f - 1.0f;

				if (length(pos - canvas) < 0.05f)
				{
					col = float4(1.0f, 0.0f, 0.0f, 1.0f);
				}
				else
				{
					col = tex2D(_Compass, i.uv);
					// just invert the colors
					col.rgb = 1 - col.rgb;
				}

                return col;
            }
            ENDCG
        }
    }
}
