using System.Drawing;
using BenchmarkDotNet.Running;
using blenderShaderGraph.Benchmarks;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

BenchmarkRunner.Run<NodeBenchTileFixer>();
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
