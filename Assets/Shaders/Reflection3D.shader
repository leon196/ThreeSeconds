Shader "Unlit/Reflection3D"
{
	Properties
	{
		_MainTex ("Texture", CUBE) = "white" {}
	}
	SubShader
	{   		
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
 		Pass {
	    Cull off
    	Blend SrcAlpha OneMinusSrcAlpha     
	    // ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Utils.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD1;
				float3 normal : NORMAL;
			};

			samplerCUBE _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = texCUBE(_MainTex, reflect(-normalize(i.viewDir), normalize(i.normal)));
				return col;
			}
			ENDCG
		}
	}
}
