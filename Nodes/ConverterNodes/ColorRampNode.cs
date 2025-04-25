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

public record ColorStop(MyColor Color, float Position);

public class ColorRampProps
{
    public ColorRampProps() { }

    public ColorRampProps(Input<float>? Image, ColorStop[]? ColorStops, ColorRampMode Mode)
    {
        this.Image = Image;
        this.ColorStops = ColorStops;
        this.Mode = Mode;
    }

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
            return InputDefaults.colorBlackInput;
        }
        if (props.ColorStops is null)
        {
            throw new ArgumentException(
                "props.Colorstops is null. Check SafeProps function and make sure you run the function before caling this function."
            );
        }

        ColorStop[] sortedStops = props.ColorStops.OrderBy(cs => cs.Position).ToArray();

        if (sortedStops.Length == 0)
        {
            return InputDefaults.colorBlackInput;
        }
        if (sortedStops.Length == 1)
        {
            return new Input<MyColor>(sortedStops[0].Color);
        }

        Func<float, ColorStop[], MyColor> func = props.Mode switch
        {
            ColorRampMode.Linear => GetColorLinear,
            ColorRampMode.Constant => GetColorConstant,
            _ => (f, cs) => new MyColor(MyMath.ClampByte(f * 255)),
        };

        if (!props.Image.useArray)
        {
            return new Input<MyColor>(func(props.Image.Value, sortedStops));
        }
        if (props.Image.Array is null)
        {
            throw new ArgumentException(
                "props.Image.Array is null. Even if it is signalied it uses the array."
            );
        }

        float[,] oldColors = props.Image.Array;
        int width = oldColors.GetLength(0);
        int height = oldColors.GetLength(1);
        MyColor[,] newColors = new MyColor[width, height];

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    newColors[x, y] = func(oldColors[x, y], sortedStops);
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
            return high.Color;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        return low.Color;
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
            low.Color,
            high.Color,
            MyMath.Map(value, low.Position, high.Position, 0, 1)
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
        if (value <= sortedStops[0].Position)
            return (null, sortedStops[0], false, null);
        if (value >= sortedStops[^1].Position)
            return (sortedStops[^1], null, false, null);

        // Binary search for the closest lower and upper ColorStops
        while (left <= right)
        {
            int mid = (left + right) / 2;
            float midPos = sortedStops[mid].Position;

            // Exact match found
            if (value == midPos)
            {
                return (null, null, true, sortedStops[mid].Color);
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
