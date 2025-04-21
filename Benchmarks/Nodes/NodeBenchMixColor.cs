using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Benchmarks;
/* 
| Method   | size | mode        | Mean         | Error      | StdDev     | Gen0     | Gen1     | Gen2     | Allocated   |
|--------- |----- |------------ |-------------:|-----------:|-----------:|---------:|---------:|---------:|------------:|
| MixColor | 128  | Mix         |     49.02 us |   0.184 us |   0.154 us |   4.1504 |   0.9155 |        - |    68.03 KB |
| MixColor | 128  | Hue         |    127.89 us |   0.253 us |   0.237 us |   4.1504 |   0.7324 |        - |    68.23 KB |
| MixColor | 128  | Saturation  |    127.31 us |   0.502 us |   0.445 us |   4.1504 |   0.7324 |        - |    68.21 KB |
| MixColor | 128  | Value       |    129.65 us |   0.588 us |   0.491 us |   4.1504 |   0.7324 |        - |    68.22 KB |
| MixColor | 128  | Darken      |     53.65 us |   0.082 us |   0.077 us |   4.2114 |   0.8545 |        - |    68.07 KB |
| MixColor | 128  | LinearLight |     61.36 us |   0.101 us |   0.079 us |   4.2114 |   0.8545 |        - |     68.1 KB |
| MixColor | 128  | Lighten     |     65.47 us |   0.250 us |   0.234 us |   4.1504 |   0.8545 |        - |    68.13 KB |
| MixColor | 512  | Mix         |    580.19 us |   1.156 us |   0.902 us | 250.9766 | 249.0234 | 249.0234 |  1037.11 KB |
| MixColor | 512  | Hue         |  1,464.13 us |  26.246 us |  24.550 us | 250.0000 | 248.0469 | 248.0469 |  1036.71 KB |
| MixColor | 512  | Saturation  |  1,446.73 us |   3.134 us |   2.778 us | 250.0000 | 248.0469 | 248.0469 |  1036.54 KB |
| MixColor | 512  | Value       |  1,487.01 us |  27.256 us |  33.473 us | 250.0000 | 248.0469 | 248.0469 |  1037.05 KB |
| MixColor | 512  | Darken      |    624.39 us |   1.180 us |   1.046 us | 250.9766 | 249.0234 | 249.0234 |  1037.53 KB |
| MixColor | 512  | LinearLight |    708.06 us |   2.197 us |   1.835 us | 250.9766 | 249.0234 | 249.0234 |  1038.06 KB |
| MixColor | 512  | Lighten     |    747.31 us |   2.110 us |   1.647 us | 250.9766 | 249.0234 | 249.0234 |   1037.5 KB |
| MixColor | 1024 | Mix         |  2,080.48 us |  11.372 us |  10.637 us | 156.2500 | 156.2500 | 156.2500 |  4104.25 KB |
| MixColor | 1024 | Hue         |  5,345.30 us |  24.742 us |  20.661 us | 156.2500 | 156.2500 | 156.2500 |  4103.99 KB |
| MixColor | 1024 | Saturation  |  5,340.50 us |  18.894 us |  16.749 us | 156.2500 | 156.2500 | 156.2500 |  4103.89 KB |
| MixColor | 1024 | Value       |  5,443.30 us |  16.369 us |  13.669 us | 156.2500 | 156.2500 | 156.2500 |  4104.15 KB |
| MixColor | 1024 | Darken      |  2,257.54 us |   9.281 us |   7.246 us | 156.2500 | 156.2500 | 156.2500 |  4104.53 KB |
| MixColor | 1024 | LinearLight |  2,581.43 us |   5.961 us |   5.284 us | 156.2500 | 156.2500 | 156.2500 |  4104.18 KB |
| MixColor | 1024 | Lighten     |  2,725.18 us |  10.820 us |   9.035 us | 156.2500 | 156.2500 | 156.2500 |  4104.12 KB |
| MixColor | 2024 | Mix         |  7,649.91 us |  46.090 us |  40.857 us | 156.2500 | 156.2500 | 156.2500 | 16007.11 KB |
| MixColor | 2024 | Hue         | 20,336.31 us |  60.656 us |  53.770 us | 125.0000 | 125.0000 | 125.0000 | 16009.78 KB |
| MixColor | 2024 | Saturation  | 20,379.20 us | 102.067 us |  90.480 us | 125.0000 | 125.0000 | 125.0000 | 16010.37 KB |
| MixColor | 2024 | Value       | 20,573.69 us |  85.323 us |  71.249 us | 125.0000 | 125.0000 | 125.0000 | 16010.16 KB |
| MixColor | 2024 | Darken      |  8,375.51 us |  22.396 us |  19.853 us | 156.2500 | 156.2500 | 156.2500 | 16008.06 KB |
| MixColor | 2024 | LinearLight |  9,747.75 us | 181.903 us | 186.801 us | 156.2500 | 156.2500 | 156.2500 | 16007.82 KB |
| MixColor | 2024 | Lighten     | 10,246.11 us | 105.390 us |  93.426 us | 156.2500 | 156.2500 | 156.2500 | 16008.31 KB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchMixColor
{
    public MyColor[,]? image1;
    public MyColor[,]? image2;

    [Params(128, 512, 1024, 2024)]
    public int size;

    [Params(
        MixColorMode.Mix,
        MixColorMode.Hue,
        MixColorMode.Saturation,
        MixColorMode.Value,
        MixColorMode.Lighten,
        MixColorMode.Darken,
        MixColorMode.LinearLight
    )]
    public MixColorMode mode;

    [GlobalSetup]
    public void Setup()
    {
        image1 = new MyColor[size, size];
        image2 = new MyColor[size, size];
    }

    [Benchmark]
    public Input<MyColor> MixColor()
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return new MixColorNode().ExecuteNode(
            new()
            {
                a = new(image1),
                b = new(image2),
                factor = new(0.69420f),
                Mode = mode,
            }
        );
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
