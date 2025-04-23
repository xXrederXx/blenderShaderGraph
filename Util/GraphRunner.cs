using System.Diagnostics;
using System.Text.Json;
using blenderShaderGraph.Nodes;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Nodes.ConverterNodes;
using blenderShaderGraph.Nodes.InputNodes;
using blenderShaderGraph.Nodes.OtherNodes;
using blenderShaderGraph.Nodes.OutputNodes;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Nodes.VectorNodes;
using blenderShaderGraph.Types;
using Microsoft.Extensions.Logging;

namespace blenderShaderGraph.Util;

public class GraphRunner
{
    static Stopwatch sw1 = new();
    static Stopwatch sw2 = new();

    public static void Run(string path)
    {
        System.Console.WriteLine("NOT IN USE");

        sw1.Restart();

        System.Console.WriteLine(
            "\n------------------------ Generation Starting ------------------------\n"
        );
        System.Console.WriteLine("\t- Parsing JSON File");

        string json = File.ReadAllText(path);
        JsonDocument doc = JsonDocument.Parse(json);
        List<Node> nodes = new List<Node>();

        System.Console.WriteLine("\t- Parsing Contnents");

        foreach (JsonElement element in doc.RootElement.EnumerateArray())
        {
            string? type = element.GetProperty("type").GetString();
            string id = element.GetProperty("id").GetString() ?? "";

            Node node = type switch
            {
                "BrickTexture" => new BrickTextureNode(id, element),
                "NoiseTexture" => new NoiseTextureNode(id, element),
                "MixColor" => new MixColorNode(id, element),
                "Bump" => new BumpNode(id, element),
                "ColorRamp" => new ColorRampNode(id, element),
                "Output" => new OutputNode(id, element),
                "TileFixer" => new TileFixerNode(id, element),
                "Resize" => new ResizeNode(id, element),
                "TextureCoordinate" => new TextureCoordinateNode(id, element),
                "MaskTexture" => new MaskTextureNode(id, element),
                _ => throw new Exception($"Unknown node type: {type}"),
            };

            nodes.Add(node);
        }

        System.Console.WriteLine("\t- Executing Nodes");

        Dictionary<string, object> context = new Dictionary<string, object>();
        foreach (Node node in nodes)
        {
            sw2.Restart();
            node.ExecuteNodeJSON(context);
            sw2.Stop();
            System.Console.WriteLine(
                $"\t  --> Executed Node {node.Id} ({node.GetType().Name}) in {sw2.ElapsedMilliseconds}ms"
            );
        }

        sw1.Stop();

        System.Console.WriteLine(
            $"\n------------------------ Finished in {sw1.ElapsedMilliseconds}ms ------------------------\n"
        );
    }
}
