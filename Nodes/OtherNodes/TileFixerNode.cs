using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public static class TileFixerNode
{
    public static Bitmap Apply(Bitmap bitmap, int blendBandSize = 32)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        Bitmap res = new (width, height);
        Color[,] oldColors = bitmap.GetPixles();
        Color[,] newColors = new Color[width, height];

        // Blend left ↔ right edges
        for (int x = 0; x < blendBandSize; x++)
        {
            float t = SmoothFalloff(x / (float)blendBandSize);

            for (int y = 0; y < height; y++)
            {
                // Left side
                Color origLeft = oldColors[x, y];
                Color mirrorRight = oldColors[width - blendBandSize + x, y];
                Color blendL = ColorUtil.LerpColor(mirrorRight, origLeft, t);
                newColors[x, y] = blendL;

                // Right side
                Color origRight = oldColors[width - 1 - x, y];
                Color mirrorLeft = oldColors[blendBandSize - 1 - x, y];
                Color blendR = ColorUtil.LerpColor(mirrorLeft, origRight, t);
                newColors[width - 1 - x, y] = blendR;
            }
        }

        // Blend top ↔ bottom edges
        for (int y = 0; y < blendBandSize; y++)
        {
            float t = SmoothFalloff(y / (float)blendBandSize);

            for (int x = 0; x < width; x++)
            {
                // Top side
                Color origTop = oldColors[x, y];
                Color mirrorBottom = oldColors[x, height - blendBandSize + y];
                Color blendT = ColorUtil.LerpColor(mirrorBottom, origTop, t);
                newColors[x, y] = blendT;

                // Bottom side
                Color origBottom = oldColors[x, height - 1 - y];
                Color mirrorTop = oldColors[x, blendBandSize - 1 - y];
                Color blendB = ColorUtil.LerpColor(mirrorTop, origBottom, t);
                newColors[x, height - 1 - y] = blendB;
            }
        }
        res.SetPixles(newColors);
        return res;
    }

    // Falloff for smoother blending — cosine gives a soft rolloff
    private static float SmoothFalloff(float x)
    {
        return (1 - MathF.Cos(x * MathF.PI)) * 0.5f;
    }
}
