using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.InputNodes;

public enum TextureCoordinateType
{
    Object,
}

public record TextureCoordinateProps
{
    public int Width { get; set; }
    public int Height { get; set; }
    public TextureCoordinateType Mode { get; set; } = TextureCoordinateType.Object;
}

public class TextureCoordinateNode : Node<TextureCoordinateProps, Input<MyColor>>
{
    public TextureCoordinateNode()
        : base() { }

    public TextureCoordinateNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override TextureCoordinateProps SafeProps(TextureCoordinateProps props)
    {
        return props;
    }

    protected override Input<MyColor> ExecuteInternal(TextureCoordinateProps props)
    {
        MyColor[,] res = new MyColor[props.Width, props.Height];
        return props.Mode switch
        {
            TextureCoordinateType.Object => new Input<MyColor>(Object(res)),
            _ => new Input<MyColor>(res),
        };
    }

    protected override TextureCoordinateProps ConvertJSONToProps(Dictionary<string, Input> contex)
    {
        JsonElement p = element.GetProperty("params");

        TextureCoordinateType type = p.GetString("mode", "") switch
        {
            "object" => TextureCoordinateType.Object,
            _ => TextureCoordinateType.Object,
        };
        return new()
        {
            Width = p.GetInt("width", 1024),
            Height = p.GetInt("height", 1024),
            Mode = type,
        };
    }

    protected override void AddDataToContext(Input<MyColor> data, Dictionary<string, Input> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
    private static MyColor[,] Object(MyColor[,] img)
    {
        int width = img.GetLength(0);
        int height = img.GetLength(1);

        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    float t = MyMath.Map(x, 0, width, 0, 1);
                    byte r = MyMath.ClampByte(MyMath.Lerp(0, 1, t) * 255);

                    t = MyMath.Map(y, 0, height, 0, 1);
                    byte g = MyMath.ClampByte(MyMath.Lerp(0, 1, t) * 255);

                    img[x, y] = new MyColor(r, g, 0);
                }
            }
        );

        return img;
    }
}
