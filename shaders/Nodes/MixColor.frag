#version 330 core

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D inputTex1;
uniform sampler2D inputTex2;
uniform sampler2D factorTex;

void main()
{
    float a = texture2D(inputTex1, TexCoords).x;
    float b = texture2D(inputTex2, TexCoords).x;
    float fac = texture2D(factorTex, TexCoords).x;
    // apply custom logic
    vec4 color = vec4(vec3(a * (1 - fac) + b * fac), 1);
    FragColor = color;
}
