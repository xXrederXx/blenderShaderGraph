using System.Diagnostics;
using System.Drawing;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

Dictionary<string, object> uniforms = new Dictionary<string, object>
{
    { "sizeX", 3.0f },
    { "sizeY", 2.0f },
    { "inputTex", NodeInstances.noiseTexture.ExecuteNode(new()) },
};
ShaderRunner.PreloadShaders(["./shaders/tmp.frag"]);
Bitmap bitmap = new(2024, 2024);
var pix = ShaderRunner.RunShaderToColorArray("./shaders/tmp.frag", 2024, 2024, uniforms);
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
