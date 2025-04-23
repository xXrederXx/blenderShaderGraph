#version 330 core

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D inputTex1;
uniform sampler2D inputTex2;
uniform sampler2D factorTex;
uniform int type;
// Utility functions
vec3 RGBToHSV(vec3 c) {
    float Cmax = max(c.r, max(c.g, c.b));
    float Cmin = min(c.r, min(c.g, c.b));
    float delta = Cmax - Cmin;
    float H = 0.0;
    
    if (delta != 0.0) {
        if (Cmax == c.r) {
            H = mod((c.g - c.b) / delta, 6.0);
        } else if (Cmax == c.g) {
            H = (c.b - c.r) / delta + 2.0;
        } else {
            H = (c.r - c.g) / delta + 4.0;
        }
        H /= 6.0;
    }

    float S = Cmax == 0.0 ? 0.0 : delta / Cmax;
    float V = Cmax;
    
    return vec3(H, S, V);
}

vec3 HSVToRGB(vec3 hsv) {
    float H = hsv.x * 6.0;
    float S = hsv.y;
    float V = hsv.z;

    int i = int(floor(H));
    float f = H - float(i);
    float p = V * (1.0 - S);
    float q = V * (1.0 - S * f);
    float t = V * (1.0 - S * (1.0 - f));

    if (i == 0) return vec3(V, t, p);
    else if (i == 1) return vec3(q, V, p);
    else if (i == 2) return vec3(p, V, t);
    else if (i == 3) return vec3(p, q, V);
    else if (i == 4) return vec3(t, p, V);
    else return vec3(V, p, q);
}

// Blend modes
vec3 mixBlend(vec3 a, vec3 b, float fac) {
    return mix(a, b, fac);
}

vec3 hueBlend(vec3 a, vec3 b, float fac) {
    vec3 ahsv = RGBToHSV(a);
    vec3 bhsv = RGBToHSV(b);
    ahsv.x = mix(ahsv.x, bhsv.x, fac);
    return HSVToRGB(ahsv);
}

vec3 saturationBlend(vec3 a, vec3 b, float fac) {
    vec3 ahsv = RGBToHSV(a);
    vec3 bhsv = RGBToHSV(b);
    ahsv.y = mix(ahsv.y, bhsv.y, fac);
    return HSVToRGB(ahsv);
}

vec3 valueBlend(vec3 a, vec3 b, float fac) {
    vec3 ahsv = RGBToHSV(a);
    vec3 bhsv = RGBToHSV(b);
    ahsv.z = mix(ahsv.z, bhsv.z, fac);
    return HSVToRGB(ahsv);
}

vec3 darkenBlend(vec3 a, vec3 b, float fac) {
    vec3 c = min(a, b);
    return mix(a, c, fac);
}

vec3 lightenBlend(vec3 a, vec3 b, float fac) {
    float gA = dot(a, vec3(0.299, 0.587, 0.114));
    float gB = dot(b, vec3(0.299, 0.587, 0.114));
    vec3 c = gA > gB ? a : b;
    return mix(a, c, fac);
}

vec3 linearLightBlend(vec3 a, vec3 b, float fac) {
    vec3 result = clamp(a + 2.0 * b - 1.0, 0.0, 1.0);
    return mix(a, result, fac);
}

// Dispatcher
vec3 mixColor(vec3 a, vec3 b, float fac, int mode) {
    if (mode == 0) return mixBlend(a, b, fac);
    else if (mode == 1) return hueBlend(a, b, fac);
    else if (mode == 2) return saturationBlend(a, b, fac);
    else if (mode == 3) return valueBlend(a, b, fac);
    else if (mode == 4) return darkenBlend(a, b, fac);
    else if (mode == 5) return linearLightBlend(a, b, fac);
    else if (mode == 6) return lightenBlend(a, b, fac);
    else return a;
}


void main() {
    vec3 a = texture2D(inputTex1, TexCoords).rgb;
    vec3 b = texture2D(inputTex2, TexCoords).rgb;
    float fac = texture2D(factorTex, TexCoords).x;
    FragColor = vec4(mixColor(a, b, fac, type), 0.);
}
