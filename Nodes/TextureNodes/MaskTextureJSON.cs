using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class MaskTextureJSON : IJsonNode
{
    public MaskTextureJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        string modeStr = p.GetString("type", "square");
        MaskTextureType mode = modeStr switch
        {
            "square" => MaskTextureType.Square,
            _ => MaskTextureType.Square,
        };
        Bitmap res = MaskTexture.Generate(
            p.GetInt("width", 1024),
            p.GetInt("height", 1024),
            p.GetInt("dots", 1024),
            p.GetInt("minSize", 1024),
            p.GetInt("maxSize", 1024),
            mode
        );
        contex[Id] = res;
    }
}
