using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.InputNodes;

public class TextureCoordinateJSON : JsonNode
{
    public TextureCoordinateJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        TextureCoordinateType type = p.GetString("mode", "") switch {
            "object" => TextureCoordinateType.Object,
            _ => TextureCoordinateType.Object
        };
        Bitmap res = TextureCoordinate.Genearate(
               p.GetInt("width", 1024),
               p.GetInt("height", 1024),
                type
        );
        contex[Id] = res;
    }
}
