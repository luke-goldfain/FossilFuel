Shader "Custom/CloseToBlack"
{
	Properties
	{
		//_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" }
			CGPROGRAM
			#pragma surface surf Lambert finalcolor:mycolor
			#include "UnityCG.cginc"

			struct Input
			{
				float2 uv_MainTex;

				float3 worldPos; 
				float3 viewDir;
			};

			//// vertex shader
			//v2f vert(appdata_base v)
			//{
			//	v2f o;
			//	o.pos = UnityObjectToClipPos(v.vertex);
			//	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			//	return o;
			//}

			//sampler2D _MainTex;

			void surf(Input IN, inout SurfaceOutput o)
			{
				//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

				//o.pos = UnityObjectToClipPos(v.vertex);
				//o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			}

			void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
			{
				float cameraDistxz = length(IN.worldPos.xz - _WorldSpaceCameraPos.xz);
				float cameraDisty = length(IN.worldPos.y - _WorldSpaceCameraPos.y);
				float cameraDistz = length(IN.worldPos.z - _WorldSpaceCameraPos.z);

				float ang = dot(normalize(IN.worldPos.xz - _WorldSpaceCameraPos.xz), IN.viewDir);

				if (IN.viewDir.y < .5 && ang > -.2 && ang < .2)
				{
					color.xyz = 0;
				}
				else //color.xyz = color.xyz * cameraDist / 3;
				{
					color.xyz = .6 / (cameraDisty * .1);
				}
			}
			ENDCG
		}
			Fallback "Diffuse"
	
}