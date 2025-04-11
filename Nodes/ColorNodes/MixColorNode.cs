using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ColorNodes;

public static class MixColorNode
{
    public static Bitmap Mix(
        Bitmap a,
        Bitmap b,
        float factor = 0.5f,
        MixColorMode mode = MixColorMode.Mix
    )
    {
        return mode switch
        {
            MixColorMode.Mix => Mix(a, b, factor),
            MixColorMode.Hue => Hue(a, b, factor),
            MixColorMode.Saturation => Saturation(a, b, factor),
            MixColorMode.Value => Value(a, b, factor),
            _ => a,
        };
    }

    private static Bitmap Mix(Bitmap a, Bitmap b, float factor = 0.5f)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Bitmap res = new(width, height);
        res.ForPixel(
            (x, y) =>
            {
                Color aCol = a.GetPixel(x, y);
                Color bCol = b.GetPixel(x, y);
                return Color.FromArgb(
                    255,
                    (byte)(aCol.R * (1 - factor) + bCol.R * factor),
                    (byte)(aCol.G * (1 - factor) + bCol.G * factor),
                    (byte)(aCol.B * (1 - factor) + bCol.B * factor)
                );
            }
        );
        return res;
    }

    private static Bitmap Hue(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Bitmap res = new(width, height);

        res.ForPixel(
            (x, y) =>
            {
                Color aCol = a.GetPixel(x, y);
                Color bCol = b.GetPixel(x, y);
                ColorUtil.ColorToHSV(aCol, out double ah, out double asat, out double av);
                ColorUtil.ColorToHSV(bCol, out double bh, out double _, out double _);
                double nh = ah * (1 - factor) + bh * factor;
                return ColorUtil.ColorFromHSV(nh, asat, av);
            }
        );

        return res;
    }

    private static Bitmap Saturation(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Bitmap res = new(width, height);

        res.ForPixel(
            (x, y) =>
            {
                Color aCol = a.GetPixel(x, y);
                Color bCol = b.GetPixel(x, y);
                ColorUtil.ColorToHSV(aCol, out double ahue, out double asat, out double av);
                ColorUtil.ColorToHSV(bCol, out double _, out double bsat, out double _);
                double nsat = asat * (1 - factor) + bsat * factor;
                return ColorUtil.ColorFromHSV(ahue, nsat, av);
            }
        );

        return res;
    }

    private static Bitmap Value(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Bitmap res = new(width, height);

        res.ForPixel(
            (x, y) =>
            {
                Color aCol = a.GetPixel(x, y);
                Color bCol = b.GetPixel(x, y);
                ColorUtil.ColorToHSV(aCol, out double ahue, out double asat, out double av);
                ColorUtil.ColorToHSV(bCol, out double _, out double _, out double bv);
                double nv = av * (1 - factor) + bv * factor;
                return ColorUtil.ColorFromHSV(ahue, asat, nv);
            }
        );

        return res;
    }
}

public enum MixColorMode
{
    Mix,
    Hue,
    Saturation,
    Value,
}
