using System.Drawing;

namespace blenderShaderGraph.Util;

public static class BitmapUtil
{
    public static void ForPixel(this Bitmap self, Func<int, int, Color> func)
    {
        for (int i = 0; i < self.Width; i++)
        {
            for (int j = 0; j < self.Height; j++)
            {
                self.SetPixel(i, j, func.Invoke(i, j));
            }
        }
    }

    public static Bitmap FilledBitmap(int width, int height, Color color)
    {
        Bitmap ret = new(width, height);
        ret.ForPixel((x, y) => color);
        return ret;
    }

    public static Bitmap ScaleToSize(int width, int height, Bitmap original)
    {
        return new Bitmap(original, width, height);
    }
}
