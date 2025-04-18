using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public record BrickTextureProps
{
    public readonly int imgWidth;
    public readonly int imgHeight;
    public readonly float offset; // 0 - 1
    public readonly int offsetFrequency;
    public readonly float squash;
    public readonly int squashFrequency;
    public readonly Color color1;
    public readonly Color color2;
    public readonly Color colorMotar;
    public readonly float motarSize;
    public readonly float motarSmoothness; // 0 - 1
    public readonly float bias; // -1 - 1
    public readonly float brickWidth;
    public readonly float rowHeight;

    public readonly int cols;
    public readonly int rows;
    public readonly int halfMotarSize;
    public readonly float MotarLerpDist;
    public readonly int offsetWidth;
    public readonly float squashedBrickWidth;
    public bool forceTilable;
    public readonly int TilableCalcRows;
    public readonly int TilableCalcRowsSquashed;

    public BrickTextureProps(
        int imgWidth = 1024,
        int imgHeight = 1024,
        float offset = 0.5f, // 0 - 1
        int offsetFrequency = 2,
        float squash = 1,
        int squashFrequency = 0,
        Color? color1 = null,
        Color? color2 = null,
        Color? colorMotar = null,
        float motarSize = 5,
        float motarSmoothness = 0, // 0 - 1
        float bias = 0, // -1 - 1
        float brickWidth = 30,
        float rowHeight = 12,
        bool forceTilable = true
    )
    {
        this.imgWidth = imgWidth;
        this.imgHeight = imgHeight;
        this.offset = MyMath.Clamp01(offset);
        this.offsetFrequency = offsetFrequency;
        this.squash = squash;
        this.squashFrequency = squashFrequency;
        this.color1 = color1 ?? Color.White;
        this.color2 = color2 ?? Color.Beige;
        this.colorMotar = colorMotar ?? Color.DarkGoldenrod;
        this.motarSize = motarSize;
        this.motarSmoothness = MyMath.Clamp01(motarSmoothness);
        this.bias = Math.Clamp(bias, -1, 1);
        this.brickWidth = brickWidth;
        this.rowHeight = rowHeight;
        this.forceTilable = forceTilable;
        this.cols = (int)(imgHeight / rowHeight) + 1;
        this.rows = (int)(imgWidth / brickWidth) + 1;
        this.halfMotarSize = (int)Math.Ceiling(motarSize / 2);
        this.MotarLerpDist = this.motarSmoothness > 0 
            ? motarSmoothness / halfMotarSize 
            : 0f;
        offsetWidth = (int)Math.Ceiling(offset * brickWidth);
        squashedBrickWidth = brickWidth * squash;

        TilableCalcRows = rows - 1;
        TilableCalcRowsSquashed = (int)(imgWidth / squashedBrickWidth);
    }
}
