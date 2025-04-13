using System.Drawing;

namespace blenderShaderGraph.Nodes.OtherNodes;

public static class ResizeNode
{
    public static Bitmap Generate(Bitmap img, int width, int height)
    {
        return new Bitmap(img, width, height);
    }
}
