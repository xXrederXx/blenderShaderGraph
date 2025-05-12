using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks.Util;

/*
| Method       | size    | Mean        | Error       | StdDev    | Allocated |
|------------- |-------- |------------:|------------:|----------:|----------:|
| MyLerp       | 262144  |    175.0 us |    61.26 us |   3.36 us |         - |
| MyMap        | 262144  |  1,198.1 us |   308.36 us |  16.90 us |         - |
| MyClamp01    | 262144  |    176.5 us |    69.46 us |   3.81 us |         - |
| MyClampByte  | 262144  |    423.9 us |    76.98 us |   4.22 us |         - |
| MySinFast    | 262144  |    293.7 us |   172.86 us |   9.47 us |         - |
| MyCosFast    | 262144  |    470.4 us |   214.94 us |  11.78 us |         - |
| SysClamp01   | 262144  |    174.1 us |    18.43 us |   1.01 us |         - |
| SysClampByte | 262144  |    442.2 us |   362.33 us |  19.86 us |         - |
| SysSine      | 262144  |  1,187.7 us |    42.62 us |   2.34 us |         - |
| SysCosFast   | 262144  |  1,151.7 us |   183.13 us |  10.04 us |         - |
| MyLerp       | 1048576 |    720.2 us |   362.63 us |  19.88 us |         - |
| MyMap        | 1048576 |  4,936.3 us | 2,888.27 us | 158.32 us |       1 B |
| MyClamp01    | 1048576 |    712.0 us |   799.68 us |  43.83 us |         - |
| MyClampByte  | 1048576 |  1,678.3 us |   140.88 us |   7.72 us |       1 B |
| MySinFast    | 1048576 |  1,143.6 us |   126.24 us |   6.92 us |       1 B |
| MyCosFast    | 1048576 |  1,827.1 us |   101.85 us |   5.58 us |         - |
| SysClamp01   | 1048576 |    681.3 us |     7.46 us |   0.41 us |         - |
| SysClampByte | 1048576 |  1,678.7 us |   197.71 us |  10.84 us |       1 B |
| SysSine      | 1048576 |  4,754.6 us |   232.43 us |  12.74 us |       3 B |
| SysCosFast   | 1048576 |  4,569.0 us |   118.43 us |   6.49 us |       3 B |
| MyLerp       | 4096576 |  2,679.3 us |   124.31 us |   6.81 us |         - |
| MyMap        | 4096576 | 18,672.0 us |   668.50 us |  36.64 us |       4 B |
| MyClamp01    | 4096576 |  2,671.9 us |    33.88 us |   1.86 us |         - |
| MyClampByte  | 4096576 |  6,555.5 us |   416.59 us |  22.83 us |       3 B |
| MySinFast    | 4096576 |  4,482.7 us |   486.28 us |  26.65 us |       1 B |
| MyCosFast    | 4096576 |  7,156.8 us |   272.25 us |  14.92 us |       1 B |
| SysClamp01   | 4096576 |  2,674.9 us |    63.32 us |   3.47 us |       2 B |
| SysClampByte | 4096576 |  6,517.2 us |   110.88 us |   6.08 us |       1 B |
| SysSine      | 4096576 | 18,556.1 us |   722.99 us |  39.63 us |       4 B |
| SysCos       | 4096576 | 17,926.0 us | 1,295.55 us |  71.01 us |       4 B |
 */

[ShortRunJob]
[MemoryDiagnoser]
public class MyMathBench
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public float[] floats;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [Params(512 * 512, 1024 * 1024, 2024 * 2024)]
    public int size;

    [GlobalSetup]
    public void setup()
    {
        Random rng = new Random(3574);
        floats = new float[size];
        for (int i = 0; i < size; i++)
        {
            floats[i] = (float)rng.NextDouble();
        }
    }

    [Benchmark]
    public float MyLerp()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.Lerp(floats[i], floats[i + 1], floats[i + 2]);
        }
        return x;
    }

    [Benchmark]
    public float MyMap()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.Map(floats[i], floats[i + 1], floats[i + 2], floats[i + 3], floats[i + 4]);
        }
        return x;
    }

    [Benchmark]
    public float MyClamp01()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.Clamp01(floats[i]);
        }
        return x;
    }

    [Benchmark]
    public float MyClampByte()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.ClampByte(floats[i]);
        }
        return x;
    }

    [Benchmark]
    public float MySinFast()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.SinFast(floats[i]);
        }
        return x;
    }

    [Benchmark]
    public float MyCosFast()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MyMath.CosFast(floats[i]);
        }
        return x;
    }

    // SYSTEM

    public float SysLerp()
    {
        // No implementation
        return 0;
    }

    public float SysMap()
    {
        // No implementation
        return 0;
    }

    [Benchmark]
    public float SysClamp01()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += Math.Clamp(floats[i], 0, 1);
        }
        return x;
    }

    [Benchmark]
    public float SysClampByte()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += (byte)Math.Clamp(floats[i], 0, 255);
        }
        return x;
    }

    [Benchmark]
    public float SysSine()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MathF.Sin(floats[i]);
        }
        return x;
    }

    [Benchmark]
    public float SysCosFast()
    {
        float x = 0;
        for (int i = 0; i < size - 10; i++)
        {
            x += MathF.Cos(floats[i]);
        }
        return x;
    }
}
