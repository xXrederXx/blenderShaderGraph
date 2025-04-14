using System;
using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public enum MaskTextureType
{
    Square,
}

public class MaskTexture
{
    private static Random rng = new();

    public static Bitmap Generate(
        int Width,
        int Height,
        int numDots,
        int maxSize,
        int minSize,
        MaskTextureType type = MaskTextureType.Square
    )
    {
        Bitmap res = BitmapUtil.FilledBitmap(Width, Height, Color.Black);
        List<(int x, int y, int size)> dots = [];
        for (int i = 0; i < numDots; i++)
        {
            int size = rng.Next(minSize, maxSize);

            int maxValueX = Width - size;
            int maxValueY = Height - size;
            if (maxValueX < size || maxValueY < size)
            {
                System.Console.WriteLine("Max Values are too big");
                continue;
            }
            int x = rng.Next(size, maxValueX);
            int y = rng.Next(size, maxValueY);
            dots.Add((x, y, size));
        }
        Color[,] cols = res.GetPixles();

        Func<float, float, float> func = type switch
        {
            MaskTextureType.Square => SquareFade,
            _ => SquareFade,
        };

        foreach (var dot in dots)
        {
            int startX = dot.x - dot.size;
            int endX = dot.x + dot.size;
            int startY = dot.y - dot.size;
            int endY = dot.y + dot.size;
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    // calc normalized distance
                    float xDist = MyMath.Clamp01(Math.Abs(x - dot.x) / dot.size);
                    float yDist = MyMath.Clamp01(Math.Abs(y - dot.y) / dot.size);

                    cols[x, y] = ColorUtil.ColorFromValue(func.Invoke(x, y), false);
                }
            }
        }
        res.SetPixles(cols);
        return res;
    }

    private static float SquareFade(float xDist, float yDist)
    {
        return 1 - xDist * 0.5f + 1 - yDist * 0.5f;
    }
}
