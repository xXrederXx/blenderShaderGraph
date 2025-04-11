using System;
using System.Text.Json;

namespace blenderShaderGraph.Nodes;

public abstract class IJsonNode
{
    public string Id { get; }
    protected JsonElement _element;

    public IJsonNode(string id, JsonElement element)
    {
        Id = id;
        _element = element;
    }

    public abstract void Execute(Dictionary<string, object> contex);
}
