Shader"Custom/GroundShader_spherical"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            #define PI 3.14159265359
            #define PI2 6.28318530718

            // Helper functions
            float2 rot(float a, float2 p) {
                float c = cos(a);
                float s = sin(a);
                return float2(c * p.x - s * p.y, s * p.x + c * p.y);
            }

            float3 hue(float t, float f) {
                return f + f * cos(PI2 * t * (float3(1.0, 0.75, 0.75) + float3(0.96, 0.57, 0.12)));
            }

            float hash21(float2 a) {
                return frac(sin(dot(a, float2(27.69, 32.58))) * 43758.53);
            }

            float box(float2 p, float2 b) {
                float2 d = abs(p) - b;
                return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
            }

            float2 pattern(float2 p, float scale) {
                float2 uv = p;
                float2 id = floor(p * scale);
                p = frac(p * scale) - 0.5;

                float rnd = hash21(id);

                // Rotate tiles
                if (rnd > 0.5) p = rot(PI * 0.5, p);
                rnd = frac(rnd * 32.54);
                if (rnd > 0.4) p = rot(PI * 0.5, p);
                if (rnd > 0.8) p = rot(PI * 0.5, p);

                // Adjust randomization
                rnd = frac(rnd * 47.13);
                float tk = 0.075;

                // Define shapes
                float d = box(p - float2(0.6, 0.7), float2(0.25, 0.75)) - 0.15;
                float l = box(p - float2(0.7, 0.5), float2(0.75, 0.15)) - 0.15;
                float b = box(p + float2(0.0, 0.7), float2(0.05, 0.25)) - 0.15;
                float r = box(p + float2(0.6, 0.0), float2(0.15, 0.05)) - 0.15;
                d = abs(d) - tk;

                l = abs(l) - tk;
                b = abs(b) - tk;
                r = abs(r) - tk;

                float e = min(d, min(l, min(b, r)));
                d = min(d, min(l, min(b, r)));

                return float2(d, e);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_ObjectToWorld));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert normal to spherical UV coordinates
                float3 n = normalize(i.worldNormal);
                float2 uv = float2(atan2(n.x, n.z) / PI2 + 0.5, acos(n.y) / PI);

                float time = _Time.y * 0.05;

                // Apply transformations
                uv = rot(time * 0.095, uv - 0.5) + 0.5; // Rotate around the sphere
                uv = float2(log(length(uv)), atan2(uv.y - 0.5, uv.x - 0.5)) * 1.272;

                float scale = 8.0;
                float3 C = float3(0.0, 0.0, 0.0);

                for (float i = 0; i < 4; i++) {
                    float ff = (i * 0.05) + 0.2;
                    uv.x += time * ff;

                    float px = fwidth(uv.x * scale);
                    float2 d = pattern(uv, scale);
                    float3 clr = hue(sin(uv.x + (i * 8.0)) * 0.2 + 0.4, (0.5 + i) * 0.15);

                    C = lerp(C, float3(0.001, 0.001, 0.001), smoothstep(px, -px, d.y - 0.04));
                    C = lerp(C, clr, smoothstep(px, -px, d.x));
                    scale *= 0.5;
                }

                // Gamma correction
                C = pow(C, float3(0.4545, 0.4545, 0.4545));
                return float4(C, 1.0);
            }
            ENDCG
        }
    }
}
