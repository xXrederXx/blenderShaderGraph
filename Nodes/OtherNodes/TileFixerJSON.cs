using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public class TileFixerJSON : JsonNode
{
    public TileFixerJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        Bitmap inp = p.GetBitmap(Id, contex, "image");
        int blur = p.GetInt("blur", 16);

        Bitmap res = TileFixerNode.Apply(inp, blur);

        contex[Id] = res;
    }
}
