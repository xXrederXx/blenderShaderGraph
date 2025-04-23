using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Benchmarks;

/*
| Method      | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MassivBench | 15.04 ms | 0.178 ms | 0.244 ms | 859.3750 | 859.3750 | 859.3750 |  28.04 MB |

| Method      | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MassivBench | 16.18 ms | 0.632 ms | 0.844 ms | 859.3750 | 859.3750 | 859.3750 |  28.04 MB |

| Method      | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------ |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| MassivBench | 15.67 ms | 0.176 ms | 0.247 ms | 843.7500 | 843.7500 | 843.7500 |  28.04 MB |

| Method      | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|------------ |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| MassivBench | 14.16 ms | 0.146 ms | 0.214 ms | 1859.3750 | 1859.3750 | 1859.3750 |  24.08 MB |
 */

[MediumRunJob, MemoryDiagnoser]
public class TmpBench
{
    [Benchmark]
    public (Input<MyColor>, Input<MyColor>) MassivBench()
    {
        float[,] fac = NodeInstances.brickTexture.ExecuteNode(new()).fac;
        Input<MyColor> obj = NodeInstances.textureCoordinate.ExecuteNode(new());
        Input<MyColor> mix = NodeInstances.mixColor.ExecuteNode(
            new()
            {
                a = new(new Types.MyColor(20, 46, 145)),
                b = obj,
                factor = new(fac),
            }
        );
        Input<float> noise = NodeInstances.noiseTexture.ExecuteNode(new());
        Input<MyColor> ramp = NodeInstances.colorRamp.ExecuteNode(
            new()
            {
                Image = noise,
                ColorStops = [new(new(0, 0, 0), 0.3f), new(new(255, 255, 255), 0.7f)],
            }
        );
        Input<MyColor> bump = NodeInstances.bump.ExecuteNode(
            new(Converter.ConvertToFloat(ramp.Array ?? new MyColor[0, 0]), new(1), new(1))
        );
        return (bump, mix);
    }
}
