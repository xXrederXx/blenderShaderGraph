using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Benchmarks;

/*
| Method       | size | Mean        | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|------------- |----- |------------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| NoiseTexture | 128  |    103.5 us |   0.21 us |   0.20 us |   4.1504 |   0.8545 |        - |    68.1 KB |
| NoiseTexture | 512  |  1,129.5 us |   4.19 us |   3.92 us | 332.0313 | 332.0313 | 332.0313 | 1028.24 KB |
| NoiseTexture | 1024 |  4,038.0 us |  20.35 us |  18.04 us | 500.0000 | 500.0000 | 500.0000 | 4107.25 KB |
| NoiseTexture | 2024 | 16,089.9 us | 318.80 us | 341.11 us | 593.7500 | 593.7500 | 593.7500 | 16011.9 KB |

| Method       | size | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------- |----- |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| NoiseTexture | 1024 | 14.26 ms | 0.121 ms | 0.108 ms | 500.0000 | 500.0000 | 500.0000 |   4.01 MB |
| NoiseTexture | 2024 | 54.17 ms | 0.397 ms | 0.371 ms | 500.0000 | 500.0000 | 500.0000 |  15.65 MB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchNoiseTexture
{
    [Params(1024, 2024)]
    public int size;

    NoiseTextureProps props;

    [GlobalSetup]
    public void setup()
    {
        props = new()
        {
            ImgWidth = size,
            ImgHeight = size,
            Detail = 10,
            Lacunarity = 5,
            Roughness = 8,
            Scale = 4,
        };
    }

    [Benchmark]
    public Input<float> NoiseTexture()
    {
        return new NoiseTextureNode().ExecuteNode(props);
    }
}
