using System.Diagnostics;
using System.Text.Json;
using blenderShaderGraph.Nodes;
using blenderShaderGraph.Nodes.ConverterNodes;
using blenderShaderGraph.Nodes.InputNodes;
using blenderShaderGraph.Nodes.OtherNodes;
using blenderShaderGraph.Nodes.OutputNodes;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Util;

public class GraphRunner
{
    static Stopwatch sw1 = new();
    static Stopwatch sw2 = new();

    public static void Run(string path)
    {
        System.Console.WriteLine("NOT IN USE");

        /* sw1.Restart();

        System.Console.WriteLine(
            "\n------------------------ Generation Starting ------------------------\n"
        );
        System.Console.WriteLine("\t- Parsing JSON File");

        string json = File.ReadAllText(path);
        JsonDocument doc = JsonDocument.Parse(json);
        List<JsonNode> nodes = new List<JsonNode>();

        System.Console.WriteLine("\t- Parsing Contnents");

        foreach (JsonElement element in doc.RootElement.EnumerateArray())
        {
            string? type = element.GetProperty("type").GetString();
            string id = element.GetProperty("id").GetString() ?? "";

            JsonNode node = type switch
            {
                "BrickTexture" => new BrickTextureJSON(id, element),
                "NoiseTexture" => new NoiseTextureJSON(id, element),
                "MixColor" => new MixColorJSON(id, element),
                "Bump" => new BumpJSON(id, element),
                "ColorRamp" => new ColorRampNode(id, element),
                "Output" => new OutputNodeJson(id, element),
                "TileFixer" => new TileFixerJSON(id, element),
                "Resize" => new ResizeJSON(id, element),
                "TextureCoordinate" => new TextureCoordinateJSON(id, element),
                "MaskTexture" => new MaskTextureJSON(id, element),
                _ => throw new Exception($"Unknown node type: {type}"),
            };

            nodes.Add(node);
        }

        System.Console.WriteLine("\t- Executing Nodes");

        Dictionary<string, object> context = new Dictionary<string, object>();
        foreach (JsonNode node in nodes)
        {
            sw2.Restart();
            node.Execute(context);
            sw2.Stop();
            System.Console.WriteLine(
                $"\t  --> Executed Node {node.Id} ({node.GetType().Name.Replace("JSON", "").Replace("Json", "")}) in {sw2.ElapsedMilliseconds}ms"
            );
        }

        sw1.Stop();

        System.Console.WriteLine(
            $"\n------------------------ Finished in {sw1.ElapsedMilliseconds}ms ------------------------\n"
        ); */
    }
}
