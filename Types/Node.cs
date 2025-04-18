using System.Text.Json;

namespace blenderShaderGraph.Types;
public class Node<T, U>
{
    protected string Id = string.Empty;
    protected JsonElement element;

    public Node(string Id = "", JsonElement element = new())
    {
        this.Id = Id;
        this.element = element;
    }

    // Public
    public U ExecuteNode(T props)
    {
        return ExecuteInternal(SafeProps(props));
    }

    public void ExecuteNodeJSON(Dictionary<string, object> contex, JsonElement element, string Id)
    {
        this.Id = Id;
        this.element = element;

        T props = ConvertJSONToProps(contex);
        U res = ExecuteInternal(SafeProps(props));
        AddDataToContext(res, contex);
    }

    // THESE NEED TO BE OVERRIDDEM IF INHERITED
    protected virtual T SafeProps(T props)
    {
        System.Console.WriteLine("WARNING: No Safe Props defined");
        return props;
    }

    protected virtual U ExecuteInternal(T props)
    {
        throw new NotImplementedException("ExecuteInternal on Node<T, U>");
    }

    protected virtual T ConvertJSONToProps(Dictionary<string, object> contex)
    {
        throw new NotImplementedException("ExecuteInternalJSON on Node<T, U>");
    }

    protected virtual void AddDataToContext(U data, Dictionary<string, object> contex)
    {
        throw new NotImplementedException("AddDataToContext on Node<T, U>");
    }
}
