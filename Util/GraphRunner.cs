using System.Text.Json;
using blenderShaderGraph.Nodes;
using blenderShaderGraph.Nodes.ConverterNodes;
using blenderShaderGraph.Nodes.OtherNodes;
using blenderShaderGraph.Nodes.OutputNodes;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Util;

public class GraphRunner
{
    public static void Run(string path)
    {
        string json = File.ReadAllText(path);
        JsonDocument doc = JsonDocument.Parse(json);
        List<IJsonNode> nodes = new List<IJsonNode>();

        foreach (JsonElement element in doc.RootElement.EnumerateArray())
        {
            string? type = element.GetProperty("type").GetString();
            string id = element.GetProperty("id").GetString() ?? "";

            IJsonNode node = type switch
            {
                "BrickTexture" => new BrickTextureJSON(id, element),
                "NoiseTexture" => new NoiseTextureJSON(id, element),
                "MixColor" => new MixColorJSON(id, element),
                "Bump" => new BumpJSON(id, element),
                "ColorRamp" => new ColorRampJSON(id, element),
                "Output" => new OutputNodeJson(id, element),
                "TileFixer" => new TileFixerJSON(id, element),
                _ => throw new Exception($"Unknown node type: {type}"),
            };

            nodes.Add(node);
        }

        Dictionary<string, object> context = new Dictionary<string, object>();
        foreach (IJsonNode node in nodes)
            node.Execute(context);
    }
}
