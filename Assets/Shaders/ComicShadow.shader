Shader "Custom/ComicShadow" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Direction ("Direction", Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Front
		ZWrite Off

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float4 _Direction;
			
			v2f vert (appdata_full v) {
				v2f o;
				float3 dir = normalize(_Direction);
				float r = dot(dir, v.normal) * 0.5 + 0.5;
				r = step(0.5, r);
				v.vertex.xyz += dir * r * 10.0;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = v.normal;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				float3 dir = normalize(float3(1.0, -1.0, 0.0));
				float r = dot(dir, i.normal) * 0.5 + 0.5;
				// fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				// fixed4 col = fixed4(1.0,1.0,1.0,1.0);
				// col.rgb *= r;
				return _Color;
			}
			ENDCG
		}
	}
}