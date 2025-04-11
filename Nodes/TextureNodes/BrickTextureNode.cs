using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public static class BrickTextureNode
{
    private static Random rng = new();

    public static (Bitmap color, Bitmap fac) GenerateTexture(BrickTextureProps props)
    {
        Bitmap imgColor = new(props.imgWidth, props.imgHeight);
        Bitmap imgFactor = new(props.imgWidth, props.imgHeight);

        ApplyColorsRow(props, imgColor, imgFactor);

        return (imgColor, imgFactor);

        static void ApplyColorsRow(BrickTextureProps props, Bitmap imgColor, Bitmap imgFactor)
        {
            for (int r = 0; r < props.rows; r++)
            {
                bool isOffset = props.offsetFrequency != 0 && r % props.offsetFrequency == 0;
                bool isSquashed = props.squashFrequency != 0 && r % props.squashFrequency == 0;

                int yStart = (int)(r * props.rowHeight);
                int yEnd = (int)(yStart + props.rowHeight);
                int colStart = isOffset ? -1 : 0;
                int colEnd = isOffset ? props.cols + 1 : props.cols;

                float _brickWidth = isSquashed ? props.brickWidth * props.squash : props.brickWidth;
                ApplyColorColumns(
                    props,
                    imgColor,
                    imgFactor,
                    isOffset,
                    yStart,
                    yEnd,
                    colStart,
                    colEnd,
                    _brickWidth
                );
            }
        }
    }

    private static void ApplyColorColumns(
        BrickTextureProps props,
        Bitmap imgColor,
        Bitmap imgFactor,
        bool isOffset,
        int yStart,
        int yEnd,
        int colStart,
        int colEnd,
        float _brickWidth
    )
    {
        for (int c = colStart; c < colEnd; c++)
        {
            int xStart = (int)(c * _brickWidth);
            int xEnd = (int)(xStart + _brickWidth);

            if (isOffset)
            {
                xStart += (int)Math.Ceiling(props.offset * props.brickWidth);
                xEnd += (int)Math.Ceiling(props.offset * props.brickWidth);
            }

            Color brickColor = GetColor(props.color1, props.color2, props.bias);
            generateSquare(props, imgColor, imgFactor, yStart, yEnd, xStart, xEnd, brickColor);
        }
    }

    private static void generateSquare(
        BrickTextureProps props,
        Bitmap imgColor,
        Bitmap imgFactor,
        int yStart,
        int yEnd,
        int xStart,
        int xEnd,
        Color brickColor
    )
    {
        for (int y = yStart; y < yEnd; y++)
        {
            for (int x = xStart; x < xEnd; x++)
            {
                if (y < 0 || y >= props.imgHeight || x < 0 || x >= props.imgWidth)
                {
                    continue;
                }

                SetPixel(props, imgColor, imgFactor, yStart, yEnd, xStart, xEnd, brickColor, y, x);
            }
        }
    }

    private static void SetPixel(
        BrickTextureProps props,
        Bitmap imgColor,
        Bitmap imgFactor,
        int yStart,
        int yEnd,
        int xStart,
        int xEnd,
        Color brickColor,
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
            Color MotarC = ColorUtil.LerpColor(props.colorMotar, brickColor, blend);
            Color MotarF = ColorUtil.LerpColor(Color.White, Color.Black, blend);
            imgColor.SetPixel(x, y, MotarC);
            imgFactor.SetPixel(x, y, MotarF);
        }
        else
        {
            imgColor.SetPixel(x, y, brickColor);
            imgFactor.SetPixel(x, y, Color.Black);
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
        float blend;
        // Corner blend: combine vertical and horizontal distance
        int verticalDist = inTopMotar ? distTop : (inBottomMotar ? distBottom : 0);
        int horizontalDist = inLeftMotar ? distLeft : (inRightMotar ? distRight : 0);

        float vBlend = verticalDist * props.MotarLerpDist;
        float hBlend = horizontalDist * props.MotarLerpDist;

        // Blend using max of both directions (soft diagonal blend)
        if (inTopMotar && !inBottomMotar && inLeftMotar && !inRightMotar)
        {
            blend = Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        else if (inTopMotar && !inBottomMotar && !inLeftMotar && inRightMotar)
        {
            blend = Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        else if (!inTopMotar && inBottomMotar && inLeftMotar && !inRightMotar)
        {
            blend = Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        else if (!inTopMotar && inBottomMotar && !inLeftMotar && inRightMotar)
        {
            blend = Math.Clamp(Math.Min(vBlend, hBlend), 0, 1);
        }
        else
        {
            blend = Math.Clamp(Math.Max(vBlend, hBlend), 0, 1);
        }

        return blend;
    }

    private static Color GetColor(Color color1, Color color2, float bias)
    {
        double val = rng.NextDouble();
        return ColorUtil.LerpColor(color1, color2, (float)Math.Clamp(val + bias, 0, 1));
    }
}
