using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public record ResizeProps
{
    public Input<Bitmap>? Image { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class ResizeNode : Node<ResizeProps, Input<Bitmap>>
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

    protected override Input<Bitmap> ExecuteInternal(ResizeProps props)
    {
        if (props.Image is null || props.Image.Value is null)
        {
            return new(new Bitmap(1024, 1024));
        }
        return new(new Bitmap(props.Image.Value, props.Width, props.Height));
    }

    protected override ResizeProps ConvertJSONToProps(Dictionary<string, Input> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new()
        {
            Width = p.GetInt("width", 1024),
            Height = p.GetInt("height", 1024),
            Image = p.GetBitmap(Id, contex, "image"),
        };
    }

    protected override void AddDataToContext(Input<Bitmap> data, Dictionary<string, Input> contex)
    {
        contex[Id] = data;
    }
}
