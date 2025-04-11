using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class NoiseTextureJSON : IJsonNode
{
    public NoiseTextureJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap res = NoiseTextureNode.Generate(
            new(
                imgWidth: p.GetInt("width", 1024),
                imgHeight: p.GetInt("height", 1024),
                size: p.GetFloat("size", 1),
                detail: p.GetFloat("detail", 2),
                roughness: p.GetFloat("roughness", 0.5f)
            )
        );
        contex[Id] = res;
    }
}
