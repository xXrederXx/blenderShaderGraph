public class Input { }

public class Input<T> : Input
{
    public T this[int x, int y]
    {
        // Not able to be null
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        get => useArray ? Values[x, y] : Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.

        set { Console.Write("WARNING: Cant set value of Input<T>"); }
    }

    private T? Value;
    private T[,]? Values;
    private bool useArray;

    public Input(T value)
    {
        Value = value;
        useArray = false;
    }

    public Input(T[,] value)
    {
        Values = value;
        useArray = true;
    }
}
