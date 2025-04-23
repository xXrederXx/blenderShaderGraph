using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public enum MaskTextureType
{
    SquareFade,
    EaseInSine,
    Square,
    Cube,
}

public record MaskTextureProps
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int NumDots { get; set; }
    public int MaxDotSize { get; set; }
    public int MinDotSize { get; set; }
    public MaskTextureType Type { get; set; } = MaskTextureType.Square;
    public bool BetterDistCalc { get; set; } = false;
}

public class MaskTextureNode : Node<MaskTextureProps, Input<float>>
{
    private static Random rng = new();
    private const float halfPI = (float)(Math.PI / 2);

    public MaskTextureNode()
        : base() { }

    public MaskTextureNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override MaskTextureProps SafeProps(MaskTextureProps props)
    {
        return props;
    }

    protected override Input<float> ExecuteInternal(MaskTextureProps props)
    {
        float[,] res = new float[props.Width, props.Height];
        List<(int x, int y, int size)> dots = GetDotsList(
            props.Width,
            props.Height,
            props.NumDots,
            props.MaxDotSize,
            props.MinDotSize
        );

        foreach ((int xCenter, int yCenter, int size) in dots)
        {
            int startX = Math.Max(0, xCenter - size);
            int endX = Math.Min(props.Width, xCenter + size);
            int startY = Math.Max(0, yCenter - size);
            int endY = Math.Min(props.Height, yCenter + size);

            for (int x = startX; x < endX; x++)
            {
                float xNorm = Math.Abs((x - xCenter) / (float)size);

                for (int y = startY; y < endY; y++)
                {
                    float yNorm = Math.Abs((y - yCenter) / (float)size);
                    float value = GetMaskValue(xNorm, yNorm, props.Type, props.BetterDistCalc);
                    value = MyMath.Clamp01(value);

                    res[x, y] = MyMath.Lerp(res[x, y], 1, value);
                }
            }
        }

        return new Input<float>(res);
    }

    protected override MaskTextureProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");

        string modeStr = p.GetString("mode", "square");
        MaskTextureType mode = modeStr switch
        {
            "squareFade" => MaskTextureType.SquareFade,
            "easeInSine" => MaskTextureType.EaseInSine,
            "square" => MaskTextureType.Square,
            "cube" => MaskTextureType.Cube,
            _ => MaskTextureType.Square,
        };
        return new MaskTextureProps()
        {
            Width = p.GetInt("width", 1024),
            Height = p.GetInt("height", 1024),
            NumDots = p.GetInt("dots", 1024),
            MaxDotSize = p.GetInt("maxSize", 124),
            MinDotSize = p.GetInt("minSize", 24),
            Type = mode,
            BetterDistCalc = p.GetBool("betterDistCalc"),
        };
    }

    protected override void AddDataToContext(Input<float> data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
    private static float GetMaskValue(
        float xNorm,
        float yNorm,
        MaskTextureType type,
        bool betterDistCalc
    )
    {
        if (betterDistCalc)
        {
            float dist = MyMath.Clamp01(MathF.Sqrt(xNorm * xNorm + yNorm * yNorm));
            return MaskShape(dist, type);
        }
        else
        {
            float vx = MaskShape(xNorm, type);
            float vy = MaskShape(yNorm, type);
            return 0.5f * (vx + vy);
        }
    }

    private static float MaskShape(float norm, MaskTextureType type)
    {
        return type switch
        {
            MaskTextureType.SquareFade => SquareFade(norm),
            MaskTextureType.EaseInSine => EaseInSine(norm),
            MaskTextureType.Square => Square(norm),
            MaskTextureType.Cube => Cube(norm),
            _ => SquareFade(norm),
        };
    }

    private static List<(int x, int y, int size)> GetDotsList(
        int Width,
        int Height,
        int numDots,
        int maxSize,
        int minSize
    )
    {
        List<(int x, int y, int size)> dots = [];
        int calcedDots = 0;
        int trysMax = numDots * 2;
        for (int i = 0; i < trysMax; i++)
        {
            int size = rng.Next(minSize, maxSize);

            int maxValueX = Width - size;
            int maxValueY = Height - size;
            if (maxValueX < size || maxValueY < size)
            {
                continue;
            }
            int x = rng.Next(size, maxValueX);
            int y = rng.Next(size, maxValueY);
            dots.Add((x, y, size));
            calcedDots++;
            if (calcedDots >= numDots)
            {
                break;
            }
        }
        return dots;
    }

    private static float SquareFade(float dist) => 1 - dist;

    private static float EaseInSine(float dist) => MyMath.CosFast(dist * halfPI);

    private static float Square(float dist) => (1 - dist) * (1 - dist);

    private static float Cube(float dist) => (1 - dist) * (1 - dist) * (1 - dist);
}
