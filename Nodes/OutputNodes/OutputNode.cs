using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OutputNodes;

public record OutputProps
{
    public MyColor[,]? Image { get; set; }
    public string? FileName { get; set; }
}

public class OutputNode : Node<OutputProps, bool>
{
    public OutputNode()
        : base() { }

    public OutputNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override OutputProps SafeProps(OutputProps props)
    {
        if (props.Image is null)
            System.Console.WriteLine("Birmap is null");
        if (string.IsNullOrEmpty(props.FileName))
        {
            System.Console.WriteLine("FileName is null or empty, setting it to 'out.png'");
            props.FileName = "out.png";
        }
        return props;
    }

    protected override bool ExecuteInternal(OutputProps props)
    {
        if (props.Image is null || string.IsNullOrEmpty(props.FileName))
        {
            throw new ArgumentException("Could not save image, one argument was null");
        }
        Bitmap res = new Bitmap(props.Image.GetLength(0), props.Image.GetLength(1));
        res.Save(props.FileName);
        Console.WriteLine($"\t- Saved: {props.FileName}");
        return true;
    }

    protected override OutputProps ConvertJSONToProps(Dictionary<string, object> context)
    {
        JsonElement p = element.GetProperty("params");

        return new OutputProps()
        {
            Image = p.GetInputMyColor(Id, context, "image").Array,
            FileName = p.GetString("filename", "out.png"),
        };
    }

    protected override void AddDataToContext(bool data, Dictionary<string, object> contex) { }
}
