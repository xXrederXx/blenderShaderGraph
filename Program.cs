using System.Drawing;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

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

/* BenchmarkRunner.Run<TmpBench>(); */

/* float[,] noise = new NoiseTextureNode().ExecuteNode(new NoiseTextureProps() { });
MyColor[,] col = Converter.ConvertToColor(noise);
Bitmap img = new Bitmap(col.GetLength(0), col.GetLength(1));
img.SetMyPixles(col);
img.Save("tmp.png");
return;

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
