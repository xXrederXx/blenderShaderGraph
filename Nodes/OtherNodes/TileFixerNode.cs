using System;
using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public static class TileFixerNode
{
    public static void Apply(Bitmap bitmap, int blendBandSize = 32)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        var originalPixels = bitmap.GetPixles();
        var remappedPixels = new Color[width, height];

        int halfWidth = width / 2;
        int halfHeight = height / 2;

        // Rearrange quadrants
        SwapQuadrant(originalPixels, remappedPixels, 0, 0, halfWidth, halfHeight, halfWidth, halfHeight);             // TL -> BR
        SwapQuadrant(originalPixels, remappedPixels, halfWidth, 0, halfWidth, halfHeight, -halfWidth, halfHeight);    // TR -> BL
        SwapQuadrant(originalPixels, remappedPixels, 0, halfHeight, halfWidth, halfHeight, halfWidth, -halfHeight);   // BL -> TR
        SwapQuadrant(originalPixels, remappedPixels, halfWidth, halfHeight, halfWidth, halfHeight, -halfWidth, -halfHeight); // BR -> TL

        // Blend vertical seams
        for (int x = halfWidth - blendBandSize; x < halfWidth + blendBandSize; x++)
        {
            float blendPos = (float)(x - (halfWidth - blendBandSize)) / (2 * blendBandSize);
            float weightA = 1f - SmoothFalloff(blendPos);
            float weightB = SmoothFalloff(blendPos);

            for (int y = 0; y < height; y++)
            {
                Color left = remappedPixels[x - blendBandSize, y];
                Color right = remappedPixels[x + blendBandSize, y];
                remappedPixels[x, y] = ColorUtil.LerpColor(left, right, weightB);
            }
        }

        // Blend horizontal seams
        for (int y = halfHeight - blendBandSize; y < halfHeight + blendBandSize; y++)
        {
            float blendPos = (float)(y - (halfHeight - blendBandSize)) / (2 * blendBandSize);
            float weightA = 1f - SmoothFalloff(blendPos);
            float weightB = SmoothFalloff(blendPos);

            for (int x = 0; x < width; x++)
            {
                Color top = remappedPixels[x, y - blendBandSize];
                Color bottom = remappedPixels[x, y + blendBandSize];
                remappedPixels[x, y] = ColorUtil.LerpColor(top, bottom, weightB);
            }
        }

        bitmap.SetPixles(remappedPixels);
    }

    private static void SwapQuadrant(Color[,] src, Color[,] dst, int startX, int startY, int width, int height, int offsetX, int offsetY)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                dst[startX + x + offsetX, startY + y + offsetY] = src[startX + x, startY + y];
            }
        }
    }

    private static float SmoothFalloff(float x)
    {
        return (1 - MathF.Cos(x * MathF.PI)) * 0.5f;
    }
}
