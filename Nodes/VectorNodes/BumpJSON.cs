using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Nodes.VectorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class BumpJSON : JsonNode
{
    public BumpJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        Bitmap res = BumpNode.GenerateBump(
            new(
                p.GetBitmap(Id, contex, "heightMap"),
                strength: p.GetFloat("strength", 1),
                distance: p.GetFloat("distance", 1),
                invert: p.GetBool("invert"),
                format: p.GetBool("isDX") == true ? NormalMapFormat.DirectX : NormalMapFormat.OpenGL
            )
        );
        contex[Id] = res;
    }
}
