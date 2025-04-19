using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public record ResizeProps
{
    public Bitmap? Image { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class ResizeNode : Node<ResizeProps, Bitmap>
{
    public ResizeNode()
        : base() { }

    public ResizeNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override ResizeProps SafeProps(ResizeProps props)
    {
        if (props.Image is null)
            System.Console.WriteLine("Birmap is null");
        return props;
    }

    protected override Bitmap ExecuteInternal(ResizeProps props)
    {
        if (props.Image is null)
        {
            return new Bitmap(1024, 1024);
        }
        return new Bitmap(props.Image, props.Width, props.Height);
    }

    protected override ResizeProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new()
        {
            Width = p.GetInt("width", 1024),
            Height = p.GetInt("height", 1024),
            Image = p.GetBitmap(Id, contex, "image"),
        };
    }

    protected override void AddDataToContext(Bitmap data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }
}
