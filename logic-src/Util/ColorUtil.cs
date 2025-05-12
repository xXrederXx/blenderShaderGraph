using blenderShaderGraph.Types;

namespace blenderShaderGraph.Util;

public static class ColorUtil
{
    public static MyColor ColorFromValue(float value, bool includeNegativ)
    {
        byte v = includeNegativ
            ? (byte)((Math.Clamp(value, -1f, 1f) + 1f) / 2f * 255)
            : (byte)(Math.Clamp(value, 0, 1f) * 255);
        return new MyColor(v, v, v);
    }

    public static float ValueFromColor(MyColor color)
    {
        return (color.R + color.G + color.B) * 0.00130718954248366f; // DIVIDED BY 3 AND 255
    }

    public static MyColor LerpColor(MyColor aCol, MyColor bCol, float t)
    {
        return new MyColor(
            (byte)MyMath.Lerp(aCol.R, bCol.R, t),
            (byte)MyMath.Lerp(aCol.G, bCol.G, t),
            (byte)MyMath.Lerp(aCol.B, bCol.B, t)
        );
    }
}
