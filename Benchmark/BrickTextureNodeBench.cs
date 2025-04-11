using System;
using System.Drawing;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Benchmark;

public class BrickTextureNodeBench : Basebench
{
    public Bitmap? img;

    public BrickTextureNodeBench() : base("Brick Texture Generation")
    {
    }

    protected override void FuncToTest()
    {
        img = BrickTextureNode
            .GenerateTexture(
                new(
                    1000,
                    1000,
                    0.5f,
                    2,
                    1.5f,
                    3,
                    Color.Blue,
                    Color.DarkCyan,
                    Color.Orange,
                    10,
                    1f,
                    0.0f,
                    200f,
                    40
                )
            )
            .color;
    }
}
