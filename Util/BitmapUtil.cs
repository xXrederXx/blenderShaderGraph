using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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

    public static Color[,] GetPixles(this Bitmap self)
    {
        int width = self.Width;
        int height = self.Height;
        Color[,] result = new Color[width, height];

        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bmpData = self.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        int byteCount = Math.Abs(bmpData.Stride) * height;
        byte[] pixels = new byte[byteCount];
        Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);

        self.UnlockBits(bmpData);

        int stride = bmpData.Stride;
        for (int y = 0; y < height; y++)
        {
            int rowOffset = y * stride;
            for (int x = 0; x < width; x++)
            {
                int index = rowOffset + x * 4;
                byte b = pixels[index];
                byte g = pixels[index + 1];
                byte r = pixels[index + 2];
                byte a = pixels[index + 3];
                result[x, y] = Color.FromArgb(a, r, g, b);
            }
        }

        return result;
    }

        public static void SetPixles(this Bitmap self, Color[,] pixels)
    {
        int width = self.Width;
        int height = self.Height;

        if (pixels.GetLength(0) != width || pixels.GetLength(1) != height)
            throw new ArgumentException("Pixel array size does not match bitmap dimensions.");

        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bmpData = self.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

        int byteCount = Math.Abs(bmpData.Stride) * height;
        byte[] pixelBytes = new byte[byteCount];

        int stride = bmpData.Stride;
        for (int y = 0; y < height; y++)
        {
            int rowOffset = y * stride;
            for (int x = 0; x < width; x++)
            {
                Color c = pixels[x, y];
                int index = rowOffset + x * 4;
                pixelBytes[index] = c.B;
                pixelBytes[index + 1] = c.G;
                pixelBytes[index + 2] = c.R;
                pixelBytes[index + 3] = c.A;
            }
        }

        Marshal.Copy(pixelBytes, 0, bmpData.Scan0, byteCount);
        self.UnlockBits(bmpData);
    }
}