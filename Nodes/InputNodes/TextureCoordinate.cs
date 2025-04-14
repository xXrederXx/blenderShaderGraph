using System.Drawing;
using blenderShaderGraph.Util;

namespace blenderShaderGraph.Nodes.InputNodes;

public enum TextureCoordinateType
{
    Object,
}

public class TextureCoordinate
{
    public static Bitmap Genearate(
        int width = 1024,
        int height = 1024,
        TextureCoordinateType type = TextureCoordinateType.Object
    )
    {
        Bitmap res = new Bitmap(width, height);
        return type switch
        {
            TextureCoordinateType.Object => Object(res),
            _ => res,
        };
    }

    private static Bitmap Object(Bitmap img)
    {
        int halfWidth = img.Width / 2;
        int halfHeight = img.Height / 2;
        int width = img.Width;
        img.ForPixelParralel(
            (x, y) =>
            {
                float t = MyMath.Map(x, halfWidth, width, 1, 0);
                byte r = MyMath.ClampByte(MyMath.Lerp(1, 0, t) * 255);

                t = MyMath.Map(y, 0, halfHeight, 0, 1);
                byte g = MyMath.ClampByte(MyMath.Lerp(1, 0, t) * 255);
                byte b = 0;

                return Color.FromArgb(255, r, g, b);
            }
        );

        return img;
    }
}
