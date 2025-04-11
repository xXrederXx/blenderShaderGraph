using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.VectorNodes;

public class BumpProps(
    Bitmap heightMap,
    float strength = 1.0f,
    float distance = 1.0f,
    bool invert = false,
    NormalMapFormat format = NormalMapFormat.OpenGL
)
{
    public Bitmap HeightMap { get; } = heightMap;
    public float Strength { get; } = MyMath.Clamp01(strength);
    public float Distance { get; } = Math.Clamp(distance, 0, 1000);
    public bool Invert { get; } = invert;
    public NormalMapFormat Format { get; } = format;
}
