using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public static class BrickTextureNode
{
    private static readonly Random rng = Random.Shared;

    public static (Bitmap color, Bitmap fac) GenerateTexture(BrickTextureProps props)
    {
        Bitmap imgColor = new(props.imgWidth, props.imgHeight);
        Bitmap imgFactor = new(props.imgWidth, props.imgHeight);

        Color[,] imgC = new Color[props.imgWidth, props.imgHeight];
        Color[,] imgF = new Color[props.imgWidth, props.imgHeight];

        ApplyColorsRow(props, imgC, imgF);

        imgColor.SetPixles(imgC);
        imgFactor.SetPixles(imgF);
        return (imgColor, imgFactor);
    }

    private static void ApplyColorsRow(
        BrickTextureProps props,
        Color[,] imgColor,
        Color[,] imgFactor
    )
    {
        Parallel.For(
            0,
            props.rows,
            (r) =>
            {
                bool isOffset = props.offsetFrequency != 0 && r % props.offsetFrequency == 0;
                bool isSquashed = props.squashFrequency != 0 && r % props.squashFrequency == 0;

                int yStart = (int)(r * props.rowHeight);
                int yEnd = (int)(yStart + props.rowHeight);
                int colStart = isOffset ? -1 : 0;
                int colEnd = isOffset ? props.cols + 1 : props.cols;

                float _brickWidth = isSquashed ? props.squashedBrickWidth : props.brickWidth;
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
        );
    }

    private static void ApplyColorColumns(
        BrickTextureProps props,
        Color[,] imgColor,
        Color[,] imgFactor,
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
                xStart += props.offsetWidth;
                xEnd += props.offsetWidth;
            }

            Color brickColor = GetColor(props.color1, props.color2, props.bias);
            GenerateSquare(props, imgColor, imgFactor, yStart, yEnd, xStart, xEnd, brickColor);
        }
    }

    private static void GenerateSquare(
        BrickTextureProps props,
        Color[,] imgColor,
        Color[,] imgFactor,
        int yStart,
        int yEnd,
        int xStart,
        int xEnd,
        Color brickColor
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
        Color[,] imgColor,
        Color[,] imgFactor,
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
            imgColor[x, y] = MotarC;
            imgFactor[x, y] = MotarF;
        }
        else
        {
            imgColor[x, y] = brickColor;
            imgFactor[x, y] = Color.Black;
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

    private static Color GetColor(Color color1, Color color2, float bias)
    {
        double val = rng.NextDouble();
        return ColorUtil.LerpColor(color1, color2, (float)Math.Clamp(val + bias, 0, 1));
    }
}
