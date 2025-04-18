using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class NoiseTextureJSON : JsonNode
{
    public NoiseTextureJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap res = NoiseTextureNode.Generate(
            new(
                ImgWidth: p.GetInt("width", 1024),
                ImgHeight: p.GetInt("height", 1024),
                Scale: p.GetFloat("size", 1),
                Detail: p.GetFloat("detail", 2),
                Roughness: p.GetFloat("roughness", 0.5f)
            )
        );
        contex[Id] = res;
    }
}
