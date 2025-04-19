using System.Drawing;
using System.Text.Json;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Util;

public static class JsonElementUtil
{
    public static int GetInt(this JsonElement self, string propName, int def = 0)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using default");
            return def;
        }
        return element.GetInt32();
    }

    public static float GetFloat(this JsonElement self, string propName, float def = 0)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using default");
            return def;
        }
        return (float)element.GetDouble();
    }

    public static string GetString(this JsonElement self, string propName, string def = "")
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using default");
            return def;
        }
        string? val = element.GetString();
        return val is null ? def : val;
    }

    public static bool GetBool(this JsonElement self, string propName)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using false");
            return false;
        }
        return element.GetBoolean();
    }

    public static Bitmap GetBitmap(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, object> contex,
        string propName,
        int WidthIfCol = 1024,
        int heightIfCol = 1024
    )
    {
        string dataKey = self.GetString(propName, "");
        if (dataKey.StartsWith('#') && (dataKey.Length == 7 || dataKey.Length == 9))
        {
            Color col = ColorTranslator.FromHtml(dataKey);
            return BitmapUtil.FilledBitmap(WidthIfCol, heightIfCol, col);
        }
        if (!contex.TryGetValue(dataKey, out object? obj) || obj is not Bitmap bmp)
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {dataKey}");
        }
        return new(bmp);
    }

    public static Input<MyColor> GetInputMyColor(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, object> contex,
        string propName
    )
    {
        string img = self.GetString(propName, "");
        if (img.StartsWith('#') && (img.Length == 7 || img.Length == 9))
        {
            Color col = ColorTranslator.FromHtml(img);
            return new Input<MyColor>(col);
        }
        if (!contex.TryGetValue(img, out object? obj) || obj is not MyColor[,] cols)
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
        }
        return new Input<MyColor>(cols);
    }

    public static float[,] GetFloat2D(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, object> contex,
        string propName
    )
    {
        string img = self.GetString(propName, "");
        if (!contex.TryGetValue(img, out object? obj) || obj is not float[,] cols)
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
        }
        return cols;
    }

    public static Input<float> GetInputFloat(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, object> contex,
        string propName
    )
    {
        string img = self.GetString(propName, "");
        if (!contex.TryGetValue(img, out object? obj))
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
        }
        if (obj is not float[,] floats)
        {
            if (obj is not float f)
            {
                throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
            }
            return new(f);
        }
        return new(floats);
    }

    public static MyColor GetColor(this JsonElement self, string propName, string def = "#000000")
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            return ColorTranslator.FromHtml(def);
        }
        return ColorTranslator.FromHtml(element.GetString(propName, def));
    }
}
