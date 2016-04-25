Shader "Hidden/Filter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_InverseX ("Inverse X", Float) = 0
		_InverseY ("Inverse Y", Float) = 0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _InverseX;
			float _InverseY;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float2 uv = i.uv;
				uv.x = lerp(uv.x, 1.0 - uv.x, _InverseX);
				uv.y = lerp(uv.y, 1.0 - uv.y, _InverseY);
				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
