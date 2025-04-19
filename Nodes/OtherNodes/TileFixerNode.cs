using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public record TileFixerProps
{
    public MyColor[,]? Image { get; set; }
    public int BlendBandSize { get; set; }
}

public class TileFixerNode : Node<TileFixerProps, MyColor[,]>
{
    public TileFixerNode()
        : base() { }

    public TileFixerNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override TileFixerProps SafeProps(TileFixerProps props)
    {
        if (props.Image is null)
            System.Console.WriteLine("Birmap is null");
        return props;
    }

    protected override MyColor[,] ExecuteInternal(TileFixerProps props)
    {
        if (props.Image is null)
        {
            return new MyColor[0, 0];
        }
        int width = props.Image.GetLength(0);
        int height = props.Image.GetLength(1);

        MyColor[,] newColors = new MyColor[width, height];

        // Blend left ↔ right edges
        for (int x = 0; x < props.BlendBandSize; x++)
        {
            float t = SmoothFalloff(x / (float)props.BlendBandSize);

            for (int y = 0; y < height; y++)
            {
                // Left side
                Color origLeft = props.Image[x, y];
                Color mirrorRight = props.Image[width - props.BlendBandSize + x, y];
                Color blendL = ColorUtil.LerpColor(mirrorRight, origLeft, t);
                newColors[x, y] = blendL;

                // Right side
                Color origRight = props.Image[width - 1 - x, y];
                Color mirrorLeft = props.Image[props.BlendBandSize - 1 - x, y];
                Color blendR = ColorUtil.LerpColor(mirrorLeft, origRight, t);
                newColors[width - 1 - x, y] = blendR;
            }
        }

        // Blend top ↔ bottom edges
        for (int y = 0; y < props.BlendBandSize; y++)
        {
            float t = SmoothFalloff(y / (float)props.BlendBandSize);

            for (int x = 0; x < width; x++)
            {
                // Top side
                Color origTop = props.Image[x, y];
                Color mirrorBottom = props.Image[x, height - props.BlendBandSize + y];
                Color blendT = ColorUtil.LerpColor(mirrorBottom, origTop, t);
                newColors[x, y] = blendT;

                // Bottom side
                Color origBottom = props.Image[x, height - 1 - y];
                Color mirrorTop = props.Image[x, props.BlendBandSize - 1 - y];
                Color blendB = ColorUtil.LerpColor(mirrorTop, origBottom, t);
                newColors[x, height - 1 - y] = blendB;
            }
        }
        return newColors;
    }

    protected override TileFixerProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new TileFixerProps()
        {
            Image = p.GetMyColor2D(Id, contex, "image").Array,
            BlendBandSize = p.GetInt("blur", 16),
        };
    }

    protected override void AddDataToContext(MyColor[,] data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }

    //NODE SPESIFIC
    // Falloff for smoother blending — cosine gives a soft rolloff
    private static float SmoothFalloff(float x)
    {
        return (1 - MathF.Cos(x * MathF.PI)) * 0.5f;
    }
}
