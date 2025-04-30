"""Used for parsing JSON"""
import json


def format_node_dict(_dict: dict[str, any]) -> dict[str, any]:
    """This takes a params dic of a node and returns it with all the types and keys ajusted

    Args:
        _dict (dict[str, any]): The params dict

    Returns:
        dict[str, any]: the ajusted dict
    """
    
    if _dict.get("id") and _dict.get("type") and _dict.get("params"):
        return _dict

    ret: dict[str, any] = {}
    params: dict[str, any] = {}
    for key, value in _dict.items():
        if key == "idS":
            ret["id"] = value
        elif key == "typeS":
            ret["type"] = value
        elif key == "descriptionS":
            ret["description"] = value
        else:
            params[key[0:-1]] = value
    ret["params"] = params
    return ret


def to_json_string(dics: list[dict[str, any]]):
    """Takes a list of nodes and creates json from them

    Args:
        dics (list[dict[str, any]]): the lis of nodes (params)

    Returns:
        json_string (str): The json as a string
    """
    arr: list[dict[str, any]] = []
    for dic in dics:
        arr.append(format_node_dict(dic))
    return json.dumps(arr)
