using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

public static class Converter
{
    public static MyColor[,] ConvertToColor(float[,] floats)
    {
        int width = floats.GetLength(0);
        int height = floats.GetLength(1);
        MyColor[,] myColors = new MyColor[width, height];

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    byte v = MyMath.ClampByte(floats[x, y] * 255);
                    myColors[x, y] = new MyColor(v, v, v);
                }
            }
        );
        return myColors;
    }

    public static float[,] ConvertToFloat(MyColor[,] colors)
    {
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        float[,] myColors = new float[width, height];

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    myColors[x, y] = colors[x, y].GetGrayscale();
                }
            }
        );
        return myColors;
    }
}
