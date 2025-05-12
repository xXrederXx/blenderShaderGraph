using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.InputNodes;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Benchmarks;

/*
| Method            | size | mode   | Mean        | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|------------------ |----- |------- |------------:|----------:|----------:|---------:|---------:|---------:|------------:|
| TextureCoordinate | 128  | Object |    31.91 us |  0.246 us |  0.205 us |   8.0566 |   1.8311 |        - |   132.01 KB |
| TextureCoordinate | 512  | Object |   470.88 us |  1.139 us |  1.010 us | 503.9063 | 499.5117 | 499.5117 |  2073.24 KB |
| TextureCoordinate | 1024 | Object | 1,489.70 us |  8.952 us |  8.374 us | 500.0000 | 498.0469 | 498.0469 |   8202.1 KB |
| TextureCoordinate | 2024 | Object | 5,609.86 us | 90.473 us | 84.628 us | 500.0000 | 500.0000 | 500.0000 | 32008.99 KB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchTextureCoordinate
{
    public MyColor[,]? image1;

    [Params(128, 512, 1024, 2024)]
    public int size;

    [Params(TextureCoordinateType.Object)]
    public TextureCoordinateType mode;

    [GlobalSetup]
    public void Setup()
    {
        image1 = new MyColor[size, size];
    }

    [Benchmark]
    public Input<MyColor> TextureCoordinate()
    {
        return new TextureCoordinateNode().ExecuteNode(
            new()
            {
                Width = size,
                Height = size,
                Mode = mode,
            }
        );
    }
}
