using System;
using System.Drawing;
using System.Numerics;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.VectorNodes;

public enum NormalMapFormat
{
    OpenGL, // Y up
    DirectX // Y down
}

public class BumpNode
{
    public static Bitmap GenerateBump(BumpProps props)
    {
        int width = props.HeightMap.Width;
        int height = props.HeightMap.Height;

        Bitmap normalMap = new(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        Color[,] colors = new Color[width, height];
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                float hL = ColorUtil.ValueFromColor(props.HeightMap.GetPixel(x - 1, y));
                float hR = ColorUtil.ValueFromColor(props.HeightMap.GetPixel(x + 1, y));
                float hD = ColorUtil.ValueFromColor(props.HeightMap.GetPixel(x, y - 1));
                float hU = ColorUtil.ValueFromColor(props.HeightMap.GetPixel(x, y + 1));

                float dx = (hR - hL) * props.Strength;
                float dy = (hU - hD) * props.Strength;

                if (props.Invert)
                {
                    dx = -dx;
                    dy = -dy;
                }

                Vector3 normal = new Vector3(-dx, -dy, 1.0f / props.Distance);
                normal = Vector3.Normalize(normal);

                float nx = normal.X * 0.5f + 0.5f;
                float ny = normal.Y * 0.5f + 0.5f;
                float nz = normal.Z * 0.5f + 0.5f;

                // Flip G (Y) channel for DirectX
                if (props.Format == NormalMapFormat.DirectX)
                {
                    ny = 1.0f - ny;
                }

                Color normalColor = Color.FromArgb(
                    (byte)Math.Clamp(nx * 255, 0, 255),
                    (byte)Math.Clamp(ny * 255, 0, 255),
                    (byte)Math.Clamp(nz * 255, 0, 255)
                );

                colors[x, y] = normalColor;
            }
        }
        normalMap.SetPixles(colors);
        return normalMap;
    }
}
