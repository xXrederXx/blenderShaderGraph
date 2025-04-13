using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public class BrickTextureJSON : IJsonNode
{
    public BrickTextureJSON(string id, JsonElement element)
        : base(id, element) { }

    public override void Execute(Dictionary<string, object> contex)
    {
        JsonElement p = _element.GetProperty("params");
        (Bitmap color, Bitmap fac) = BrickTextureNode.Generate(
            new(
                imgWidth: p.GetInt("width", 1024),
                imgHeight: p.GetInt("height", 1024),
                offset: p.GetFloat("offset", 0.5f),
                offsetFrequency: p.GetInt("offsetFrequency", 2),
                squash: p.GetFloat("squash", 1),
                squashFrequency: p.GetInt("squashFrequency", 0),
                color1: ColorTranslator.FromHtml(p.GetString("color1", "black")),
                color2: ColorTranslator.FromHtml(p.GetString("color2", "black")),
                colorMotar: ColorTranslator.FromHtml(p.GetString("colorMotar", "white")),
                motarSize: p.GetFloat("motarSize", 5),
                motarSmoothness: p.GetFloat("motarSmoothness", 0),
                bias: p.GetFloat("bias", 0),
                brickWidth: p.GetFloat("brickWidth", 30),
                rowHeight: p.GetFloat("rowHeight", 12),
                forceTilable: p.GetBool("forceTilable")
            )
        );
        contex[Id + ".color"] = color;
        contex[Id + ".fac"] = fac;
        contex[Id] = color;
    }
}
