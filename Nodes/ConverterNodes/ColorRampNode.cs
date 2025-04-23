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

public record ColorStop(MyColor color, float pos);

public record ColorRampProps
{
    public Input<float>? Image { get; set; }
    public ColorStop[]? ColorStops { get; set; }
    public ColorRampMode Mode { get; set; } = ColorRampMode.Linear;
}

public class ColorRampNode : Node<ColorRampProps, Input<MyColor>>
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
            props.ColorStops[0] = new ColorStop(new MyColor(0, 0, 0), 0);
        }

        return props;
    }

    protected override Input<MyColor> ExecuteInternal(ColorRampProps props)
    {
        if (props.Image is null)
        {
            return new Input<MyColor>(new MyColor[0, 0]);
        }
        if (props.ColorStops is null)
        {
            throw new ArgumentException(
                "props.Colorstops is null. Check SafeProps function and make sure you run the function before caling this function."
            );
        }

        ColorStop[] sortedStops = props.ColorStops.OrderBy(cs => cs.pos).ToArray();
        if (!props.Image.useArray)
        {
            GetColor(props.Image.Value, sortedStops, props.Mode);
        }
        if (props.Image.Array is null)
        {
            throw new ArgumentException(
                "props.Image.Array is null. Even if it is signalied it uses the array."
            );
        }

        int width = props.Image.Array.GetLength(0);
        int height = props.Image.Array.GetLength(1);

        float[,] oldColors = props.Image.Array;
        MyColor[,] newColors = new MyColor[width, height];

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    float value = oldColors[x, y];
                    newColors[x, y] = GetColor(value, sortedStops, props.Mode);
                }
            }
        );

        return new(newColors);
    }

    protected override ColorRampProps ConvertJSONToProps(Dictionary<string, Input> contex)
    {
        JsonElement p = element.GetProperty("params");
        List<ColorStop> stops = [];
        foreach (JsonElement x in p.GetProperty("colorStops").EnumerateArray())
        {
            MyColor col = ColorTranslator.FromHtml(x.GetString("color", "black"));
            float pos = x.GetFloat("position", 0);
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
            Image = p.GetInputFloat(Id, contex, "image"),
            ColorStops = stops.ToArray(),
            Mode = mode,
        };
    }

    protected override void AddDataToContext(Input<MyColor> data, Dictionary<string, Input> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
    private static MyColor GetColor(
        float value,
        ColorStop[] sortedStops,
        ColorRampMode mode = ColorRampMode.Linear
    )
    {
        value = Math.Clamp(value, 0, 1);
        if (sortedStops.Length == 0)
        {
            return new MyColor(0, 0, 0);
        }
        if (sortedStops.Length == 1)
        {
            return sortedStops[0].color;
        }

        return mode switch
        {
            ColorRampMode.Linear => GetColorLinear(value, sortedStops),
            ColorRampMode.Constant => GetColorConstant(value, sortedStops),
            _ => new MyColor(0, 0, 0),
        };
    }

    private static MyColor GetColorConstant(float value, ColorStop[] sortedStops)
    {
        (ColorStop? low, ColorStop? high, bool exactMatch, MyColor? exactColor) =
            FindSurroundingStops(value, sortedStops);
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

    private static MyColor GetColorLinear(float value, ColorStop[] sortedStops)
    {
        (ColorStop? low, ColorStop? high, bool exactMatch, MyColor? exactColor) =
            FindSurroundingStops(value, sortedStops);
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
        MyColor? exactColor
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
