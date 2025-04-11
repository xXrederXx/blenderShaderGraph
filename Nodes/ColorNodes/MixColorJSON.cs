using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class MixColorJSON : IJsonNode
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
            _ => MixColorMode.Mix,
        };

        Bitmap res;
        try
        {
            res = MixColorNode.Generate(
                p.GetBitmap(Id, contex, "a"),
                p.GetBitmap(Id, contex, "b"),
                p.GetBitmap(Id, contex, "factor"),
                mode
            );
        }
        catch
        {
            res = MixColorNode.Generate(
                p.GetBitmap(Id, contex, "a"),
                p.GetBitmap(Id, contex, "b"),
                p.GetFloat("factor", 0.5f),
                mode
            );
        }
        contex[Id] = res;
    }
}
