using System.Drawing;
using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.InputNodes;
using blenderShaderGraph.Nodes.OtherNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks;

/*
| Method    | size | newSize | blendBandSize | Mean        | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|---------- |----- |-------- |-------------- |------------:|----------:|----------:|---------:|---------:|---------:|------------:|
| TileFixer | 128  | 64      | 8             |    32.72 us |  0.139 us |  0.123 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 64      | 16            |    65.00 us |  0.369 us |  0.345 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 64      | 32            |   127.84 us |  0.326 us |  0.289 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 8             |    32.97 us |  0.147 us |  0.137 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 16            |    65.41 us |  1.022 us |  0.956 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 32            |   127.98 us |  0.433 us |  0.405 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 8             |    32.86 us |  0.169 us |  0.150 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 16            |    64.82 us |  0.244 us |  0.228 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 32            |   127.96 us |  0.336 us |  0.281 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 8             |    32.77 us |  0.107 us |  0.100 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 16            |    64.63 us |  0.346 us |  0.324 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 32            |   128.02 us |  0.355 us |  0.332 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 512  | 64      | 8             |   203.16 us |  0.748 us |  0.625 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 64      | 16            |   340.99 us |  0.742 us |  0.694 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 64      | 32            |   597.37 us |  1.713 us |  1.602 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 256     | 8             |   210.41 us |  0.392 us |  0.327 us | 250.0000 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 256     | 16            |   334.77 us |  0.953 us |  0.891 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 256     | 32            |   596.64 us |  2.264 us |  1.891 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 1024    | 8             |   204.41 us |  0.437 us |  0.387 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 1024    | 16            |   341.09 us |  0.642 us |  0.601 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 1024    | 32            |   603.09 us |  2.602 us |  2.434 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 4048    | 8             |   210.58 us |  0.340 us |  0.318 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 4048    | 16            |   341.17 us |  0.924 us |  0.865 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 4048    | 32            |   605.98 us |  1.213 us |  1.135 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 1024 | 64      | 8             |   591.64 us |  1.559 us |  1.458 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 64      | 16            |   843.87 us |  2.069 us |  1.834 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 64      | 32            | 1,332.65 us |  3.259 us |  2.722 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 8             |   578.59 us |  1.300 us |  1.085 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 16            |   849.23 us |  6.184 us |  5.784 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 32            | 1,336.16 us |  2.987 us |  2.332 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 8             |   582.91 us |  1.904 us |  1.687 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 16            |   850.06 us |  4.344 us |  4.064 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 32            | 1,337.34 us |  5.301 us |  4.958 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 8             |   580.55 us |  1.667 us |  1.560 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 16            |   842.98 us |  3.623 us |  3.389 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 32            | 1,353.22 us |  5.330 us |  4.725 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 2024 | 64      | 8             | 3,289.88 us | 30.214 us | 28.262 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 64      | 16            | 3,789.60 us | 54.449 us | 50.932 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 64      | 32            | 4,741.54 us | 48.631 us | 45.489 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 256     | 8             | 3,011.76 us | 31.468 us | 29.435 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 256     | 16            | 3,633.91 us | 50.475 us | 39.407 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 256     | 32            | 4,500.80 us | 11.561 us | 10.249 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 8             | 2,944.50 us | 29.659 us | 27.743 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 16            | 3,460.44 us | 11.697 us | 10.941 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 32            | 4,813.76 us | 22.553 us | 19.993 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 8             | 2,923.28 us | 25.277 us | 23.644 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 16            | 3,589.36 us | 16.928 us | 15.834 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 32            | 4,520.85 us | 30.317 us | 26.875 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
*/

[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchTileFixer
{
    public MyColor[,]? image;

    [Params(128, 512, 1024, 2024)]
    public int size;

    [Params(64, 256, 1024, 4048)]
    public int newSize;

    [Params(8, 16, 32)]
    public int blendBandSize;

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
    public Input<MyColor> TileFixer()
    {
        return new TileFixerNode().ExecuteNode(
            new() { Image = image, BlendBandSize = blendBandSize }
        );
    }
}
