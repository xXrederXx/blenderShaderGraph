using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.TextureNodes;

public static class NoiseTextureNode
{
    static readonly FastNoise noiseGen = new();

    static NoiseTextureNode()
    {
        noiseGen.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
        noiseGen.SetFractalType(FastNoise.FractalType.FBM);
        noiseGen.SetFractalLacunarity(2);
    }

    public static Bitmap Generate(NoiseTextureProps props)
    {
        Bitmap bitmap = new(props.ImgWidth, props.ImgHeight);

        noiseGen.SetSeed(DateTime.Now.Millisecond);

        // Apply detail and roughness settings
        noiseGen.SetFractalOctaves((int)Math.Clamp(props.Detail, 1, 8));
        noiseGen.SetFractalGain(Math.Clamp(props.Roughness, 0f, 1f));

        // Apply noise based on scaled coordinates
        bitmap.ForPixelParralel(
            (x, y) =>
            {
                float nx = x * props.Scale;
                float ny = y * props.Scale;
                float value = noiseGen.GetNoise(nx, ny);
                return ColorUtil.ColorFromValue(value, true);
            }
        );

        return bitmap;
    }
}
