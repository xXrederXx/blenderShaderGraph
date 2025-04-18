using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ConverterNodes;

public static class ColorRampNode
{
    public static Bitmap Apply(
        Bitmap bitmap,
        ColorStop[] colorStops,
        ColorRampMode mode = ColorRampMode.Linear
    )
    {
        Bitmap res = new Bitmap(bitmap.Width, bitmap.Height);
        Color[,] colors = bitmap.GetPixles();
        ColorStop[] sortedStops = colorStops.OrderBy(cs => cs.pos).ToArray();
        res.ForPixelParralel(
            (x, y) =>
            {
                Color color = colors[x, y];
                float value = ColorUtil.ValueFromColor(color);
                return GetColor(value, sortedStops, mode);
            }
        );
        return res;
    }

    private static Color GetColor(
        float value,
        ColorStop[] sortedStops,
        ColorRampMode mode = ColorRampMode.Linear
    )
    {
        value = Math.Clamp(value, 0, 1);
        if (sortedStops.Length == 0)
        {
            return Color.FromKnownColor(KnownColor.Black);
        }
        if (sortedStops.Length == 1)
        {
            return sortedStops[0].color;
        }

        return mode switch
        {
            ColorRampMode.Linear => GetColorLinear(value, sortedStops),
            ColorRampMode.Constant => GetColorConstant(value, sortedStops),
            _ => Color.FromKnownColor(KnownColor.Black),
        };
    }

    private static Color GetColorConstant(float value, ColorStop[] sortedStops)
    {
        var (low, high, exactMatch, exactColor) = FindSurroundingStops(value, sortedStops);
        if (exactMatch && exactColor.HasValue)
        {
            return exactColor.Value;
        }

        if (high is null && low is null)
        {
            throw new Exception(
                "Both nextHigh and NextLow are null. this means, that no color is found."
            );
        }

        if (low is null)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return high.color;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        return low.color;
    }

    private static Color GetColorLinear(float value, ColorStop[] sortedStops)
    {
        var (low, high, exactMatch, exactColor) = FindSurroundingStops(value, sortedStops);
        if (exactMatch && exactColor.HasValue)
        {
            return exactColor.Value;
        }

        if (high is null)
        {
            high = low;
        }
        if (low is null)
        {
            low = high;
        }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return ColorUtil.LerpColor(
            low.color,
            high.color,
            MyMath.Map(value, low.pos, high.pos, 0, 1)
        );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private static (
        ColorStop? low,
        ColorStop? high,
        bool exactMatch,
        Color? exactColor
    ) FindSurroundingStops(float value, ColorStop[] sortedStops)
    {
        int left = 0;
        int right = sortedStops.Length - 1;

        // Handle edge cases: value is outside the bounds
        if (value <= sortedStops[0].pos)
            return (null, sortedStops[0], false, null);
        if (value >= sortedStops[^1].pos)
            return (sortedStops[^1], null, false, null);

        // Binary search for the closest lower and upper ColorStops
        while (left <= right)
        {
            int mid = (left + right) / 2;
            float midPos = sortedStops[mid].pos;

            // Exact match found
            if (value == midPos)
            {
                return (null, null, true, sortedStops[mid].color);
            }
            // Search left half
            else if (value < midPos)
            {
                right = mid - 1;
            }
            // Search right half
            else
            {
                left = mid + 1;
            }
        }

        // At this point:
        // - right is the largest index where pos <= value
        // - left is the smallest index where pos > value
        ColorStop low = sortedStops[right];
        ColorStop high = sortedStops[left];
        return (low, high, false, null);
    }
}

public enum ColorRampMode
{
    Linear,
    Constant,
}

public record ColorStop(Color color, float pos);
