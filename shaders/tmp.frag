#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform float sizeX;
uniform float sizeY;
uniform float width;
uniform float height;

void main()
{
    float x = TexCoords.x * width;
    float y = TexCoords.y * height;

    float r = mod(x * sizeX, width) / width;
    float g = mod(y * sizeY, height) / height;
    float b = 1.0;

    FragColor = vec4(r, g, b, 1.0);
}
