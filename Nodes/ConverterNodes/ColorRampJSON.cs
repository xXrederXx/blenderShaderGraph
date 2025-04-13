using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ConverterNodes;

public class ColorRampJSON : IJsonNode
{
    public ColorRampJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap bmp = p.GetBitmap(Id, contex, "image");
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

        Bitmap res = ColorRampNode.Apply(bmp, stops.ToArray(), mode);
        contex[Id] = res;
    }
}
