Shader "PostProcessing/Light"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisMap("DistanceMap", 2D) = "white" {}
        _LightColor("LightColor", Color) = (1,1,1,1)
		_Dis("Dis",Range(0,1)) = 1
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
			sampler2D _DisMap;
			fixed4 _LightColor;
			float _Dis;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 color = fixed4(1,1,1,1);

				float d = tex2D(_DisMap, i.uv).r;
				d = saturate(d / _Dis);
				d = 1-d;


				return d * color * _LightColor * tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
