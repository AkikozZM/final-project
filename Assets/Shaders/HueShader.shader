Shader "Custom/HueShader"
{
    Properties
    {
        _BaseColor1 ("Base Color 1", Color) = (0.957, 0.804, 0.623, 1)
        _BaseColor2 ("Base Color 2", Color) = (0.192, 0.384, 0.933, 1)
        _OverlayColor1 ("Overlay Color 1", Color) = (0.910, 0.510, 0.8, 1)
        _OverlayColor2 ("Overlay Color 2", Color) = (0.350, 0.71, 0.953, 1)
        _TimeSpeed ("Time Speed", Float) = 2.0
        _Frequency ("Wave Frequency", Float) = 5.0
        _Amplitude ("Wave Amplitude", Float) = 30.0
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

            float4 _BaseColor1;
            float4 _BaseColor2;
            float4 _OverlayColor1;
            float4 _OverlayColor2;
            float _TimeSpeed;
            float _Frequency;
            float _Amplitude;

            float2 hash(float2 p)
            {
                p = float2(dot(p, float2(2127.1, 81.17)), dot(p, float2(1269.5, 283.37)));
                return frac(sin(p) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float2 u = f * f * (3.0 - 2.0 * f);

                float n = lerp(lerp(dot(-1.0 + 2.0 * hash(i + float2(0.0, 0.0)), f - float2(0.0, 0.0)),
                                    dot(-1.0 + 2.0 * hash(i + float2(1.0, 0.0)), f - float2(1.0, 0.0)), u.x),
                               lerp(dot(-1.0 + 2.0 * hash(i + float2(0.0, 1.0)), f - float2(0.0, 1.0)),
                                    dot(-1.0 + 2.0 * hash(i + float2(1.0, 1.0)), f - float2(1.0, 1.0)), u.x), u.y);
                return 0.5 + 0.5 * n;
            }

            float2 rotate(float2 p, float a)
            {
                float s = sin(a);
                float c = cos(a);
                return float2(c * p.x - s * p.y, s * p.x + c * p.y);
            }

            float smoothStep(float a, float b, float t)
            {
                t = saturate((t - a) / (b - a));
                return t * t * (3.0 - 2.0 * t);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float ratio = _ScreenParams.x / _ScreenParams.y;

                float2 tuv = uv - 0.5;

                // Noise-based rotation
                float degree = noise(float2(_Time.y * 0.1, tuv.x * tuv.y));
                tuv.y *= 1.0 / ratio;
                tuv = rotate(tuv, radians((degree - 0.5) * 720.0 + 180.0));
                tuv.y *= ratio;

                // Wave warp with sin
                float frequency = _Frequency;
                float amplitude = _Amplitude;
                float speed = _Time.y * _TimeSpeed;
                tuv.x += sin(tuv.y * frequency + speed) / amplitude;
                tuv.y += sin(tuv.x * frequency * 1.5 + speed) / (amplitude * 0.5);

                // Gradient layers
                float3 colorYellow = _BaseColor1.rgb;
                float3 colorDeepBlue = _BaseColor2.rgb;
                float3 layer1 = lerp(colorYellow, colorDeepBlue, smoothStep(-0.3, 0.2, (rotate(tuv, radians(-5.0))).x));

                float3 colorRed = _OverlayColor1.rgb;
                float3 colorBlue = _OverlayColor2.rgb;
                float3 layer2 = lerp(colorRed, colorBlue, smoothStep(-0.3, 0.2, (rotate(tuv, radians(-5.0))).x));

                // Final composition
                float3 finalComp = lerp(layer1, layer2, smoothStep(0.5, -0.3, tuv.y));

                return float4(finalComp, 1.0);
            }
            ENDCG
        }
    }
}
