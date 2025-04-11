using System;
using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ConverterNodes;

public static class ColorRampNode
{
    public static void ApplyColorRamp(
        Bitmap bitmap,
        ColorStop[] colorStops,
        ColorRampMode mode = ColorRampMode.Linear
    )
    {
        bitmap.ForPixel(
            (x, y) =>
            {
                Color color = bitmap.GetPixel(x, y);
                float value = ColorUtil.ValueFromColor(color);
                return GetColor(value, colorStops, mode);
            }
        );
    }

    private static Color GetColor(
        float value,
        ColorStop[] colorStops,
        ColorRampMode mode = ColorRampMode.Linear
    )
    {
        value = Math.Clamp(value, 0, 1);
        if (colorStops.Length == 0)
        {
            return Color.FromKnownColor(KnownColor.Black);
        }
        if (colorStops.Length == 1)
        {
            return colorStops[0].color;
        }

        return mode switch
        {
            ColorRampMode.Linear => GetColorLinear(value, colorStops),
            ColorRampMode.Constant => GetColorConstant(value, colorStops),
            _ => Color.FromKnownColor(KnownColor.Black),
        };
    }

    private static Color GetColorConstant(float value, ColorStop[] colorStops)
    {
        (bool ret, Color retColor) = CalcLowAndHeigh(
            value,
            colorStops,
            out ColorStop? nextHigh,
            out ColorStop? nextLow
        );
        if (ret)
        {
            return retColor;
        }

        if (nextHigh is null && nextLow is null)
        {
            throw new Exception(
                "Both nextHigh and NextLow are null. this means, that no color is found."
            );
        }

        if (nextLow is null)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return nextHigh.color;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        return nextLow.color;
    }

    private static Color GetColorLinear(float value, ColorStop[] colorStops)
    {
        (bool ret, Color retColor) = CalcLowAndHeigh(
            value,
            colorStops,
            out ColorStop? nextHigh,
            out ColorStop? nextLow
        );
        if (ret)
        {
            return retColor;
        }

        if (nextHigh is null)
        {
            nextHigh = nextLow;
        }
        if (nextLow is null)
        {
            nextLow = nextHigh;
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return ColorUtil.LerpColor(
            nextLow.color,
            nextHigh.color,
            MyMath.Map(value, nextLow.pos, nextHigh.pos, 0, 1)
        );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private static (bool ret, Color value) CalcLowAndHeigh(
        float value,
        ColorStop[] colorStops,
        out ColorStop? nextHigh,
        out ColorStop? nextLow
    )
    {
        nextHigh = null;
        nextLow = null;
        foreach (ColorStop stop in colorStops)
        {
            if (stop.pos > value && (nextHigh is null || stop.pos < nextHigh.pos))
            {
                nextHigh = stop;
            }
            if (stop.pos < value && (nextLow is null || stop.pos > nextLow.pos))
            {
                nextLow = stop;
            }
            if (stop.pos == value)
            {
                return (ret: true, value: stop.color);
            }
        }

        if (nextHigh is null && nextLow is null)
        {
            throw new Exception(
                "Both nextHigh and NextLow are null. this means, that no color is found."
            );
        }

        return (ret: false, value: default);
    }
}

public enum ColorRampMode
{
    Linear,
    Constant,
}

public record ColorStop(Color color, float pos);
