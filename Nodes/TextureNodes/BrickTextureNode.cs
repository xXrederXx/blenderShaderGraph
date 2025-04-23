using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class BrickTextureNode : Node<BrickTextureProps, (MyColor[,] color, float[,] fac)>
{
    private static readonly Random rng = Random.Shared;
    private static readonly int seedForTile;

    static BrickTextureNode()
    {
        seedForTile = new Random().Next();
    }

    public BrickTextureNode()
        : base() { }

    public BrickTextureNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override BrickTextureProps SafeProps(BrickTextureProps props)
    {
        // Allready built into the class
        return props;
    }

    protected override (MyColor[,] color, float[,] fac) ExecuteInternal(BrickTextureProps props)
    {
        MyColor[,] imgC = new MyColor[props.imgWidth, props.imgHeight];
        float[,] imgF = new float[props.imgWidth, props.imgHeight];

        ApplyColorsRow(props, imgC, imgF);

        return (imgC, imgF);
    }

    protected override BrickTextureProps ConvertJSONToProps(Dictionary<string, Input> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new(
            imgWidth: p.GetInt("width", 1024),
            imgHeight: p.GetInt("height", 1024),
            offset: p.GetFloat("offset", 0.5f),
            offsetFrequency: p.GetInt("offsetFrequency", 2),
            squash: p.GetFloat("squash", 1),
            squashFrequency: p.GetInt("squashFrequency", 0),
            color1: ColorTranslator.FromHtml(p.GetString("color1", "black")),
            color2: ColorTranslator.FromHtml(p.GetString("color2", "black")),
            colorMotar: ColorTranslator.FromHtml(p.GetString("colorMotar", "white")),
            motarSize: p.GetFloat("motarSize", 5),
            motarSmoothness: p.GetFloat("motarSmoothness", 0),
            bias: p.GetFloat("bias", 0),
            brickWidth: p.GetFloat("brickWidth", 30),
            rowHeight: p.GetFloat("rowHeight", 12),
            forceTilable: p.GetBool("forceTilable")
        );
    }

    protected override void AddDataToContext(
        (MyColor[,] color, float[,] fac) data,
        Dictionary<string, Input> contex
    )
    {
        contex[Id + ".color"] = new Input<MyColor>(data.color);
        contex[Id + ".fac"] = new Input<float>(data.fac);
        contex[Id] = new Input<MyColor>(data.color);
    }

    //NODE SPESIFIC
    private static void ApplyColorsRow(
        BrickTextureProps props,
        MyColor[,] imgColor,
        float[,] imgFactor
    )
    {
        Parallel.For(
            0,
            props.cols,
            (r) =>
            {
                bool isOffset = props.offsetFrequency != 0 && r % props.offsetFrequency == 0;
                bool isSquashed = props.squashFrequency != 0 && r % props.squashFrequency == 0;

                int yStart = (int)(r * props.rowHeight);
                int yEnd = (int)(yStart + props.rowHeight);
                int colStart = isOffset ? -1 : 0;
                int colEnd = isOffset ? props.rows + 1 : props.rows;

                float _brickWidth = isSquashed ? props.squashedBrickWidth : props.brickWidth;
                ApplyColorColumns(
                    props,
                    imgColor,
                    imgFactor,
                    isOffset,
                    isSquashed,
                    yStart,
                    yEnd,
                    colStart,
                    colEnd,
                    _brickWidth,
                    r
                );
            }
        );
    }

    private static void ApplyColorColumns(
        BrickTextureProps props,
        MyColor[,] imgColor,
        float[,] imgFactor,
        bool isOffset,
        bool isSquashed,
        int yStart,
        int yEnd,
        int colStart,
        int colEnd,
        float _brickWidth,
        int r
    )
    {
        for (int c = colStart; c < colEnd; c++)
        {
            int xStart = (int)(c * _brickWidth);
            int xEnd = (int)(xStart + _brickWidth);

            if (isOffset)
            {
                xStart += props.offsetWidth;
                xEnd += props.offsetWidth;
            }

            MyColor brickColor = props.forceTilable
                ? GetTileableColor(props, r, c, isSquashed)
                : GetColor(props.color1, props.color2, props.bias);
            GenerateSquare(props, imgColor, imgFactor, yStart, yEnd, xStart, xEnd, brickColor);
        }
    }

    private static void GenerateSquare(
        BrickTextureProps props,
        MyColor[,] imgColor,
        float[,] imgFactor,
        int yStart,
        int yEnd,
        int xStart,
        int xEnd,
        MyColor brickColor
    )
    {
        int yStartLoop = Math.Max(yStart, 0);
        int yEndLoop = Math.Min(yEnd, props.imgHeight);
        int xStartLoop = Math.Max(xStart, 0);
        int xEndLoop = Math.Min(xEnd, props.imgWidth);

        for (int y = yStartLoop; y < yEndLoop; y++)
        {
            for (int x = xStartLoop; x < xEndLoop; x++)
            {
                SetPixel(props, imgColor, imgFactor, yStart, yEnd, xStart, xEnd, brickColor, y, x);
            }
        }
    }

    private static void SetPixel(
        BrickTextureProps props,
        MyColor[,] imgColor,
        float[,] imgFactor,
        int yStart,
        int yEnd,
        int xStart,
        int xEnd,
        MyColor brickColor,
        int y,
        int x
    )
    {
        int distTop = y - yStart;
        int distBottom = yEnd - y - 1;
        int distLeft = x - xStart;
        int distRight = xEnd - x - 1;

        bool inTopMotar = distTop < props.halfMotarSize;
        bool inBottomMotar = distBottom < props.halfMotarSize;
        bool inLeftMotar = distLeft < props.halfMotarSize;
        bool inRightMotar = distRight < props.halfMotarSize;

        float blend = 0;
        bool isMotar = false;

        if (inTopMotar || inBottomMotar || inLeftMotar || inRightMotar)
        {
            isMotar = true;
            blend = CalculateBlend(
                props,
                distTop,
                distBottom,
                distLeft,
                distRight,
                inTopMotar,
                inBottomMotar,
                inLeftMotar,
                inRightMotar
            );
        }

        if (isMotar)
        {
            MyColor MotarC = ColorUtil.LerpColor(props.colorMotar, brickColor, blend);
            float MotarF = MyMath.Lerp(1, 0, blend);
            imgColor[x, y] = MotarC;
            imgFactor[x, y] = MotarF;
        }
        else
        {
            imgColor[x, y] = brickColor;
            imgFactor[x, y] = 0;
        }
    }

    private static float CalculateBlend(
        BrickTextureProps props,
        int distTop,
        int distBottom,
        int distLeft,
        int distRight,
        bool inTopMotar,
        bool inBottomMotar,
        bool inLeftMotar,
        bool inRightMotar
    )
    {
        // Corner blend: combine vertical and horizontal distance
        int verticalDist = inTopMotar ? distTop : (inBottomMotar ? distBottom : 0);
        int horizontalDist = inLeftMotar ? distLeft : (inRightMotar ? distRight : 0);

        float vBlend = verticalDist * props.MotarLerpDist;
        float hBlend = horizontalDist * props.MotarLerpDist;

        // Blend using max of both directions (soft diagonal blend)
        if (inTopMotar && !inBottomMotar && inLeftMotar && !inRightMotar)
        {
            return Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        if (inTopMotar && !inBottomMotar && !inLeftMotar && inRightMotar)
        {
            return Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        if (!inTopMotar && inBottomMotar && inLeftMotar && !inRightMotar)
        {
            return Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        if (!inTopMotar && inBottomMotar && !inLeftMotar && inRightMotar)
        {
            return Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        return Math.Clamp(Math.Max(vBlend, hBlend), 0, 1);
    }

    private static MyColor GetColor(MyColor color1, MyColor color2, float bias)
    {
        double val = rng.NextDouble();
        return ColorUtil.LerpColor(color1, color2, (float)Math.Clamp(val + bias, 0, 1));
    }

    private static MyColor GetTileableColor(
        BrickTextureProps props,
        int row,
        int col,
        bool isSquashed
    )
    {
        // Ensures color is repeated across tiles
        int r = row % props.cols;
        int c = (col + 1) % (isSquashed ? props.TilableCalcRowsSquashed : props.TilableCalcRows);
        int hash = HashPosition(r, c);
        double val = hash % 10000 / 10000.0;
        return ColorUtil.LerpColor(
            props.color1,
            props.color2,
            (float)Math.Clamp(val + props.bias, 0, 1)
        );
    }

    private static int HashPosition(int r, int c)
    {
        // Simple 2D hash for tileable deterministic color
        return ((r * 73856093) ^ (c * 19349663)) & seedForTile;
    }
}
