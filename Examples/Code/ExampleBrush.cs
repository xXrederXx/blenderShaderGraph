using System;
using System.Drawing;
using System.Linq;
using blenderShaderGraph.Nodes.ColorNodes;
using blenderShaderGraph.Nodes.TextureNodes;

namespace blenderShaderGraph.Examples.Code;

public class ExampleBrush
{
    public static void Generate() { }

    private static void MassiveV1()
    {
        int[] dotsM1 = [10, 30, 50];
        int[] dotsM2 = [1, 5, 10];
        int[] maxM1 = [50, 100];
        int[] minM1 = [10, 20];
        int[] minM2 = [150, 250];
        float[] facMix1 = [0.4f, 0.5f, 0.6f];
        float[] facMix2 = [0.05f, 0.15f, 0.25f];
        int i = 0;
        foreach (
            var (ndm1, ndm2, mxm1, mnm1, mnm2, fm1, fm2) in from ndm1 in dotsM1
            from ndm2 in dotsM2
            from mxm1 in maxM1
            from mnm1 in minM1
            from mnm2 in minM2
            from fm1 in facMix1
            from fm2 in facMix2
            select (ndm1, ndm2, mxm1, mnm1, mnm2, fm1, fm2)
        )
        {
            GenerateV1(
                1024,
                "Mask" + i.ToString("D3"),
                ndm1,
                ndm2,
                mxm1,
                mnm1,
                400,
                mnm2,
                fm1,
                fm2
            );
            System.Console.WriteLine("Finished " + i);
            i++;
        }
    }

    private static void GenerateV1(
        int imgSize,
        string filename,
        int numDotsM1 = 30,
        int numDotsM2 = 5,
        int maxM1 = 100,
        int minM1 = 20,
        int maxM2 = 500,
        int minM2 = 200,
        float facMix1 = 0.5f,
        float facMix2 = 0.15f
    )
    {
        Bitmap n = NoiseTextureNode.Generate(new());
        Bitmap m1 = MaskTexture.Generate(
            imgSize,
            imgSize,
            numDotsM1,
            maxM1,
            minM1,
            MaskTextureType.Cube,
            true
        );
        Bitmap m2 = MaskTexture.Generate(
            imgSize,
            imgSize,
            numDotsM2,
            maxM2,
            minM2,
            MaskTextureType.EaseInSine,
            true
        );
        Bitmap mix = MixColorNode.Generate(m1, m2, facMix1, MixColorMode.Lighten);
        Bitmap mix2 = MixColorNode.Generate(mix, n, facMix2, MixColorMode.LinearLight);
        mix2.Save(filename);
    }
}
