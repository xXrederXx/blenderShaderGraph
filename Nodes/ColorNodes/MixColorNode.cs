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
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        var facBitmap = BitmapUtil.FilledBitmap(
            width,
            height,
            ColorUtil.ColorFromValue(factor, false)
        );
        return GenerateInternal(a, b, facBitmap, mode, width, height);
    }

    public static Bitmap Generate(
        Bitmap a,
        Bitmap b,
        Bitmap factor,
        MixColorMode mode = MixColorMode.Mix
    )
    {
        int width = Math.Min(a.Width, b.Width);
        int height = Math.Min(a.Height, b.Height);
        return GenerateInternal(a, b, factor, mode, width, height);
    }

    private static Bitmap GenerateInternal(
        Bitmap a,
        Bitmap b,
        Bitmap factor,
        MixColorMode mode,
        int width,
        int height
    )
    {
        Color[,] aCols = a.GetPixles();
        Color[,] bCols = b.GetPixles();
        Color[,] facCols = factor.GetPixles();
        Bitmap res = new(width, height);

        Func<Color, Color, float, Color> blendFunc = mode switch
        {
            MixColorMode.Mix => MixBlend,
            MixColorMode.Hue => HueBlend,
            MixColorMode.Saturation => SaturationBlend,
            MixColorMode.Value => ValueBlend,
            MixColorMode.Darken => DarkenBlend,
            MixColorMode.LinearLight => LinearLightBlend,
            MixColorMode.Lighten => LightenBlend,
            _ => (ac, bc, f) => ac,
        };

        res.ForPixelParralel(
            (x, y) =>
            {
                float fac = ColorUtil.ValueFromColor(facCols[x, y]);
                return blendFunc(aCols[x, y], bCols[x, y], fac);
            }
        );

        return res;
    }

    private static Color MixBlend(Color a, Color b, float fac)
    {
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + b.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + b.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + b.B * fac)
        );
    }

    private static Color HueBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyV(b, out double bh);
        double nh = ah * (1 - fac) + bh * fac;
        return ColorUtil.ColorFromHSV(nh, asat, av);
    }

    private static Color SaturationBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyS(b, out double bsat);
        double ns = asat * (1 - fac) + bsat * fac;
        return ColorUtil.ColorFromHSV(ah, ns, av);
    }

    private static Color ValueBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyV(b, out double bv);
        double nv = av * (1 - fac) + bv * fac;
        return ColorUtil.ColorFromHSV(ah, asat, nv);
    }

    private static Color DarkenBlend(Color a, Color b, float fac)
    {
        var c = Color.FromArgb(255, Math.Min(a.R, b.R), Math.Min(a.G, b.G), Math.Min(a.B, b.B));
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static Color LightenBlend(Color a, Color b, float fac)
    {
        var c = a.GetBrightness() > b.GetBrightness() ? a : b;
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static Color LinearLightBlend(Color a, Color b, float fac)
    {
        byte Blend(byte ac, byte bc)
        {
            float af = ac / 255f,
                bf = bc / 255f;
            float result = MyMath.Clamp01(af + 2f * bf - 1f);
            return MyMath.ClampByte((af * (1f - fac) + result * fac) * 255f);
        }

        return Color.FromArgb(255, Blend(a.R, b.R), Blend(a.G, b.G), Blend(a.B, b.B));
    }
}

public enum MixColorMode
{
    Mix,
    Hue,
    Saturation,
    Value,
    Darken,
    LinearLight,
    Lighten,
}
