Shader "Unlit/Reflection"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		// _InverseX ("Inverse X", Float) = 0
		// _InverseY ("Inverse Y", Float) = 0
	}
	SubShader
	{   		
		Tags { "RenderType"="Opaque" }
 		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Utils.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				// float3 viewDir : TEXCOORD2;
				// float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CameraTexture;
			// samplerCUBE _PanoramaTexture;
			// float _PlayerDistance;
			// float _InverseX;
			// float _InverseY;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				// o.viewDir = WorldSpaceViewDir(v.vertex);
				// o.normal = mul(_Object2World, v.normal);
    		o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// float t = smoothstep(0.5, 1.0, _PlayerDistance);
				fixed4 col = tex2D(_CameraTexture, i.screenPos.xy / i.screenPos.w);
				// fixed4 pano = texCUBE(_PanoramaTexture, reflect(-normalize(i.viewDir), normalize(i.normal)));
				// return lerp(col, pano, t);
				return col;
			}
			ENDCG
		}
	}
}
