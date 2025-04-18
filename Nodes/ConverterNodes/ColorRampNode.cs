using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ConverterNodes;

public enum ColorRampMode
{
    Linear,
    Constant,
}

public record ColorStop(Color color, float pos);

public record ColorRampProps
{
    public MyColor[,]? Image { get; set; }
    public ColorStop[]? ColorStops { get; set; }
    public ColorRampMode Mode { get; set; } = ColorRampMode.Linear;
}

public class ColorRampNode : Node<ColorRampProps, MyColor[,]>
{
    public ColorRampNode()
        : base() { }

    public ColorRampNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override ColorRampProps SafeProps(ColorRampProps props)
    {
        if (props.Image is null)
            System.Console.WriteLine("Birmap is null");
        if (props.ColorStops is null || props.ColorStops.Length == 0)
        {
            System.Console.WriteLine("Colorstops is null or empty");
            props.ColorStops = new ColorStop[1];
            props.ColorStops[0] = new ColorStop(Color.Black, 0);
        }

        return props;
    }

    protected override MyColor[,] ExecuteInternal(ColorRampProps props)
    {
        if (props.Image is null)
        {
            return new MyColor[0, 0];
        }
        if (props.ColorStops is null)
        {
            throw new ArgumentException(
                "props.Colorstops is null. Check SafeProps function and make sure you run the function before caling this function."
            );
        }

        int width = props.Image.GetLength(0);
        int height = props.Image.GetLength(1);

        MyColor[,] oldColors = props.Image;
        MyColor[,] newColors = new MyColor[width, height];

        ColorStop[] sortedStops = props.ColorStops.OrderBy(cs => cs.pos).ToArray();

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = oldColors[x, y];
                    float value = ColorUtil.ValueFromColor(color);
                    newColors[x, y] = GetColor(value, sortedStops, props.Mode);
                }
            }
        );

        return newColors;
    }

    protected override ColorRampProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");
        Input<MyColor> bmp = p.GetMyColor2D(Id, contex, "image");
        List<ColorStop> stops = [];
        foreach (JsonElement x in p.GetProperty("colorStops").EnumerateArray())
        {
            var col = ColorTranslator.FromHtml(x.GetString("color", "black"));
            var pos = x.GetFloat("position", 0);
            stops.Add(new(col, pos));
        }
        string modeStr = p.GetString("mode", "linear");
        ColorRampMode mode = modeStr switch
        {
            "linear" => ColorRampMode.Linear,
            "constant" => ColorRampMode.Constant,
            _ => ColorRampMode.Linear,
        };
        return new ColorRampProps()
        {
            Image = bmp.Array,
            ColorStops = stops.ToArray(),
            Mode = mode,
        };
    }

    protected override void AddDataToContext(MyColor[,] data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
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
        // only one of both can be null
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
