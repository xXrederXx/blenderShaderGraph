using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public record BrickTextureProps(
    int imgWidth,
    int imgHeight,
    float offset, // 0 - 1
    int offsetFrequency,
    float squash,
    int squashFrequency,
    Color color1,
    Color color2,
    Color colorMotar,
    float motarSize,
    float motarSmoothness, // 0 - 1
    float bias, // -1 - 1
    float brickWidth,
    float rowHeight
)
{
    public readonly int imgWidth = imgWidth;
    public readonly int imgHeight = imgHeight;
    public readonly float offset = MyMath.Clapm01(offset); // 0 - 1
    public readonly int offsetFrequency = offsetFrequency;
    public readonly float squash = squash;
    public readonly int squashFrequency = squashFrequency;
    public readonly Color color1 = color1;
    public readonly Color color2 = color2;
    public readonly Color colorMotar = colorMotar;
    public readonly float motarSize = motarSize;
    public readonly float motarSmoothness = MyMath.Clapm01(motarSmoothness); // 0 - 1
    public readonly float bias = Math.Clamp(bias, -1, 1); // -1 - 1
    public readonly float brickWidth = brickWidth;
    public readonly float rowHeight = rowHeight;

    public int rows = (int)(imgHeight / rowHeight) + 1;
    public int cols = (int)(imgWidth / brickWidth) + 1;
    public int halfMotarSize = (int)Math.Ceiling(motarSize / 2);
    public float MotarLerpDist =
        motarSmoothness > 0 ? (1f / ((int)Math.Ceiling(motarSize / 2) * motarSmoothness)) : 1f;
    public int offsetWidth = (int)Math.Ceiling(offset * brickWidth);
    public float squashedBrickWidth = brickWidth * squash;
}
