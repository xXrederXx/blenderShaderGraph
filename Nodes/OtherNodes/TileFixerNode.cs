using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public static class TileFixerNode
{
    public static void Apply(Bitmap bitmap, int blendBandSize = 32)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        Bitmap clone = (Bitmap)bitmap.Clone();

        // Blend left ↔ right edges
        for (int x = 0; x < blendBandSize; x++)
        {
            float t = SmoothFalloff(x / (float)blendBandSize);

            for (int y = 0; y < height; y++)
            {
                // Left side
                Color origLeft = clone.GetPixel(x, y);
                Color mirrorRight = clone.GetPixel(width - blendBandSize + x, y);
                Color blendL = ColorUtil.LerpColor(mirrorRight, origLeft, t);
                bitmap.SetPixel(x, y, blendL);

                // Right side
                Color origRight = clone.GetPixel(width - 1 - x, y);
                Color mirrorLeft = clone.GetPixel(blendBandSize - 1 - x, y);
                Color blendR = ColorUtil.LerpColor(mirrorLeft, origRight, t);
                bitmap.SetPixel(width - 1 - x, y, blendR);
            }
        }

        // Blend top ↔ bottom edges
        for (int y = 0; y < blendBandSize; y++)
        {
            float t = SmoothFalloff(y / (float)blendBandSize);

            for (int x = 0; x < width; x++)
            {
                // Top side
                Color origTop = clone.GetPixel(x, y);
                Color mirrorBottom = clone.GetPixel(x, height - blendBandSize + y);
                Color blendT = ColorUtil.LerpColor(mirrorBottom, origTop, t);
                bitmap.SetPixel(x, y, blendT);

                // Bottom side
                Color origBottom = clone.GetPixel(x, height - 1 - y);
                Color mirrorTop = clone.GetPixel(x, blendBandSize - 1 - y);
                Color blendB = ColorUtil.LerpColor(mirrorTop, origBottom, t);
                bitmap.SetPixel(x, height - 1 - y, blendB);
            }
        }
    }

    // Falloff for smoother blending — cosine gives a soft rolloff
    private static float SmoothFalloff(float x)
    {
        return (1 - MathF.Cos(x * MathF.PI)) * 0.5f;
    }
}
