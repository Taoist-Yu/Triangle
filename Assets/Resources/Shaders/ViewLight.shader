Shader "PostProcessing/Light"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				//卷积核
				float kernel [3][3] = {
					{ 1.0, 1.0, 1.0 },
					{ 1.0, -8.0, 1.0},
					{ 1.0, 1.0, 1.0}
				};

				fixed4 color;
				color = fixed4(0,0,0,0);
				for(float x=-1;x<=1;x++)
					for(int y=-1;y<=1;y++)
						color = color + kernel[x+1][y+1] * tex2D(_MainTex, i.uv + float2(x/100.0 , y/100.0));
				color /= 4.0;
				return color;
			}
			ENDCG
		}
	}
}
