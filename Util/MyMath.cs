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

    public static float Clamp01(float val) => Math.Clamp(val, 0, 1);
    public static float SmoothStep(float edge0, float edge1, float x)
    {
        x = Math.Clamp((x - edge0) / (edge1 - edge0), 0f, 1f);
        return x * x * (3 - 2 * x);
    }

}
