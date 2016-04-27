Shader "Unlit/Reflection3D"
{
	Properties
	{
		_MainTex ("Texture", CUBE) = "white" {}
		_InverseX ("Inverse X", Float) = 0
		_InverseY ("Inverse Y", Float) = 0
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
			float3 _RayDirection;
			float _RayDistance;
			float _InverseX;
			float _InverseY;
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				// v.vertex.x *= -1.0;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				// o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				// o.normal = normalize(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				// o.normal = mul(UNITY_MATRIX_MVP, v.normal);
				o.normal = mul(_Object2World, v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// fixed4 col = texCUBE(_MainTex, normalize(i.normal));
				// fixed4 col = texCUBE(_MainTex, reflect(normalize(i.normal), normalize(i.viewDir)));
				// fixed4 col = texCUBE(_MainTex, reflect(-i.viewDir, i.normal));
				// float3 normal = normalize(float3(i.normal.x, 0.0, i.normal.z));
				// fixed4 col = texCUBE(_MainTex, reflect(-normalize(i.viewDir), normalize(i.normal)));

				float t = smoothstep(0.0, 0.01, _RayDistance);
				float3 ray = _RayDirection;

				ray.y = 0.0;
				ray = normalize(ray);

				float3 normal = lerp(normalize(ray), normalize(i.normal), t);

				fixed4 col = texCUBE(_MainTex, reflect(-normalize(i.viewDir), normal));
				// fixed4 col = texCUBE(_MainTex, reflect(normalize(i.viewDir), normalize(i.normal)));
				return col;
			}
			ENDCG
		}
	}
}
