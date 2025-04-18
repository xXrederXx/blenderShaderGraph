using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public class ResizeJSON : JsonNode
{
    public ResizeJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap res = ResizeNode.Generate(
            p.GetBitmap(Id, contex, "image"),
            p.GetInt("width", 1024),
            p.GetInt("height", 1024)
        );
        contex[Id] = res;
    }
}
