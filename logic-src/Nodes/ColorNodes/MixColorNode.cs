using System.Buffers;
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

public class MixColorProps
{
    public MixColorProps() { }

    public MixColorProps(
        Input<MyColor>? a,
        Input<MyColor>? b,
        Input<float>? factor,
        MixColorMode Mode
    )
    {
        this.ImageA = a;
        this.ImageB = b;
        this.Factor = factor;
        this.Mode = Mode;
    }

    public Input<MyColor>? ImageA { get; set; }
    public Input<MyColor>? ImageB { get; set; }
    public Input<float>? Factor { get; set; }
    public MixColorMode Mode { get; set; } = MixColorMode.Mix;
}

public class MixColorNode : Node<MixColorProps, Input<MyColor>>
{
    public MixColorNode()
        : base() { }

    public MixColorNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override MixColorProps SafeProps(MixColorProps props)
    {
        if (props.ImageA is null)
            System.Console.WriteLine("a props is null");
        if (props.ImageB is null)
            System.Console.WriteLine("b props is null");

        return props;
    }

    protected override Input<MyColor> ExecuteInternal(MixColorProps props)
    {
        if (props.ImageA is null)
        {
            props.ImageA = InputDefaults.colorBlackInput;
        }
        if (props.ImageB is null)
        {
            props.ImageB = InputDefaults.colorBlackInput;
        }
        if (props.Factor is null)
        {
            props.Factor = InputDefaults.floatInput;
        }

        int width = Math.Min(props.ImageA.Width, props.ImageB.Width);
        int height = Math.Min(props.ImageA.Height, props.ImageB.Height);

        return GenerateInternal(
            props.ImageA,
            props.ImageB,
            props.Factor,
            props.Mode,
            width,
            height
        );
    }

    protected override MixColorProps ConvertJSONToProps(Dictionary<string, Input> contex)
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
            ImageA = p.GetInputMyColor(Id, contex, "a"),
            ImageB = p.GetInputMyColor(Id, contex, "b"),
            Factor = p.GetInputFloat(Id, contex, "factor"),
            Mode = mode,
        };
    }

    protected override void AddDataToContext(Input<MyColor> data, Dictionary<string, Input> contex)
    {
        contex[Id] = data;
    }

    // NODE SPESIFIC
    private static Input<MyColor> GenerateInternal(
        Input<MyColor> a,
        Input<MyColor> b,
        Input<float> factor,
        MixColorMode mode,
        int width,
        int height
    )
    {
        Func<MyColor, MyColor, float, MyColor> blendFunc = mode switch
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
        if (!a.useArray && !b.useArray && !factor.useArray)
        {
            return new Input<MyColor>(blendFunc(a.Value, b.Value, factor.Value));
        }
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

        return new Input<MyColor>(newColors);
    }

    private static MyColor MixBlend(MyColor a, MyColor b, float fac)
    {
        return new MyColor(
            MyMath.ClampByte(a.R * (1 - fac) + b.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + b.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + b.B * fac)
        );
    }

    private static MyColor HueBlend(MyColor a, MyColor b, float fac)
    {
        (float ah, float asat, float av) = a.GetHSV();
        float bh = b.GetHSVOnlyH();
        float nh = ah * (1 - fac) + bh * fac;
        return MyColor.FromHSV(nh, asat, av);
    }

    private static MyColor SaturationBlend(MyColor a, MyColor b, float fac)
    {
        (float ah, float asat, float av) = a.GetHSV();
        float bsat = b.GetHSVOnlyS();
        float ns = asat * (1 - fac) + bsat * fac;
        return MyColor.FromHSV(ah, ns, av);
    }

    private static MyColor ValueBlend(MyColor a, MyColor b, float fac)
    {
        (float ah, float asat, float av) = a.GetHSV();
        float bv = b.GetHSVOnlyV();
        float nv = av * (1 - fac) + bv * fac;
        return MyColor.FromHSV(ah, asat, nv);
    }

    private static MyColor DarkenBlend(MyColor a, MyColor b, float fac)
    {
        MyColor c = new MyColor(Math.Min(a.R, b.R), Math.Min(a.G, b.G), Math.Min(a.B, b.B));
        return new MyColor(
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static MyColor LightenBlend(MyColor a, MyColor b, float fac)
    {
        MyColor c = a.GetGrayscale() > b.GetGrayscale() ? a : b;
        return new MyColor(
            MyMath.ClampByte(a.R * (1 - fac) + c.R * fac),
            MyMath.ClampByte(a.G * (1 - fac) + c.G * fac),
            MyMath.ClampByte(a.B * (1 - fac) + c.B * fac)
        );
    }

    private static MyColor LinearLightBlend(MyColor a, MyColor b, float fac)
    {
        byte Blend(byte ac, byte bc)
        {
            float af = ac / 255f,
                bf = bc / 255f;
            float result = MyMath.Clamp01(af + 2f * bf - 1f);
            return MyMath.ClampByte((af * (1f - fac) + result * fac) * 255f);
        }

        return new MyColor(Blend(a.R, b.R), Blend(a.G, b.G), Blend(a.B, b.B));
    }
}
