#version 330 core

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D inputTex;

void main()
{
    float val = texture2D(inputTex, TexCoords).x;
    // apply custom logic
    vec4 color = vec4(vec3(1-val),  1.);
    FragColor = color;
}
