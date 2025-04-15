using System;
using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public enum MaskTextureType
{
    SquareFade,
    EaseInSine,
    Square,
    Cube,
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
        MaskTextureType type = MaskTextureType.Square,
        bool betterDistCalc = false
    )
    {
        Bitmap res = BitmapUtil.FilledBitmap(Width, Height, Color.Black);
        List<(int x, int y, int size)> dots = [];
        int calcedDots = 0;
        int trysMax = numDots * 2;
        for (int i = 0; i < trysMax; i++)
        {
            int size = rng.Next(minSize, maxSize);

            int maxValueX = Width - size;
            int maxValueY = Height - size;
            if (maxValueX < size || maxValueY < size)
            {
                continue;
            }
            int x = rng.Next(size, maxValueX);
            int y = rng.Next(size, maxValueY);
            dots.Add((x, y, size));
            calcedDots++;
            if(calcedDots >= numDots)
            {
                break;
            }
        }
        Color[,] cols = res.GetPixles();

        Func<float, float> func = type switch
        {
            MaskTextureType.SquareFade => SquareFade,
            MaskTextureType.EaseInSine => EaseInSine,
            MaskTextureType.Square => Square,
            MaskTextureType.Cube => Cube,
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
                    float xDist = Math.Abs(x - dot.x) / (float)dot.size;
                    float yDist = Math.Abs(y - dot.y) / (float)dot.size;
                    float value;
                    if (betterDistCalc)
                    {
                        float dist = MyMath.Clamp01(
                            (float)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2))
                        );
                        value = func.Invoke(dist);
                    }
                    else
                    {
                        value = func.Invoke(xDist) * 0.5f + func.Invoke(yDist) * 0.5f;
                    }
                    cols[x, y] = ColorUtil.LerpColor(
                        cols[x, y],
                        Color.White,
                        MyMath.Clamp01(value)
                    );
                }
            }
        }
        res.SetPixles(cols);
        return res;
    }

    private static float SquareFade(float dist) => 1 - dist;

    private static float EaseInSine(float dist) => (float)Math.Cos(dist * Math.PI / 2);

    private static float Square(float dist) => (float)Math.Pow(1 - dist, 2);

    private static float Cube(float dist) => (float)Math.Pow(1 - dist, 3);
}
