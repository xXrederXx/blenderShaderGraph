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
        Bitmap bitmap = new(props.imgWidth, props.imgHeight);

        noiseGen.SetSeed(DateTime.Now.Millisecond);

        // Apply detail and roughness settings
        noiseGen.SetFractalOctaves((int)Math.Clamp(props.detail, 1, 8));
        noiseGen.SetFractalGain(Math.Clamp(props.roughness, 0f, 1f));

        // Apply noise based on scaled coordinates
        bitmap.ForPixelParralel(
            (x, y) =>
            {
                float nx = x * props.size;
                float ny = y * props.size;
                float value = noiseGen.GetNoise(nx, ny);
                return ColorUtil.ColorFromValue(value, true);
            }
        );

        return bitmap;
    }
}
