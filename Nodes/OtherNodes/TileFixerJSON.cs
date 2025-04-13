using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.OtherNodes;

public class TileFixerJSON : IJsonNode
{
    public TileFixerJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        Bitmap res = new Bitmap(p.GetBitmap(Id, contex, "image"));
        int blur = p.GetInt("blur", 16);

        TileFixerNode.Apply(res, blur);
        
        contex[Id] = res;
    }
}
