using System;
using System.Drawing;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Benchmark;

public class NoiseTextureBench : Basebench
{
    public Bitmap? img;

    public NoiseTextureBench()
        : base("Noise Texture Generation") { }

    protected override void FuncToTest()
    {
        img = NoiseTextureNode.ApplyNoise(new(1024, 1024));
    }
}
