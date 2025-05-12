using System.Drawing;
/*
Input<MyColor> input = NodeInstances.colorRamp.ExecuteNode(
    new()
    {
        Image = new(
            NodeInstances.noiseTexture.ExecuteNode(
                new()
                {
                    ImgWidth = 2024,
                    ImgHeight = 2024,
                    Scale = 0.1f,
                }
            )
        ),
        ColorStops = [new(new MyColor(0, 0, 0), 0.3f), new(new MyColor(255, 255, 255), 0.5f)],
        Mode = blenderShaderGraph.Nodes.ConverterNodes.ColorRampMode.Constant,
    }
);

Dictionary<string, object> uniforms = new Dictionary<string, object>
{
    { "inputTex1", NodeInstances.noiseTexture.ExecuteNode(new() { Scale = 0.5f }) },
    { "inputTex2", NodeInstances.noiseTexture.ExecuteNode(new() { Scale = 2f, Roughness = 5 }) },
    { "factorTex", input.Array },
    { "type", 0 },
};
ShaderRunner.PreloadShaders(["./shaders/tmp.frag", "./shaders/Nodes/MixColor.frag"]);
Bitmap bitmap = new(2024, 2024);
var pix = ShaderRunner.RunShaderToColorArray("./shaders/Nodes/MixColor.frag", 2024, 2024, uniforms);
bitmap.SetMyPixles(pix);
bitmap.Save("tmp.png");
 */

/* BenchmarkRunner.Run<NodeBenchNoiseTexture>(); */

/* float[,] noise = new NoiseTextureNode().ExecuteNode(new NoiseTextureProps() { });
MyColor[,] col = Converter.ConvertToColor(noise);
Bitmap img = new Bitmap(col.GetLength(0), col.GetLength(1));
img.SetMyPixles(col);
img.Save("tmp.png");
return; */

/* ShaderRunner.PreloadShaders(["./shaders/tmp.frag", "./shaders/Nodes/MixColor.frag"]);
string content = "";
string newContent = "";
string fp = "./graph.sg.json";
while (true)
{
    content = newContent;

    try
    {
        GraphRunner.Run(fp);
    }
    catch (Exception err)
    {
        System.Console.WriteLine(err.Message + err.StackTrace);
    }
    while (content == newContent)
    {
        Thread.Sleep(500); // Ajust as needed
        StreamReader x = File.OpenText(fp);
        newContent = x.ReadToEnd();
        x.Close();
    }
}

 */

using System.Net;
using System.Text;
using BenchmarkDotNet.Running;
using blenderShaderGraph.Benchmarks;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

class Program
{
    static void Main()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/generate-image/");
        listener.Start();
        Console.WriteLine("Server listening on http://localhost:5000/generate-image/");

        while (true)
        {
            var context = listener.GetContext();
            Task.Run(() => HandleRequest(context));
        }
    }

    static void HandleRequest(HttpListenerContext context)
    {
        try
        {
            if (context.Request.HttpMethod != "POST")
            {
                context.Response.StatusCode = 405;
                context.Response.Close();
                return;
            }

            using var reader = new StreamReader(
                context.Request.InputStream,
                context.Request.ContentEncoding
            );
            string json = reader.ReadToEnd();
            byte[] imageBytes = GraphRunner.RunFromJSON(json); // Replace with your logic

            context.Response.ContentType = "image/png";
            context.Response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
            context.Response.OutputStream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            var buffer = Encoding.UTF8.GetBytes("Internal Server Error");
            context.Response.StatusCode = 500;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
