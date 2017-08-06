// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BeltShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UnscaledTime("_UnscaledTime", Float) = 0
		_MultiplierX("_MultiplierX", Float) = 0
	}
	SubShader
	{
		Tags {
			"Queue" = "Overlay+1"
			}


		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _UnscaledTime;
			float _MultiplierX;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);

				return OUT;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = float2(fmod(i.texcoord.x + _UnscaledTime * _MultiplierX, 1),
						i.texcoord.y);

				fixed4 col = tex2D(_MainTex, uv);
				col.rgb *= col.a;

				return col;
			}
			ENDCG
		}
	}
}