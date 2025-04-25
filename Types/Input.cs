namespace blenderShaderGraph.Types;

public static class InputDefaults
{
    public static readonly Input<float> floatInput = new Input<float>(0);
    public static readonly Input<MyColor> colorBlackInput = new Input<MyColor>(MyColors.Black);
}

public class Input { }

public class Input<T> : Input
{
    const int defaultSize = 1024;

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

    public T this[int x, int y]
    {
        // Not able to be null

        get => useArray ? Array[x, y] : Value;
        set { Console.Write("WARNING: Cant set value of Input<T>"); }
    }
    public int Width => useArray ? Array.GetLength(0) : defaultSize;
    public int Height => useArray ? Array.GetLength(1) : defaultSize;

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.

    public readonly T? Value;
    public readonly T[,]? Array;
    public readonly bool useArray;

    public Input(T value)
    {
        Value = value;
        useArray = false;
    }

    public Input(T[,] value)
    {
        Array = value;
        useArray = true;
    }
}
