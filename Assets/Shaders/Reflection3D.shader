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
	    ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Utils.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 localPos : COLOR;
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
				o.localPos = v.vertex;
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float d = dot(normalize(i.viewDir), normalize(i.normal));

				// float x = 0.0;

				float x = dot(normalize(i.viewDir), normalize(i.normal)) * 0.5 + 0.5;
				float y = (atan2(i.localPos.y, i.localPos.x) / PI) * 0.5 + 0.5;

				// x = 1.0 - x;

				float2 uv = float2(x, y);
				// float2 uv = float2(dot(normalize(i.viewDir), normalize(i.normal)), atan2(i.vertex.y, i.vertex.x));
				// uv.x = 1.0 - uv.x;
				// fixed4 col = tex2D(_MainTex, uv);
				// fixed4 col = tex2D(_MainTex, lerp(i.uv, uv, d));
				// fixed4 col = texCUBE(_MainTex, normalize(i.normal));
				fixed4 col = texCUBE(_MainTex, reflect(-normalize(i.viewDir), normalize(i.normal)));
				// fixed4 col = texCUBE(_MainTex, normalize(i.viewDir));
				return col;
			}
			ENDCG
		}
	}
}
