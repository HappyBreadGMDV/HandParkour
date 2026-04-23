// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader "psx/vertexlit" {
// 	Properties{
// 		_MainTex("Base (RGB)", 2D) = "white" {}
// 	}
// 		SubShader{
// 			Tags { "RenderType" = "Opaque" }
// 			LOD 200

// 			Pass {
// 			Lighting On
// 				CGPROGRAM

// 					#pragma vertex vert
// 					#pragma fragment frag
// 					#include "UnityCG.cginc"

// 					struct v2f
// 					{
// 						fixed4 pos : SV_POSITION;
// 						half4 color : COLOR0;
// 						half4 colorFog : COLOR1;
// 						float2 uv_MainTex : TEXCOORD0;
// 						half3 normal : TEXCOORD1;
// 					};

// 					float4 _MainTex_ST;
// 					uniform half4 unity_FogStart;
// 					uniform half4 unity_FogEnd;

// 					v2f vert(appdata_full v)
// 					{
// 						v2f o;

// 						Vertex snapping
// 						float4 snapToPixel = UnityObjectToClipPos(v.vertex);
// 						float4 vertex = snapToPixel;
// 						vertex.xyz = snapToPixel.xyz / snapToPixel.w;
// 						vertex.x = floor(160 * vertex.x) / 160;
// 						vertex.y = floor(120 * vertex.y) / 120;
// 						vertex.xyz *= snapToPixel.w;
// 						o.pos = vertex;

// 						Vertex lighting 
// 						o.color =  float4(ShadeVertexLights(v.vertex, v.normal), 1.0);
// 						o.color = float4(ShadeVertexLightsFull(v.vertex, v.normal, 4, true), 1.0);
// 						o.color *= v.color;

// 						float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

// 						Affine Texture Mapping
// 						float4 affinePos = vertex; vertex;				
// 						o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
// 						o.uv_MainTex *= distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;
// 						o.normal = distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;

// 						Fog
// 						float4 fogColor = unity_FogColor;

// 						float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
// 						o.normal.g = fogDensity;
// 						o.normal.b = 1;

// 						o.colorFog = fogColor;
// 						o.colorFog.a = clamp(fogDensity,0,1);

// 						Cut out polygons
// 						if (distance > unity_FogStart.z + unity_FogColor.a * 255)
// 						{
// 							o.pos.w = 0;
// 						}

// 						return o;
// 					}

// 					sampler2D _MainTex;

// 					float4 frag(v2f IN) : COLOR
// 					{
// 						half4 c = tex2D(_MainTex, IN.uv_MainTex / IN.normal.r)*IN.color;
// 						half4 color = c*(IN.colorFog.a);
// 						color.rgb += IN.colorFog.rgb*(1 - IN.colorFog.a);
// 						return color;
// 					}
// 				ENDCG
// 			}
// 	}
// }

Shader "psx/stable_world_tiled" {
    Properties {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Tiling("Texture Scale", Float) = 0.5
        _SnapRes("Snapping Resolution", Float) = 160.0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass {
            Lighting On
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR0;
                half4 colorFog : COLOR1;
            };

            sampler2D _MainTex;
            float _Tiling, _SnapRes;
            uniform half4 unity_FogStart, unity_FogEnd;

            v2f vert(appdata_full v) {
                v2f o;

                // 1. Стабильные Мировые UV (Triplanar-lite)
                // Текстура прилипает к миру, а не к мешу
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 blend = abs(worldNormal);
                
                // Выбираем лучшую плоскость для наложения (чтобы не тянуло на склонах)
                float2 uv;
                if (blend.y > blend.x && blend.y > blend.z) uv = worldPos.xz;
                else if (blend.x > blend.z) uv = worldPos.zy;
                else uv = worldPos.xy;

                o.uv = uv * _Tiling;

                // 2. Освещение
                o.color = float4(ShadeVertexLightsFull(v.vertex, v.normal, 4, true), 1.0) * v.color;

                // 3. PSX Vertex Snapping (Дрожание вершин)
                float4 snapPos = UnityObjectToClipPos(v.vertex);
                snapPos.xyz /= snapPos.w;
                snapPos.xy = floor(_SnapRes * snapPos.xy) / _SnapRes;
                snapPos.xyz *= snapPos.w;
                o.pos = snapPos;

                // 4. Туман
                float dist = length(mul(UNITY_MATRIX_MV, v.vertex));
                float fog = clamp((unity_FogEnd.x - dist) / (unity_FogEnd.x - unity_FogStart.x), 0, 1);
                o.colorFog = unity_FogColor;
                o.colorFog.a = fog;

                return o;
            }

            float4 frag(v2f i) : COLOR {
                half4 c = tex2D(_MainTex, i.uv) * i.color;
                // Смешивание с туманом
                return lerp(i.colorFog, c, i.colorFog.a);
            }
            ENDCG
        }
    }
}


