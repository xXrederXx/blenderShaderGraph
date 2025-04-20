using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Nodes.ConverterNodes;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks;

/*
| Method           | size | mode     | Mean         | Error      | StdDev     | Gen0     | Gen1     | Gen2     | Allocated   |
|----------------- |----- |--------- |-------------:|-----------:|-----------:|---------:|---------:|---------:|------------:|
| ColorRamp2Stops  | 128  | Linear   |     70.67 us |   0.163 us |   0.144 us |   4.1504 |   0.8545 |        - |    68.41 KB |
| ColorRamp8Stops  | 128  | Linear   |     95.44 us |   0.190 us |   0.168 us |   4.1504 |   0.8545 |        - |    68.59 KB |
| ColorRamp16Stops | 128  | Linear   |     95.29 us |   0.274 us |   0.229 us |   4.1504 |   0.8545 |        - |    68.77 KB |
| ColorRamp2Stops  | 128  | Constant |     48.91 us |   0.108 us |   0.101 us |   4.2114 |   0.8545 |        - |    68.34 KB |
| ColorRamp8Stops  | 128  | Constant |     70.56 us |   0.306 us |   0.287 us |   4.1504 |   0.8545 |        - |    68.57 KB |
| ColorRamp16Stops | 128  | Constant |     74.43 us |   0.213 us |   0.178 us |   4.1504 |   0.9766 |        - |    68.75 KB |
| ColorRamp2Stops  | 512  | Linear   |    821.11 us |   1.364 us |   1.209 us | 250.9766 | 249.0234 | 249.0234 |  1038.03 KB |
| ColorRamp8Stops  | 512  | Linear   |  1,045.20 us |   4.500 us |   3.757 us | 250.0000 | 248.0469 | 248.0469 |   1036.8 KB |
| ColorRamp16Stops | 512  | Linear   |  1,096.38 us |   2.797 us |   2.479 us | 250.0000 | 248.0469 | 248.0469 |  1036.07 KB |
| ColorRamp2Stops  | 512  | Constant |    594.93 us |   2.726 us |   2.276 us | 250.9766 | 249.0234 | 249.0234 |  1038.03 KB |
| ColorRamp8Stops  | 512  | Constant |    806.81 us |   2.149 us |   2.010 us | 250.9766 | 249.0234 | 249.0234 |  1038.08 KB |
| ColorRamp16Stops | 512  | Constant |    853.55 us |   2.083 us |   1.847 us | 250.9766 | 249.0234 | 249.0234 |  1037.76 KB |
| ColorRamp2Stops  | 1024 | Linear   |  3,017.03 us |   7.727 us |   6.452 us | 394.5313 | 394.5313 | 394.5313 |  4109.69 KB |
| ColorRamp8Stops  | 1024 | Linear   |  3,863.46 us |  41.892 us |  41.143 us | 394.5313 | 394.5313 | 394.5313 |  4107.49 KB |
| ColorRamp16Stops | 1024 | Linear   |  4,006.91 us |   7.009 us |   5.853 us | 390.6250 | 390.6250 | 390.6250 |  4107.67 KB |
| ColorRamp2Stops  | 1024 | Constant |  2,150.95 us |   6.810 us |   6.370 us | 394.5313 | 394.5313 | 394.5313 |  4111.12 KB |
| ColorRamp8Stops  | 1024 | Constant |  2,957.74 us |  10.186 us |   9.528 us | 394.5313 | 394.5313 | 394.5313 |  4110.34 KB |
| ColorRamp16Stops | 1024 | Constant |  3,132.78 us |  17.262 us |  14.415 us | 394.5313 | 394.5313 | 394.5313 |  4109.83 KB |
| ColorRamp2Stops  | 2024 | Linear   | 12,155.47 us |  70.455 us |  65.903 us | 937.5000 | 937.5000 | 937.5000 |  16007.3 KB |
| ColorRamp8Stops  | 2024 | Linear   | 15,291.66 us | 292.205 us | 286.985 us | 937.5000 | 937.5000 | 937.5000 | 16007.48 KB |
| ColorRamp16Stops | 2024 | Linear   | 16,101.77 us | 241.512 us | 225.910 us | 875.0000 | 875.0000 | 875.0000 | 16007.64 KB |
| ColorRamp2Stops  | 2024 | Constant |  8,740.51 us |  57.547 us |  51.014 us | 937.5000 | 937.5000 | 937.5000 | 16007.28 KB |
| ColorRamp8Stops  | 2024 | Constant | 11,968.20 us |  73.960 us |  61.760 us | 937.5000 | 937.5000 | 937.5000 | 16007.47 KB |
| ColorRamp16Stops | 2024 | Constant | 12,617.30 us |  56.179 us |  52.550 us | 937.5000 | 937.5000 | 937.5000 | 16007.64 KB |
 */
[SimpleJob]
[MemoryDiagnoser]
public class NodeBenchColorRamp
{
    public MyColor[,] image;

    [Params(128, 512, 1024, 2024)]
    public int size;

    [Params(ColorRampMode.Linear, ColorRampMode.Constant)]
    public ColorRampMode mode;

    static ColorStop[] stops2;
    static ColorStop[] stops8;
    static ColorStop[] stops16;

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
        stops2 = [new(new(255, 255, 255), 0), new(new(0, 255, 0), 0.8f)];
        stops8 =
        [
            new(new(255, 255, 255), 0),
            new(new(0, 255, 0), 0.8f),
            new(new(54, 255, 54), 0.38f),
            new(new(5, 124, 45), 0.7374f),
            new(new(255, 255, 255), 0.5673f),
            new(new(0, 255, 0), 0.848f),
            new(new(54, 255, 54), 0.438f),
            new(new(5, 124, 45), 0.27374f),
        ];
        stops16 =
        [
            new(new(255, 255, 255), 0),
            new(new(0, 255, 0), 0.8f),
            new(new(54, 255, 54), 0.38f),
            new(new(5, 124, 45), 0.7374f),
            new(new(255, 255, 255), 0.5673f),
            new(new(0, 255, 0), 0.848f),
            new(new(54, 255, 54), 0.438f),
            new(new(5, 124, 45), 0.327374f),
            new(new(15, 184, 24), 0.105f),
            new(new(54, 4, 0), 0.48f),
            new(new(12, 12, 3), 0.638f),
            new(new(65, 74, 45), 0.73674f),
            new(new(255, 41, 255), 0.56573f),
            new(new(0, 255, 64), 0.8448f),
            new(new(31, 255, 54), 0.4638f),
            new(new(5, 3, 45), 0.627374f),
        ];
    }

    [Benchmark]
    public MyColor[,] ColorRamp2Stops()
    {
        return new ColorRampNode().ExecuteNode(
            new()
            {
                Image = image,
                Mode = mode,
                ColorStops = stops2,
            }
        );
    }

    [Benchmark]
    public MyColor[,] ColorRamp8Stops()
    {
        return new ColorRampNode().ExecuteNode(
            new()
            {
                Image = image,
                Mode = mode,
                ColorStops = stops8,
            }
        );
    }

    [Benchmark]
    public MyColor[,] ColorRamp16Stops()
    {
        return new ColorRampNode().ExecuteNode(
            new()
            {
                Image = image,
                Mode = mode,
                ColorStops = stops16,
            }
        );
    }
}
