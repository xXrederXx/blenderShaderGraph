using System.Drawing;
using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.InputNodes;
using blenderShaderGraph.Nodes.OtherNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks;

/*
| Method | size | newSize | mode   | Mean        | Error     | StdDev    | Allocated |
|------- |----- |-------- |------- |------------:|----------:|----------:|----------:|
| Resize | 128  | 64      | Object |    103.0 us |   0.32 us |   0.27 us |     168 B |
| Resize | 128  | 256     | Object |    103.9 us |   0.55 us |   0.49 us |     168 B |
| Resize | 128  | 1024    | Object |    103.5 us |   0.35 us |   0.31 us |     168 B |
| Resize | 128  | 4048    | Object |    103.4 us |   0.65 us |   0.55 us |     168 B |
| Resize | 512  | 64      | Object |    940.4 us |   6.95 us |   6.50 us |     169 B |
| Resize | 512  | 256     | Object |    944.1 us |   7.45 us |   6.97 us |     168 B |
| Resize | 512  | 1024    | Object |    942.3 us |   5.46 us |   4.84 us |     168 B |
| Resize | 512  | 4048    | Object |    942.4 us |   5.49 us |   4.87 us |     168 B |
| Resize | 1024 | 64      | Object |  3,645.5 us |  27.86 us |  26.06 us |     170 B |
| Resize | 1024 | 256     | Object |  3,624.3 us |   9.21 us |   7.19 us |     168 B |
| Resize | 1024 | 1024    | Object |  3,658.7 us |  24.97 us |  23.36 us |     168 B |
| Resize | 1024 | 4048    | Object |  3,637.4 us |  28.16 us |  26.34 us |     168 B |
| Resize | 2024 | 64      | Object | 14,600.7 us | 106.20 us |  94.14 us |     169 B |
| Resize | 2024 | 256     | Object | 14,619.7 us | 104.88 us |  87.58 us |     170 B |
| Resize | 2024 | 1024    | Object | 14,814.1 us | 252.06 us | 247.55 us |     174 B |
| Resize | 2024 | 4048    | Object | 14,787.3 us | 157.86 us | 131.82 us |     174 B |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchResize
{
    public Bitmap? image1;

    [Params(128, 512, 1024, 2024)]
    public int size;
    [Params(64, 256, 1024, 4048)]
    public int newSize;

    [Params(TextureCoordinateType.Object)]
    public TextureCoordinateType mode;

    [GlobalSetup]
    public void Setup()
    {
        image1 = new(size, size);
        image1.SetMyPixles(new MyColor[size, size]);
    }

    [Benchmark]
    public Bitmap Resize()
    {
        return new ResizeNode().ExecuteNode(
            new()
            {
                Width = size,
                Height = size,
                Image = image1,
            }
        );
    }
}
