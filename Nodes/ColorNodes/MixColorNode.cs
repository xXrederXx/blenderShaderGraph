using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.ColorNodes;

public enum MixColorMode
{
    Mix,
    Hue,
    Saturation,
    Value,
    Darken,
    LinearLight,
    Lighten,
}

public record MixColorProps
{
    public Input<MyColor>? a { get; set; }
    public Input<MyColor>? b { get; set; }
    public Input<float>? factor { get; set; }
    public MixColorMode Mode { get; set; } = MixColorMode.Mix;
}

public class MixColorNode : Node<MixColorProps, MyColor[,]>
{
    public MixColorNode()
        : base() { }

    public MixColorNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override MixColorProps SafeProps(MixColorProps props)
    {
        if (props.a is null)
            System.Console.WriteLine("a props is null");
        if (props.b is null)
            System.Console.WriteLine("b props is null");

        return props;
    }

    protected override MyColor[,] ExecuteInternal(MixColorProps props)
    {
        if (props.a is null)
        {
            props.a = new Input<MyColor>(new MyColor(0, 0, 0));
        }
        if (props.b is null)
        {
            props.b = new Input<MyColor>(new MyColor(0, 0, 0));
        }
        if (props.factor is null)
        {
            props.factor = new(0);
        }

        int widthA = props.a.Width;
        int heightA = props.a.Height;
        int widthB = props.b.Width;
        int heightB = props.b.Height;

        int width = Math.Min(widthA, widthB);
        int height = Math.Min(heightA, heightB);

        return GenerateInternal(props.a, props.b, props.factor, props.Mode, width, height);
    }

    protected override MixColorProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");

        string modeStr = p.GetString("mode", "mix");
        MixColorMode mode = modeStr switch
        {
            "mix" => MixColorMode.Mix,
            "hue" => MixColorMode.Hue,
            "saturation" => MixColorMode.Saturation,
            "value" => MixColorMode.Value,
            "darken" => MixColorMode.Darken,
            "lighten" => MixColorMode.Lighten,
            "linearlight" => MixColorMode.LinearLight,
            _ => MixColorMode.Mix,
        };
        return new MixColorProps()
        {
            a = p.GetMyColor2D(Id, contex, "a"),
            b = p.GetMyColor2D(Id, contex, "b"),
            factor = p.GetInputFloat(Id, contex, "factor"),
            Mode = mode,
        };
    }

    protected override void AddDataToContext(MyColor[,] data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
    private static MyColor[,] GenerateInternal(
        Input<MyColor> a,
        Input<MyColor> b,
        Input<float> factor,
        MixColorMode mode,
        int width,
        int height
    )
    {
        Func<Color, Color, float, Color> blendFunc = mode switch
        {
            MixColorMode.Mix => MixBlend,
            MixColorMode.Hue => HueBlend,
            MixColorMode.Saturation => SaturationBlend,
            MixColorMode.Value => ValueBlend,
            MixColorMode.Darken => DarkenBlend,
            MixColorMode.LinearLight => LinearLightBlend,
            MixColorMode.Lighten => LightenBlend,
            _ => (ac, bc, f) => ac,
        };

        MyColor[,] newColors = new MyColor[width, height];
        Parallel.For(
            0,
            width,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    newColors[x, y] = blendFunc(a[x, y], b[x, y], factor[x, y]);
                }
            }
        );

        return newColors;
    }

    private static Color MixBlend(Color a, Color b, float fac)
    {
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + b.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + b.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + b.B * fac)
        );
    }

    private static Color HueBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyV(b, out double bh);
        double nh = ah * (1 - fac) + bh * fac;
        return ColorUtil.ColorFromHSV(nh, asat, av);
    }

    private static Color SaturationBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyS(b, out double bsat);
        double ns = asat * (1 - fac) + bsat * fac;
        return ColorUtil.ColorFromHSV(ah, ns, av);
    }

    private static Color ValueBlend(Color a, Color b, float fac)
    {
        ColorUtil.ColorToHSV(a, out double ah, out double asat, out double av);
        ColorUtil.ColorToHSVOnlyV(b, out double bv);
        double nv = av * (1 - fac) + bv * fac;
        return ColorUtil.ColorFromHSV(ah, asat, nv);
    }

    private static Color DarkenBlend(Color a, Color b, float fac)
    {
        var c = Color.FromArgb(255, Math.Min(a.R, b.R), Math.Min(a.G, b.G), Math.Min(a.B, b.B));
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static Color LightenBlend(Color a, Color b, float fac)
    {
        var c = a.GetBrightness() > b.GetBrightness() ? a : b;
        return Color.FromArgb(
            255,
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static Color LinearLightBlend(Color a, Color b, float fac)
    {
        byte Blend(byte ac, byte bc)
        {
            float af = ac / 255f,
                bf = bc / 255f;
            float result = MyMath.Clamp01(af + 2f * bf - 1f);
            return MyMath.ClampByte((af * (1f - fac) + result * fac) * 255f);
        }

        return Color.FromArgb(255, Blend(a.R, b.R), Blend(a.G, b.G), Blend(a.B, b.B));
    }
}
