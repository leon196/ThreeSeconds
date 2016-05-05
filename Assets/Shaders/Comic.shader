Shader "Custom/Comic" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Ramp ("Ramp", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf SimpleLambert fullforwardshadows

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _Ramp;

		half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = dot (s.Normal, lightDir);
      half diff = NdotL * 0.5 + 0.5;
      // half3 ramp = tex2D (_Ramp, float2(diff, diff)).rgb;
      float d = 4.0;
      half3 ramp = half3(1.0, 1.0, 1.0) * (floor(diff * d) / d);
      half4 c;
      c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
			// c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float col = Luminance(tex2D (_MainTex, IN.uv_MainTex).rgb);
			float d = 4.0;
			o.Albedo = half3(1.0, 1.0, 1.0) * (floor(col * d) / d);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
