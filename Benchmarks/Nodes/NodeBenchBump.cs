using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Nodes.VectorNodes;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Benchmarks;

/*
| Method | size | Mean         | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|------- |----- |-------------:|----------:|----------:|---------:|---------:|---------:|------------:|
| Bump   | 128  |     87.25 us |  1.306 us |  1.158 us |   4.1504 |   0.8545 |        - |    68.15 KB |
| Bump   | 512  |    950.30 us |  2.464 us |  2.184 us | 250.9766 | 249.0234 | 249.0234 |  1037.68 KB |
| Bump   | 1024 |  3,555.42 us | 10.278 us |  8.583 us | 394.5313 | 394.5313 | 394.5313 |  4113.97 KB |
| Bump   | 2024 | 16,963.57 us | 86.868 us | 72.539 us | 875.0000 | 875.0000 | 875.0000 | 16007.19 KB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchBump
{
    [Params(128, 512, 1024, 2024)]
    public int size;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    float[,] img;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [GlobalSetup]
    public void Setup()
    {
        img = new NoiseTextureNode().ExecuteNode(new() { ImgWidth = size, ImgHeight = size }).Array!;
    }

    [Benchmark]
    public Input<MyColor> Bump()
    {
        return new BumpNode().ExecuteNode(new(img, new(1.52f), new(2.7f), true, NormalMapFormat.OpenGL));
    }
}
