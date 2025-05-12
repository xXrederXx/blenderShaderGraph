using System.Drawing;
using System.Numerics;
using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.VectorNodes;

public enum NormalMapFormat
{
    OpenGL, // Y up
    DirectX // Y down
    ,
}

public class BumpProps(
    Input<float> heightMap,
    Input<float>? strength = null,
    Input<float>? distance = null,
    bool invert = false,
    NormalMapFormat format = NormalMapFormat.OpenGL
)
{
    public Input<float> HeightMap { get; } = heightMap;
    public Input<float>? Strength { get; } = strength;
    public Input<float>? Distance { get; } = distance;
    public bool Invert { get; } = invert;
    public NormalMapFormat Format { get; } = format;
}

public class BumpNode : Node<BumpProps, Input<MyColor>>
{
    readonly MyColor flatNormalColor = new(127, 127, 255);

    public BumpNode()
        : base() { }

    public BumpNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override BumpProps SafeProps(BumpProps props)
    {
        Input<float> dist;
        if (props.Distance is null)
        {
            System.Console.WriteLine("props.Distance is null, default to 1");
            dist = new(1);
        }
        else if (props.Distance.useArray)
        {
            float[,] dists = new float[props.Distance.Width, props.Distance.Height];
            for (int x = 0; x < props.Distance.Width; x++)
            {
                for (int y = 0; y < props.Distance.Height; y++)
                {
                    dists[x, y] = Math.Clamp(props.Distance[x, y], 0, 1000);
                }
            }
            dist = new(dists);
        }
        else
        {
            dist = new(Math.Clamp(props.Distance.Value, 0, 1000));
        }

        Input<float> strength;
        if (props.Strength is null)
        {
            System.Console.WriteLine("props.Strength is null, default to 1");
            strength = new(1);
        }
        else if (props.Strength.useArray)
        {
            float[,] dists = new float[props.Strength.Width, props.Strength.Height];
            for (int x = 0; x < props.Strength.Width; x++)
            {
                for (int y = 0; y < props.Strength.Height; y++)
                {
                    dists[x, y] = MyMath.Clamp01(props.Strength[x, y]);
                }
            }
            strength = new(dists);
        }
        else
        {
            strength = new(MyMath.Clamp01(props.Strength.Value));
        }

        return new(props.HeightMap, strength, dist, props.Invert, props.Format);
    }

    protected override Input<MyColor> ExecuteInternal(BumpProps props)
    {
        if (props.Strength is null || props.Distance is null)
        {
            throw new ArgumentNullException("props.Strength is null || props.Distance is null");
        }
        if(!props.HeightMap.useArray)
        {
            return new (flatNormalColor);
        }
        if(props.HeightMap.Array is null)
        {
            throw new ArgumentNullException("props.HeightMap.Array is null");
        }
        int width = props.HeightMap.Array.GetLength(0);
        int height = props.HeightMap.Array.GetLength(1);

        Bitmap normalMap = new(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        MyColor[,] newColors = new MyColor[width, height];
        float[,] oldColors = props.HeightMap.Array;

        Parallel.For(
            1,
            height - 1,
            (y) =>
            {
                for (int x = 1; x < width - 1; x++)
                {
                    float hL = oldColors[x - 1, y];
                    float hR = oldColors[x + 1, y];
                    float hD = oldColors[x, y - 1];
                    float hU = oldColors[x, y + 1];

                    float dx = (hR - hL) * props.Strength[x, y];
                    float dy = (hU - hD) * props.Strength[x, y];

                    if (dx == 0 && dy == 0)
                    {
                        newColors[x, y] = flatNormalColor;
                        continue;
                    }
                    if (props.Invert)
                    {
                        dx = -dx;
                        dy = -dy;
                    }

                    Vector3 normal = new Vector3(-dx, -dy, 1.0f / props.Distance[x, y]);
                    normal = Vector3.Normalize(normal);

                    // from -1 - 1 to 0 - 1
                    float nx = normal.X * 0.5f + 0.5f;
                    float ny = normal.Y * 0.5f + 0.5f;
                    float nz = normal.Z * 0.5f + 0.5f;

                    // Flip G (Y) channel for DirectX
                    if (props.Format == NormalMapFormat.DirectX)
                    {
                        ny = 1.0f - ny;
                    }

                    MyColor normalColor = new MyColor(
                        (byte)Math.Clamp(nx * 255, 0, 255),
                        (byte)Math.Clamp(ny * 255, 0, 255),
                        (byte)Math.Clamp(nz * 255, 0, 255)
                    );

                    newColors[x, y] = normalColor;
                }
            }
        );
        return new Input<MyColor>(newColors);
    }

    protected override BumpProps ConvertJSONToProps(Dictionary<string, Input> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new(
            p.GetInputFloat(Id, contex, "heightMap"),
            strength: p.GetInputFloat(Id, contex, "strength"),
            distance: p.GetInputFloat(Id, contex, "distance"),
            invert: p.GetBool("invert"),
            format: p.GetBool("isDX") == true ? NormalMapFormat.DirectX : NormalMapFormat.OpenGL
        );
    }

    protected override void AddDataToContext(Input<MyColor> data, Dictionary<string, Input> contex)
    {
        contex[Id] = data;
    }
}
