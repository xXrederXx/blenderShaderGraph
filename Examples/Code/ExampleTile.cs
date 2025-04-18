using System.Drawing;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Nodes.ConverterNodes;
using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Nodes.VectorNodes;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Examples;

public static class ExampleTile
{
    public static void Execute()
    {
        Bitmap Diffuse;
        Bitmap Normal;
        Bitmap Roughness;

        (Bitmap color, Bitmap fac) brick = BrickTextureNode.Generate(
            new(
                offset: 0,
                squash: 1,
                color1: Color.White,
                color2: Color.Black,
                colorMotar: Color.White,
                motarSize: 20,
                motarSmoothness: 0f,
                brickWidth: 100,
                rowHeight: 100
            )
        );
        Roughness = CalcR(brick);
        Normal = CalcN(brick);
        Diffuse = CalcD(brick);

        Roughness.Save("r.png");
        Normal.Save("n.png");
        Diffuse.Save("d.png");
    }

    private static Bitmap CalcD((Bitmap color, Bitmap fac) brick)
    {
        Bitmap brickNoise = NoiseTextureNode.Generate(new(Scale: 10));
        ColorRampNode.Apply(
            brickNoise,
            [new(Color.Black, 0), new(Color.FromArgb(113, 113, 113), 1)]
        );
        Bitmap brickMixed = MixColorNode.Generate(
            brick.color,
            brickNoise,
            0.442f,
            MixColorMode.Value
        );
        ColorRampNode.Apply(
            brickMixed,
            [
                new(
                    Color.FromArgb(
                        (byte)Math.Clamp(0.1 * 255, 0, 255),
                        (byte)Math.Clamp(0.24 * 255, 0, 255),
                        (byte)Math.Clamp(0.2 * 255, 0, 255)
                    ),
                    0
                ),
                new(
                    Color.FromArgb(
                        (byte)Math.Clamp(0.13 * 255, 0, 255),
                        (byte)Math.Clamp(0.3 * 255, 0, 255),
                        (byte)Math.Clamp(0.35 * 255, 0, 255)
                    ),
                    0.313f
                ),
                new(
                    Color.FromArgb(
                        (byte)Math.Clamp(0.035 * 255, 0, 255),
                        (byte)Math.Clamp(0.031 * 255, 0, 255),
                        (byte)Math.Clamp(0.175 * 255, 0, 255)
                    ),
                    0.476f
                ),
                new(
                    Color.FromArgb(
                        (byte)Math.Clamp(0.27 * 255, 0, 255),
                        (byte)Math.Clamp(0.22 * 255, 0, 255),
                        (byte)Math.Clamp(0.41 * 255, 0, 255)
                    ),
                    1
                ),
            ]
        );

        Bitmap lineNoise = NoiseTextureNode.Generate(
            new(Scale: 14.6f, Detail: 5.9f, Roughness: 0.767f)
        );
        ColorRampNode.Apply(
            lineNoise,
            [
                new(
                    Color.Black,
                    0
                ),
                new(
                    Color.FromArgb(
                        (byte)Math.Clamp(0.242 * 255, 0, 255),
                        (byte)Math.Clamp(0.242 * 255, 0, 255),
                        (byte)Math.Clamp(0.242 * 255, 0, 255)
                    ),
                    1
                )
            ]
        );

        return MixColorNode.Generate(brickMixed, lineNoise, brick.fac);
    }

    private static Bitmap CalcN((Bitmap color, Bitmap fac) brick)
    {
        Bitmap noise = NoiseTextureNode.Generate(new(Scale: 5, Detail: 8, Roughness: 1));
        Bitmap mixed = MixColorNode.Generate(
            noise,
            BitmapUtil.FilledBitmap(1024, 1024, Color.White),
            brick.fac
        );
        return BumpNode.GenerateBump(new(mixed, strength: 0.3f));
    }

    private static Bitmap CalcR((Bitmap color, Bitmap fac) brick)
    {
        Bitmap Roughness = new(brick.fac);
        ColorRampNode.Apply(
            Roughness,
            [new(Color.White, 0.01f), new(Color.Black, 0f)],
            ColorRampMode.Constant
        );
        return Roughness;
    }
}
