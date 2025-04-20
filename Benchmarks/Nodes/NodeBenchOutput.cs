using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.OutputNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks;

/*
| Method    | size | Mean        | Error     | StdDev    | Allocated |
|---------- |----- |------------:|----------:|----------:|----------:|
| TileFixer | 128  |    571.3 us |   4.80 us |   4.49 us |     153 B |
| TileFixer | 512  |  4,096.0 us |  32.31 us |  30.22 us |     156 B |
| TileFixer | 1024 | 15,240.2 us |  49.23 us |  43.64 us |     159 B |
| TileFixer | 2024 | 58,230.2 us | 129.75 us | 108.35 us |     202 B |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchOutput
{
    public MyColor[,]? image;

    [Params(128, 512, 1024, 2024)]
    public int size;

    [GlobalSetup]
    public void Setup()
    {
        Random rng = new(696969696);
        if (image is null)
        {
            image = new MyColor[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    byte v = MyMath.ClampByte((float)rng.NextDouble() * 255);
                    image[i, j] = new(v, v, v);
                }
            }
        }
    }

    [Benchmark]
    public bool TileFixer()
    {
        return new OutputNode().ExecuteNode(
            new() { Image = image,  FileName = "tmp.png", WriteLine = false}
        );
    }
}
