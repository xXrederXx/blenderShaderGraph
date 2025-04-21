using System.Diagnostics;
using System.Drawing;
using BenchmarkDotNet.Running;
using blenderShaderGraph.Benchmarks;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

var uniforms = new Dictionary<string, float>
        {
            { "sizeX", 3.0f },
            { "sizeY", 2.0f }
        };
ShaderRunner.RunShaderToColorArray("./shaders/tmp.frag", 1024, 1024, uniforms);
Stopwatch sw = new(); sw.Start();
ShaderRunner.RunShaderToColorArray("./shaders/tmp.frag", 1024, 1024, uniforms);
sw.Stop();
System.Console.WriteLine(sw.ElapsedMilliseconds);
return;
BenchmarkRunner.Run<TmpBench>();
return;
float[,] noise = new NoiseTextureNode().ExecuteNode(new NoiseTextureProps() { });
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
