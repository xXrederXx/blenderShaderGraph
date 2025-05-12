# How To Add A Node

## 1. Abstract creation
First you need to thing of a Node in an Abstract way. You will need to write these things down.

- What are input values
- What should the node Generate or Modify
- What are output values

For the input / output also decide which value types you will need.

## 2. Logic (C-Sharp)
Now your task is to implement the Node in C-Sharp. Go into the ```Nodes``` folder and create a file in the fitting subfolder. Then you should start with the Props-Class. A Props-Class is a class which holds all input data for a node. Aim to not use ```int``` or ```float[,]``` instead use ```Input<int>``` and ```Input<float>```. Here is an example of a Props-Class:

```csharp
public class MyNodeProps
{
    public MyNodeProps() { }

    public MyNodeProps(
        Input<MyColor>? param1,
        Input<float>? param2,
    )
    {
        this.Param1 = param1;
        this.Param2 = param2;
    }

    public Input<MyColor>? Param1 { get; set; }
    public Input<float>? Param2 { get; set; }
}
```

After you implemented this you can start implementing the Node-Class. Create a class and inherit from ```Node<T, U>```. ```T``` is the input type, so use ```MyNodeProps```. ```U``` is the output type. Most times you will use ```Input<MyColor>``` or ```Input<float>```. The you will need to override the functions. I like to start with ```SafeProps```. This function takes the Inputs and returns a safe version. For example clamp some values or check for nulls. Here is an example:

```csharp
protected override MyNodeProps SafeProps(MyNodeProps props)
{
    if (props.Param1 is null)
        System.Console.WriteLine("a props is null");
    if (props.Param2 is null)
    {
        System.Console.WriteLine("b props is null");
        props.Param2 = new(0)
    }

    return props;
}
```

Then you should start with the ```ConvertToJSONProps```. This fuction takes a Dictionary and returns the Props-Class. First you will take the Params JsonElement and then you will need to map its keys to the Props-Class. Here is an example:

```csharp
 protected override MyNodeProps ConvertJSONToProps(Dictionary<string, Input> contex)
{
    JsonElement p = element.GetProperty("params");

    return new MyNodeProps()
    {
        ImageA = p.GetInputMyColor(Id, contex, "param1"),
        Factor = p.GetInputFloat(Id, contex, "param2"),
    };
}
```

After you have done this, you should override the ```AddDataToContext``` function. Most times you just add the value to the context, but sometimes you can add suffixes. Here is an example:

```csharp
protected override void AddDataToContext(Input<MyColor> data, Dictionary<string, Input> contex)
{
    contex[Id] = data;
    contex[Id + ".single"] = data.Value
}
```

Then there is the most complicated part. Implementing the logic. ```ExecuteInternal``` is the function whic gets called when the node gets executed, it can only be ran with safe props. You will need to write this function on you own.

Finaly you need to add your node to the GraphRunner functions.

## 3. GUI (Python)
Now there is everyithing working on the C-Sharp side. Lets implement the GUI.
This is relatively straight foreward. Just open the ```nodes.py``` in the ```GUI-src``` folder. Then search the nodegroup in witch you will put the node, you may need to create a new one. In this case jsu add it to the end of the list and give it a name and a color.
Then add a ```NodeType``` to the array inside of the ```NodeGroup```. The node Type needs a name. The name must exactly match the one spesifyed on the C-Sharp side. 
Then create a Dictionary with all the posible Params. The keys are defined like this: ```ParamKey:ParamType(s)```. The ParamKey must match the one on the C-Sharp side. The ParamType(s) spesify which types are valide. The options are:

- I - Intiger
- F - Floating point Number
- B - Boolean
- S - String
- E - Enum
  
The Enums are defined like this ```ParamKey:E-V1-V2-V3-ETC```. This would represemt an enum with the possible values of V1, V2, V3, ETC
If you implemented everything it should look something like this:
```python
NodeTypeGroup(
    "MyGroup",
    "#3c3c83",
    [
        NodeType(
            "MyNodeType",
            {
                "Param1:S": "",
                "Param2:F": 1,
                "Param3:E-XD-DX-AR": "XD",
            },
        )
    ],
),
```

## 4. Add JSON schema (optional)
If you want you can add your node to the json-schema. But this slowly bekomes obsolet with the usage of the GUI.

But if you want to do it here are some simple steps to do:
1. Add your node to ```items > discriminator > mapping```
2. Add your node to ```items > oneOf```
3. Copy paste the following to ```$defs```
```json
"NodeName": {
    "type": "object",
    "required": ["id", "type", "params"],
    "properties": {
        "id": { "type": "string" },
        "type": { "const": "Resize" },
        "params": {
            "type": "object",
            "required": ["<All required params>"],
            "properties": {
                "param1Name": { "type": "string" },
                "param2Name": { "type": "number" },
                "enum-exmpl": { 
                    "type": "string",
					"enum": ["V1", "V2", "V3", "ETC"]
                },
                "multi-type-exmpl": {
                    "anyOf": [{ "type": "number" }, { "type": "string" }]
                }
            }
        }
    }
},
```
4. Ajust all values