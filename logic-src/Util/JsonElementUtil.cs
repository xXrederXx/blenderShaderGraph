using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using blenderShaderGraph.Types;

namespace blenderShaderGraph.Util;

public static class JsonElementUtil
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetInt(this JsonElement self, string propName, int def = 0)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using default");
            return def;
        }
        return element.GetInt32();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetFloat(this JsonElement self, string propName, float def = 0)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using default");
            return def;
        }
        return element.GetSingle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetBool(this JsonElement self, string propName)
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found, using false");
            return false;
        }
        return element.GetBoolean();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Input<Bitmap> GetBitmap(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, Input> contex,
        string propName,
        int WidthIfCol = 1024,
        int heightIfCol = 1024
    )
    {
        string dataKey = self.GetString(propName, "");
        if (dataKey.StartsWith('#') && (dataKey.Length == 7 || dataKey.Length == 9))
        {
            Color col = ColorTranslator.FromHtml(dataKey);
            return new(BitmapUtil.FilledBitmap(WidthIfCol, heightIfCol, col));
        }
        if (!contex.TryGetValue(dataKey, out Input? obj) || obj is not Input<Bitmap> bmp)
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {dataKey}");
        }
        return bmp;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Input<MyColor> GetInputMyColor(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, Input> contex,
        string propName
    )
    {
        string img = self.GetString(propName, "");
        if (img.StartsWith('#') && (img.Length == 7 || img.Length == 9))
        {
            Color col = ColorTranslator.FromHtml(img);
            return new Input<MyColor>(col);
        }
        if (!contex.TryGetValue(img, out Input? obj))
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
        }
        if (obj is Input<MyColor> cols)
        {
            return cols;
        }
        if (obj is Input<float> flots)
        {
            return Converter.ToColor(flots);
        }
        throw new FileNotFoundException(
            $"Node {IdSelf} could not find input: {img} or convert it to the right format: {obj.GetType()}"
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Input<float> GetInputFloat(
        this JsonElement self,
        string IdSelf,
        Dictionary<string, Input> contex,
        string propName
    )
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            throw new FileNotFoundException($"Node {IdSelf} could not find property: {propName}");
        }
        if (element.ValueKind == JsonValueKind.Number)
        {
            return new Input<float>(element.GetSingle());
        }
        string img = self.GetString(propName, "");
        if (!contex.TryGetValue(img, out Input? obj) || obj is not Input<float> flot)
        {
            if (obj is not Input<MyColor> col)
            {
                throw new FileNotFoundException($"Node {IdSelf} could not find input: {img}");
            }
            if(col.useArray)
            {
                return new Input<float>(Converter.ConvertToFloat(col.Array ?? new MyColor[0, 0]));
            }
            return new Input<float>(col.Value.GetGrayscale());
        }
        return flot;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyColor GetColor(this JsonElement self, string propName, string def = "#000000")
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            return ColorTranslator.FromHtml(def);
        }
        return ColorTranslator.FromHtml(element.GetString(propName, def));
    }

    public static T GetT<T>(
        this JsonElement self,
        Dictionary<string, Input> context,
        string propName,
        T def
    )
    {
        if (!self.TryGetProperty(propName, out JsonElement element))
        {
            System.Console.WriteLine($"Property {propName} not found");
            return def;
        }
        return element.ValueKind switch
        {
            JsonValueKind.String => element.HandleString(def, context),
            JsonValueKind.Number => element.HandleNumber(def),
            _ => def,
        };
    }

    private static T HandleString<T>(
        this JsonElement self,
        T def,
        Dictionary<string, Input> context
    )
    {
        string contextKey = self.GetString() ?? string.Empty;
        if (typeof(T) == typeof(string))
        {
            return Unsafe.As<string, T>(ref contextKey);
        }
        if (context.TryGetValue(contextKey, out Input? val)) { }
        return def;
    }

    private static T HandleNumber<T>(this JsonElement self, T def)
    {
        Type type = typeof(T);
        if (type == typeof(byte) && self.TryGetByte(out byte valB))
        {
            return Unsafe.As<byte, T>(ref valB);
        }
        if (type == typeof(sbyte) && self.TryGetSByte(out sbyte valBS))
        {
            return Unsafe.As<sbyte, T>(ref valBS);
        }
        if (type == typeof(short) && self.TryGetInt16(out short valS))
        {
            return Unsafe.As<short, T>(ref valS);
        }
        if (type == typeof(ushort) && self.TryGetUInt16(out ushort valUS))
        {
            return Unsafe.As<ushort, T>(ref valUS);
        }
        if (type == typeof(int) && self.TryGetInt32(out int valI))
        {
            return Unsafe.As<int, T>(ref valI);
        }
        if (type == typeof(uint) && self.TryGetUInt32(out uint valUI))
        {
            return Unsafe.As<uint, T>(ref valUI);
        }
        if (type == typeof(long) && self.TryGetInt64(out long valL))
        {
            return Unsafe.As<long, T>(ref valL);
        }
        if (type == typeof(ulong) && self.TryGetUInt64(out ulong valUL))
        {
            return Unsafe.As<ulong, T>(ref valUL);
        }
        if (type == typeof(float) && self.TryGetSingle(out float valF))
        {
            return Unsafe.As<float, T>(ref valF);
        }
        if (type == typeof(double) && self.TryGetDouble(out double valD))
        {
            return Unsafe.As<double, T>(ref valD);
        }
        return def;
    }
}
