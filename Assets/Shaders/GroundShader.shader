Shader "Custom/GroundShader"
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

            float circle(float2 p, float radius)
            {
                return abs(length(p) - radius);
            }

            float spiral(float2 p, float time, float scale)
            {
                float angle = atan2(p.y, p.x) + time * 0.1;
                float radius = length(p) * scale;
                return abs(length(p) - radius) - 0.1 * cos(angle);
            }

            float2 pattern2(float2 p, float scale, float time)
            {
                float2 uv = p;
                float2 id = floor(p * scale);
                p = frac(p * scale) - 0.5;

                float rnd = hash21(id);

                // Rotate tiles with random variations
                if (rnd > 0.5)
                    p = rot(PI * 0.5, p);
                rnd = frac(rnd * 32.54);
                if (rnd > 0.4)
                    p = rot(PI * 0.5, p);
                if (rnd > 0.8)
                    p = rot(PI * 0.5, p);

                // Define the shapes with more dynamic elements
                float b = box(p + float2(0.0, 0.7), float2(0.05, 0.25)) - 0.15;
                float r = box(p + float2(0.6, 0.0), float2(0.15, 0.05)) - 0.15;
                float c = circle(p, 0.6); // Circle pattern
                float s = spiral(p, time, 1.5); // Spiral pattern

                // Combine shapes with dynamic effects
                float d = min(min(b, r), c);
                float e = min(d, s);

                return float2(d, e);
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0; // Normalize UV to [-1, 1]
                float time = _Time.y * 0.1;

                // Apply transformations
                uv = rot(time * 0.095, uv);
                uv = float2(log(length(uv)), atan2(uv.y, uv.x)) * 1.272;

                float scale = 8.0;
                float3 C = float3(0.0, 0.0, 0.0);

                for (float i = 0; i < 4; i++) {
                    float ff = (i * 0.05) + 0.2;
                    uv.x += time * ff;

                    float px = fwidth(uv.x * scale);
                    float2 d = pattern2(uv, scale, time);
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
