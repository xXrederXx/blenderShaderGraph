using System.Drawing;

public readonly struct MyColorF : IEquatable<MyColorF>
{
    public readonly uint rgba;

    public byte r => unchecked((byte)rgba); // R: bits 0–7
    public byte g => unchecked((byte)(rgba >> 8)); // G: bits 8–15
    public byte b => unchecked((byte)(rgba >> 16)); // B: bits 16–23
    public byte a => unchecked((byte)(rgba >> 24)); // A: bits 24–31
    public Color color => Color.FromArgb(a, r, g, b);

    public MyColorF(byte r, byte g, byte b, byte a = 255)
    {
        rgba = ((uint)a << 24) | ((uint)b << 16) | ((uint)g << 8) | r;
    }

    public bool Equals(MyColorF other) => rgba == other.rgba;

    // --- HSV Conversion Helpers ---

    public (float H, float S, float V) GetHSV() => RGBToHSV();

    public float GetHSVOnlyH() => color.GetHue();

    public float GetHSVOnlyS() => color.GetSaturation();

    public float GetHSVOnlyV() => color.GetBrightness();

    // Standard luminance formula, normalized to 0-1
    public float GetGrayscale() => (0.299f * r + 0.587f * g + 0.114f * b) / 255f;

    public static MyColorF FromHSV(float h, float s, float v, byte a = 255)
    {
        (byte r, byte g, byte b) = HSVToRGB(h, s, v);
        return new MyColorF(r, g, b, a);
    }

    // --- Utility Methods ---

    private (float H, float S, float V) RGBToHSV()
    {
        return (color.GetHue(), color.GetSaturation(), color.GetBrightness());
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
        return $"Color [R: {r} G: {g} B: {b} A: {a}]";
    }
    public static implicit operator Color(MyColorF myColor)
    {
        return myColor.color;
    }
    public static implicit operator MyColorF(Color color)
    {
        return new(color.R, color.G, color.B, color.A);
    }
}
