using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ColorNodes;

public static class MixColorNode
{
    public static Bitmap Generate(
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

    public static Bitmap Generate(
        Bitmap a,
        Bitmap b,
        Bitmap factor,
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
        return Mix(
            a,
            b,
            BitmapUtil.FilledBitmap(width, height, ColorUtil.ColorFromValue(factor, false))
        );
    }

    private static Bitmap Mix(Bitmap a, Bitmap b, Bitmap factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Color[,] aCols = a.GetPixles();
        Color[,] bCols = b.GetPixles();
        Color[,] facCols = factor.GetPixles();
        Bitmap res = new(width, height);
        res.ForPixelParralel(
            (x, y) =>
            {
                Color aCol = aCols[x, y];
                Color bCol = bCols[x, y];
                float fac = ColorUtil.ValueFromColor(facCols[x, y]);
                return Color.FromArgb(
                    255,
                    (byte)(aCol.R * (1 - fac) + bCol.R * fac),
                    (byte)(aCol.G * (1 - fac) + bCol.G * fac),
                    (byte)(aCol.B * (1 - fac) + bCol.B * fac)
                );
            }
        );
        return res;
    }

    private static Bitmap Hue(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        return Hue(
            a,
            b,
            BitmapUtil.FilledBitmap(width, height, ColorUtil.ColorFromValue(factor, false))
        );
    }

    private static Bitmap Hue(Bitmap a, Bitmap b, Bitmap factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Color[,] aCols = a.GetPixles();
        Color[,] bCols = b.GetPixles();
        Color[,] facCols = factor.GetPixles();
        Bitmap res = new(width, height);

        res.ForPixelParralel(
            (x, y) =>
            {
                Color aCol = aCols[x, y];
                Color bCol = bCols[x, y];
                float fac = ColorUtil.ValueFromColor(facCols[x, y]);
                ColorUtil.ColorToHSV(aCol, out double ah, out double asat, out double av);
                ColorUtil.ColorToHSVOnlyV(bCol, out double bh);
                double nh = ah * (1 - fac) + bh * fac;
                return ColorUtil.ColorFromHSV(nh, asat, av);
            }
        );

        return res;
    }

    private static Bitmap Saturation(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        return Saturation(
            a,
            b,
            BitmapUtil.FilledBitmap(width, height, ColorUtil.ColorFromValue(factor, false))
        );
    }

    private static Bitmap Saturation(Bitmap a, Bitmap b, Bitmap factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Color[,] aCols = a.GetPixles();
        Color[,] bCols = b.GetPixles();
        Color[,] facCols = factor.GetPixles();
        Bitmap res = new(width, height);

        res.ForPixelParralel(
            (x, y) =>
            {
                Color aCol = aCols[x, y];
                Color bCol = bCols[x, y];
                float fac = ColorUtil.ValueFromColor(facCols[x, y]);
                ColorUtil.ColorToHSV(aCol, out double ahue, out double asat, out double av);
                ColorUtil.ColorToHSVOnlyS(bCol, out double bsat);
                double nsat = asat * (1 - fac) + bsat * fac;
                return ColorUtil.ColorFromHSV(ahue, nsat, av);
            }
        );

        return res;
    }

    private static Bitmap Value(Bitmap a, Bitmap b, float factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        return Value(
            a,
            b,
            BitmapUtil.FilledBitmap(width, height, ColorUtil.ColorFromValue(factor, false))
        );
    }

    private static Bitmap Value(Bitmap a, Bitmap b, Bitmap factor)
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        Color[,] aCols = a.GetPixles();
        Color[,] bCols = b.GetPixles();
        Color[,] facCols = factor.GetPixles();
        Bitmap res = new(width, height);

        res.ForPixelParralel(
            (x, y) =>
            {
                Color aCol = aCols[x, y];
                Color bCol = bCols[x, y];
                float fac = ColorUtil.ValueFromColor(facCols[x, y]);
                ColorUtil.ColorToHSV(aCol, out double ahue, out double asat, out double av);
                ColorUtil.ColorToHSVOnlyV(bCol, out double bv);
                double nv = av * (1 - fac) + bv * fac;
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
