using BenchmarkDotNet.Attributes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Benchmarks.Util;

[ShortRunJob]
[MemoryDiagnoser]
public class MyMathBench
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public float[] floats;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [Params(512*512, 1024*1024, 2024*2024)]
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
