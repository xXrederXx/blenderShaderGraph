using System.Runtime.CompilerServices;

namespace blenderShaderGraph.Util;

public static class MyMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        if (value < inMin)
            return outMin;
        if (value > inMax)
            return outMax;
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp01(float val) => Math.Clamp(val, 0, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampByte(float value) => (byte)Math.Clamp(value, 0, 255);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static float SinFast(float x) //x in radians
    {
        float sinn;
        if (x < -3.14159265f)
            x += 6.28318531f;
        else if (x > 3.14159265f)
            x -= 6.28318531f;

        if (x < 0)
        {
            sinn = 1.27323954f * x + 0.405284735f * x * x;

            if (sinn < 0)
                sinn = 0.225f * (sinn * -sinn - sinn) + sinn;
            else
                sinn = 0.225f * (sinn * sinn - sinn) + sinn;
            return sinn;
        }
        else
        {
            sinn = 1.27323954f * x - 0.405284735f * x * x;

            if (sinn < 0)
                sinn = 0.225f * (sinn * -sinn - sinn) + sinn;
            else
                sinn = 0.225f * (sinn * sinn - sinn) + sinn;
            return sinn;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static float CosFast(float x) //x in radians
    {
        return SinFast(x + 1.5707963f);
    }
}
