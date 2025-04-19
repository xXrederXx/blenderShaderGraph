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
    private const float halfPI = (float)(Math.PI / 2);

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
        var dots = GetDotsList(Width, Height, numDots, maxSize, minSize);
        var cols = res.GetPixles();

        foreach (var (xCenter, yCenter, size) in dots)
        {
            int startX = Math.Max(0, xCenter - size);
            int endX = Math.Min(Width, xCenter + size);
            int startY = Math.Max(0, yCenter - size);
            int endY = Math.Min(Height, yCenter + size);

            for (int x = startX; x < endX; x++)
            {
                float xNorm = Math.Abs((x - xCenter) / (float)size);

                for (int y = startY; y < endY; y++)
                {
                    float yNorm = Math.Abs((y - yCenter) / (float)size);
                    float value = GetMaskValue(xNorm, yNorm, type, betterDistCalc);
                    value = MyMath.Clamp01(value);

                    cols[x, y] = ColorUtil.LerpColor(cols[x, y], Color.White, value);
                }
            }
        }

        res.SetPixles(cols);
        return res;
    }

    private static float GetMaskValue(
        float xNorm,
        float yNorm,
        MaskTextureType type,
        bool betterDistCalc
    )
    {
        if (betterDistCalc)
        {
            float dist = MyMath.Clamp01(MathF.Sqrt(xNorm * xNorm + yNorm * yNorm));
            return MaskShape(dist, type);
        }
        else
        {
            float vx = MaskShape(xNorm, type);
            float vy = MaskShape(yNorm, type);
            return 0.5f * (vx + vy);
        }
    }

    private static float MaskShape(float norm, MaskTextureType type)
    {
        return type switch
        {
            MaskTextureType.SquareFade => SquareFade(norm),
            MaskTextureType.EaseInSine => EaseInSine(norm),
            MaskTextureType.Square => Square(norm),
            MaskTextureType.Cube => Cube(norm),
            _ => SquareFade(norm),
        };
    }

    private static List<(int x, int y, int size)> GetDotsList(
        int Width,
        int Height,
        int numDots,
        int maxSize,
        int minSize
    )
    {
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
            if (calcedDots >= numDots)
            {
                break;
            }
        }
        return dots;
    }

    private static float SquareFade(float dist) => 1 - dist;

    private static float EaseInSine(float dist) => MyMath.CosFast(dist * halfPI);

    private static float Square(float dist) => (1 - dist) * (1 - dist);

    private static float Cube(float dist) => (1 - dist) * (1 - dist) * (1 - dist);
}
