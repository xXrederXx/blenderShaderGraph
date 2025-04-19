using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class MaskTextureJSON : JsonNode
{
    public MaskTextureJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        string modeStr = p.GetString("mode", "square");
        MaskTextureType mode = modeStr switch
        {
            "squareFade" => MaskTextureType.SquareFade,
            "easeInSine" => MaskTextureType.EaseInSine,
            "square" => MaskTextureType.Square,
            "cube" => MaskTextureType.Cube,
            _ => MaskTextureType.Square,
        };
        Bitmap res = MaskTexture.Generate(
            p.GetInt("width", 1024),
            p.GetInt("height", 1024),
            p.GetInt("dots", 1024),
            p.GetInt("maxSize", 124),
            p.GetInt("minSize", 24),
            mode,
            p.GetBool("betterDistCalc")
        );
        contex[Id] = res;
    }
}
