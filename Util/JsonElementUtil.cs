using System;
using System.Drawing;
using System.Text.Json;

namespace blenderShaderGraph.Util;

public static class JsonElementUtil
{
    public static int GetInt(this JsonElement self, string propName, int def = 0)
    {
        if(!self.TryGetProperty(propName, out JsonElement element))
        {
            return def;
        }
        int val = element.GetInt32();
        return val == 0 ? def : val;
    }

    public static float GetFloat(this JsonElement self, string propName, float def = 0)
    {
        if(!self.TryGetProperty(propName, out JsonElement element))
        {
            return def;
        }
        float val = (float)element.GetDouble();
        return val == 0f ? def : val;
    }

    public static string GetString(this JsonElement self, string propName, string def = "")
    {
        if(!self.TryGetProperty(propName, out JsonElement element))
        {
            return def;
        }
        string? val = element.GetString();
        return val is null ? def : val;
    }
    public static bool GetBool(this JsonElement self, string propName)
    {
        if(!self.TryGetProperty(propName, out JsonElement element))
        {
            return false;
        }
        return element.GetBoolean();
    }
    public static Bitmap GetBitmap(this JsonElement self, string IdSelf, Dictionary<string, object> contex, string propName)
    {
        string img = self.GetString(propName, "");
        if (!contex.TryGetValue(img, out var obj) || obj is not Bitmap bmp)
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
        }
        return new(bmp);
    }
}
