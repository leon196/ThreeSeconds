Shader "Custom/Reflection" {
	Properties {
		_MainTex ("Texture", CUBE) = "white" {}
	}
	SubShader
	{   		
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert
		#include "UnityCG.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		samplerCUBE _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 normal;
		};

    void vert (inout appdata_full v, out Input o) {
        UNITY_INITIALIZE_OUTPUT(Input,o);
        o.normal = v.normal;
    }

		void surf (Input i, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			// fixed4 c = tex2D (_MainTex, i.uv_MainTex) * _Color;
			// o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			// o.Metallic = _Metallic;
			// o.Smoothness = _Glossiness;
			o.Emission = texCUBE(_MainTex, reflect(-normalize(i.viewDir), normalize(i.normal))).rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
