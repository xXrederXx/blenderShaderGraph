using System.Drawing;

namespace blenderShaderGraph.Util;

public static class ColorUtil
{
    public static Color ColorFromValue(float value, bool includeNegativ)
    {
        byte v = includeNegativ
            ? (byte)((Math.Clamp(value, -1f, 1f) + 1f) / 2f * 255)
            : (byte)(Math.Clamp(value, 0, 1f) * 255);
        return Color.FromArgb(255, v, v, v);
    }

    public static float ValueFromColor(Color color)
    {
        return (color.R + (float)color.G + color.B) / 3.0f / 255.0f;
    }

    public static Color LerpColor(Color aCol, Color bCol, float t)
    {
        return Color.FromArgb(
            (int)MyMath.Lerp(aCol.R, bCol.R, t),
            (int)MyMath.Lerp(aCol.G, bCol.G, t),
            (int)MyMath.Lerp(aCol.B, bCol.B, t)
        );
    }

    public static void ColorToHSV(
        Color color,
        out double hue,
        out double saturation,
        out double value
    )
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        hue = color.GetHue();
        saturation = (max == 0) ? 0 : 1d - (1d * min / max);
        value = max / 255d;
    }

    public static Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value *= 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
        {
            return Color.FromArgb(255, v, t, p);
        }
        else if (hi == 1)
        {
            return Color.FromArgb(255, q, v, p);
        }
        else if (hi == 2)
        {
            return Color.FromArgb(255, p, v, t);
        }
        else if (hi == 3)
        {
            return Color.FromArgb(255, p, q, v);
        }
        else if (hi == 4)
        {
            return Color.FromArgb(255, t, p, v);
        }
        else
        {
            return Color.FromArgb(255, v, p, q);
        }
    }
}
