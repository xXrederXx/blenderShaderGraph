using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class MixColorJSON : JsonNode
{
    public MixColorJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");

        string modeStr = p.GetString("mode", "mix");
        MixColorMode mode = modeStr switch
        {
            "mix" => MixColorMode.Mix,
            "hue" => MixColorMode.Hue,
            "saturation" => MixColorMode.Saturation,
            "value" => MixColorMode.Value,
            "darken" => MixColorMode.Darken,
            "lighten" => MixColorMode.Lighten,
            "linearlight" => MixColorMode.LinearLight,
            _ => MixColorMode.Mix,
        };
        Bitmap res;
        if (p.GetProperty("factor").ValueKind == JsonValueKind.Number)
        {
            res = MixColorNode.Generate(
                p.GetBitmap(Id, contex, "a"),
                p.GetBitmap(Id, contex, "b"),
                p.GetFloat("factor", 0),
                mode
            );
        }
        else
        {
            Bitmap factor = p.GetBitmap(Id, contex, "factor");
            res = MixColorNode.Generate(
                p.GetBitmap(Id, contex, "a", factor.Width, factor.Height),
                p.GetBitmap(Id, contex, "b", factor.Width, factor.Height),
                factor,
                mode
            );
        }
        contex[Id] = res;
    }
}
