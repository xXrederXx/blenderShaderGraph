using System;

namespace blenderShaderGraph.Util;

public static class MyMath
{
    public static float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    public static float Clapm01(float val) => Math.Clamp(val, 0, 1);
}
