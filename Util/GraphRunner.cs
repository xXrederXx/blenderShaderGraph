using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        Dictionary<string, Input> context = new Dictionary<string, Input>();
        foreach (Node node in nodes)
        {
            sw2.Restart();
            node.ExecuteNodeJSON(context);
            sw2.Stop();
            System.Console.WriteLine(
                $"\t  --> Executed Node {node.Id} ({node.GetType().Name}) in {sw2.Elapsed.TotalMicroseconds:#,##0.##}us"
            );
        }

        sw1.Stop();

        System.Console.WriteLine(
            $"\n------------------------ Finished in {sw1.Elapsed.TotalMicroseconds:#,##0.##}us ------------------------\n"
        );
    }

    public static byte[] RunFromJSON(string json)
    {
        JsonDocument doc = JsonDocument.Parse(json);
        List<Node> nodes = new List<Node>();
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

        Dictionary<string, Input> context = new Dictionary<string, Input>();
        string lastId = "";
        foreach (Node node in nodes)
        {
            lastId = node.Id;
            node.ExecuteNodeJSON(context);
        }
        Input res = context[lastId];
        return InputToByteArray(res);
    }

    private static byte[] InputToByteArray(Input res)
    {
        if (res is Input<MyColor> colorInput)
        {
            if (colorInput.useArray && colorInput.Array is not null)
            {
                System.Console.WriteLine("Color Array");
                int w = colorInput.Width;
                int h = colorInput.Height;
                var img = new Bitmap(w, h);
                img.SetMyPixles(colorInput.Array);
                using var mems = new MemoryStream();
                img.Save(mems, ImageFormat.Png);
                return mems.ToArray();
            }

            System.Console.WriteLine("Color");
            using var bmp = new Bitmap(200, 100);
            using var gfx = Graphics.FromImage(bmp);
            gfx.Clear(colorInput.Value.Color);
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        if (res is Input<float> floatInput)
        {
            if (floatInput.useArray && floatInput.Array != null)
            {
                System.Console.WriteLine("Float Array");
                int w = floatInput.Width;
                int h = floatInput.Height;
                var img = new Bitmap(w, h);
                img.SetMyPixles(Converter.ConvertToColor(floatInput.Array));
                img.Save("./tmp.png");
                using var mems = new MemoryStream();
                img.Save(mems, ImageFormat.Png);
                return mems.ToArray();
            }

            System.Console.WriteLine("Float");
            byte col  = MyMath.ClampByte(floatInput.Value * 255);
            var bmp = BitmapUtil.FilledBitmap(200, 200, new MyColor(col));
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        // Fallback blank image
        using (var bmp = new Bitmap(200, 100))
        using (var gfx = Graphics.FromImage(bmp))
        using (var ms = new MemoryStream())
        {
            gfx.Clear(Color.Gray);
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
