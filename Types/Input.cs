public class Input<T>
{
    public T this[int x, int y]
    {
        get { return useArray ? Values[x, y] : Value; }
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
