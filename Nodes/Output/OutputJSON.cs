using System;
using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.Output;


public class OutputNodeJson : IJsonNode
{
    public OutputNodeJson(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> context)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap img = p.GetBitmap(Id, context, "image");
        string fn = p.GetString("filename", "out.png");

        img.Save(fn);
        Console.WriteLine($"Saved: {fn}");
    }
}

