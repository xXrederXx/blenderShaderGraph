using System.Text.Json;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public record NoiseTextureProps
{
    public int ImgWidth { get; set; } = 1024;
    public int ImgHeight { get; set; } = 1024;
    public float Scale { get; set; } = 1;
    public float Detail { get; set; } = 2;
    public float Roughness { get; set; } = 0.5f;
    public float Lacunarity { get; set; } = 2;
};

public class NoiseTextureNode : Node<NoiseTextureProps, float[,]>
{
    static readonly FastNoise noiseGen = new();

    static NoiseTextureNode()
    {
        noiseGen.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
        noiseGen.SetFractalType(FastNoise.FractalType.FBM);
    }

    public NoiseTextureNode()
        : base() { }

    public NoiseTextureNode(string Id, JsonElement element)
        : base(Id, element) { }

    protected override NoiseTextureProps SafeProps(NoiseTextureProps props)
    {
        props.Detail = Math.Clamp(props.Detail, 0, 8);
        props.Roughness = Math.Clamp(props.Roughness, 0, 8);
        return props;
    }

    protected override float[,] ExecuteInternal(NoiseTextureProps props)
    {
        float[,] values = new float[props.ImgWidth, props.ImgHeight];

        noiseGen.SetSeed(DateTime.Now.Millisecond);

        // Apply detail and roughness settings
        noiseGen.SetFractalOctaves((int)Math.Clamp(props.Detail, 1, 8));
        noiseGen.SetFractalGain(Math.Clamp(props.Roughness, 0f, 1f));
        noiseGen.SetFractalLacunarity(props.Lacunarity);

        int height = props.ImgHeight;
        Parallel.For(
            0,
            props.ImgWidth,
            (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    float nx = x * props.Scale;
                    float ny = y * props.Scale;
                    float value = noiseGen.GetNoise(nx, ny);
                    values[x, y] = (Math.Clamp(value, -1f, 1f) + 1f) * 0.5f;
                }
            }
        );
        return values;
    }

    protected override NoiseTextureProps ConvertJSONToProps(Dictionary<string, object> contex)
    {
        JsonElement p = element.GetProperty("params");
        return new()
        {
            ImgWidth = p.GetInt("width", 1024),
            ImgHeight = p.GetInt("height", 1024),
            Scale = p.GetFloat("size", 1),
            Detail = p.GetFloat("detail", 2),
            Roughness = p.GetFloat("roughness", 0.5f),
            Lacunarity = p.GetFloat("lacunarity", 2f),
        };
    }

    protected override void AddDataToContext(float[,] data, Dictionary<string, object> contex)
    {
        contex[Id] = data;
    }
}
