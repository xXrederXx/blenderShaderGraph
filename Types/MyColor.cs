using System.Drawing;

namespace blenderShaderGraph.Types;

public readonly struct MyColor : IEquatable<MyColor>
{
    public readonly uint rgba;

    public byte R => unchecked((byte)rgba); // R: bits 0–7
    public byte G => unchecked((byte)(rgba >> 8)); // G: bits 8–15
    public byte B => unchecked((byte)(rgba >> 16)); // B: bits 16–23
    public byte A => unchecked((byte)(rgba >> 24)); // A: bits 24–31
    public Color Color => Color.FromArgb(A, R, G, B);

    public MyColor(byte r, byte g, byte b, byte a = 255)
    {
        rgba = ((uint)a << 24) | ((uint)b << 16) | ((uint)g << 8) | r;
    }

    public MyColor(byte rgb, byte a = 255)
    {
        rgba = ((uint)a << 24) | ((uint)rgb << 16) | ((uint)rgb << 8) | rgb;
    }

    public bool Equals(MyColor other) => rgba == other.rgba;

    // --- HSV Conversion Helpers ---

    public (float H, float S, float V) GetHSV() => RGBToHSV();

    public float GetHSVOnlyH() => Color.GetHue();

    public float GetHSVOnlyS() => Color.GetSaturation();

    public float GetHSVOnlyV() => Color.GetBrightness();

    // Standard luminance formula, normalized to 0-1
    public float GetGrayscale() => (0.299f * R + 0.587f * G + 0.114f * B) / 255f;

    public static MyColor FromHSV(float h, float s, float v, byte a = 255)
    {
        (byte r, byte g, byte b) = HSVToRGB(h, s, v);
        return new MyColor(r, g, b, a);
    }

    // --- Utility Methods ---

    private (float H, float S, float V) RGBToHSV()
    {
        return (Color.GetHue(), Color.GetSaturation(), Color.GetBrightness());
    }

    private static (byte R, byte G, byte B) HSVToRGB(float h, float s, float v)
    {
        float c = v * s;
        float x = c * (1f - Math.Abs((h / 60f % 2f) - 1f));
        float m = v - c;

        float r = 0f,
            g = 0f,
            b = 0f;

        if (h < 60f)
        {
            (r, g, b) = (c, x, 0);
        }
        else if (h < 120f)
        {
            (r, g, b) = (x, c, 0);
        }
        else if (h < 180f)
        {
            (r, g, b) = (0, c, x);
        }
        else if (h < 240f)
        {
            (r, g, b) = (0, x, c);
        }
        else if (h < 300f)
        {
            (r, g, b) = (x, 0, c);
        }
        else
        {
            (r, g, b) = (c, 0, x);
        }

        byte R = (byte)Math.Round((r + m) * 255f);
        byte G = (byte)Math.Round((g + m) * 255f);
        byte B = (byte)Math.Round((b + m) * 255f);

        return (R, G, B);
    }

    public override int GetHashCode()
    {
        return rgba.GetHashCode();
    }

    public override string ToString()
    {
        return $"Color [R: {R} G: {G} B: {B} A: {A}]";
    }

    public static implicit operator Color(MyColor myColor)
    {
        return myColor.Color;
    }

    public static implicit operator MyColor(Color color)
    {
        return new(color.R, color.G, color.B, color.A);
    }
}
