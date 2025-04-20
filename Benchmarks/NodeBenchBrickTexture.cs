using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks;

/*
| Method       | size | smooth | force | Mean        | Error      | StdDev     | Median      | Gen0     | Gen1     | Gen2     | Allocated   |
|------------- |----- |------- |------ |------------:|-----------:|-----------:|------------:|---------:|---------:|---------:|------------:|
| BrickTexture | 128  | 0      | False |    37.28 us |   0.414 us |   0.367 us |    37.18 us |   8.1177 |   2.3804 |        - |   132.01 KB |
| BrickTexture | 128  | 0      | True  |    38.13 us |   0.468 us |   0.390 us |    38.11 us |   8.1177 |   2.3193 |        - |   131.96 KB |
| BrickTexture | 128  | 1      | False |    36.62 us |   0.226 us |   0.189 us |    36.61 us |   8.1177 |   2.5635 |        - |   132.01 KB |
| BrickTexture | 128  | 1      | True  |    40.67 us |   0.430 us |   0.382 us |    40.79 us |   8.1177 |   2.3193 |        - |   131.96 KB |
| BrickTexture | 512  | 0      | False |   721.60 us |   3.522 us |   2.941 us |   721.01 us | 502.9297 | 499.0234 | 499.0234 |     2072 KB |
| BrickTexture | 512  | 0      | True  |   717.83 us |   2.297 us |   1.918 us |   717.96 us | 502.9297 | 499.0234 | 499.0234 |  2071.46 KB |
| BrickTexture | 512  | 1      | False |   717.69 us |   2.486 us |   1.941 us |   717.32 us | 502.9297 | 499.0234 | 499.0234 |  2072.55 KB |
| BrickTexture | 512  | 1      | True  |   735.62 us |  14.389 us |  21.974 us |   728.08 us | 502.9297 | 499.0234 | 499.0234 |  2073.01 KB |
| BrickTexture | 1024 | 0      | False | 2,291.88 us |  35.973 us |  42.823 us | 2,277.16 us | 496.0938 | 496.0938 | 496.0938 |  8202.09 KB |
| BrickTexture | 1024 | 0      | True  | 2,852.51 us |  54.318 us |  68.695 us | 2,838.05 us | 484.3750 | 484.3750 | 484.3750 |  8204.62 KB |
| BrickTexture | 1024 | 1      | False | 2,668.75 us |  77.128 us | 224.986 us | 2,755.99 us | 496.0938 | 496.0938 | 496.0938 |     8203 KB |
| BrickTexture | 1024 | 1      | True  | 2,321.70 us |  14.472 us |  13.537 us | 2,319.20 us | 496.0938 | 496.0938 | 496.0938 |  8201.97 KB |
| BrickTexture | 2024 | 0      | False | 9,410.14 us | 102.987 us |  80.406 us | 9,415.06 us | 500.0000 | 500.0000 | 500.0000 | 32009.05 KB |
| BrickTexture | 2024 | 0      | True  | 9,439.38 us |  62.689 us |  58.639 us | 9,424.67 us | 500.0000 | 500.0000 | 500.0000 | 32009.05 KB |
| BrickTexture | 2024 | 1      | False | 9,293.76 us | 132.001 us | 117.016 us | 9,267.65 us | 500.0000 | 500.0000 | 500.0000 | 32009.04 KB |
| BrickTexture | 2024 | 1      | True  | 9,310.19 us |  58.945 us |  55.137 us | 9,329.95 us | 500.0000 | 500.0000 | 500.0000 | 32009.04 KB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchBrickTexture
{

    [Params(128, 512, 1024, 2024)]
    public int size;

    [Params(0, 1)]
    public int smooth;

    [Params(true, false)]
    public bool force;

    [Benchmark]
    public MyColor[,] BrickTexture()
    {
        return new BrickTextureNode()
            .ExecuteNode(
                new(
                    imgWidth: size,
                    imgHeight: size,
                    squash: 2f,
                    squashFrequency: 3,
                    bias: 0.2f,
                    forceTilable: force,
                    motarSmoothness: smooth
                )
            )
            .color;
    }
}
