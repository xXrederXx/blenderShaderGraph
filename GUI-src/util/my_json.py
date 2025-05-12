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
        if key == "id:S":
            ret["id"] = value
        elif key == "type:S":
            ret["type"] = value
        elif key == "description:S":
            ret["description"] = value
        else:
            params[key.split(":")[0]] = value
    ret["params"] = params
    return ret


def unformat_node_dict(formatted: dict[str, any]) -> dict[str, any]:
    """Reverts a formatted node dict back to the original app format.

    Args:
        formatted (dict[str, any]): A formatted dict with 'id', 'type', 'params'.

    Returns:
        dict[str, any]: The original-style dict with 'idS', 'typeS', etc.
    """
    print("Not working with type system")
    return
    result: dict[str, any] = {}

    if "id" in formatted:
        result["idS"] = formatted["id"]
    if "type" in formatted:
        result["typeS"] = formatted["type"]
    if "description" in formatted:
        result["descriptionS"] = formatted["description"]

    if "params" in formatted:
        for key, value in formatted["params"].items():
            result[f"{key}N"] = value

    return result


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


def from_json_string(json_str: str) -> list[dict[str, any]]:
    """Parses a JSON string and converts formatted dicts back to original format.

    Args:
        json_str (str): JSON string from to_json_string().

    Returns:
        list[dict[str, any]]: The original-style dicts.
    """
    try:
        raw_list = json.loads(json_str)
        return [unformat_node_dict(d) for d in raw_list]
    except json.JSONDecodeError as e:
        raise ValueError(f"Invalid JSON string: {e}") from e


def from_json_file(file_path: str) -> list[dict[str, any]]:
    """Reads a JSON file and converts formatted dicts back to original format.

    Args:
        file_path (str): Path to the JSON file.

    Returns:
        list[dict[str, any]]: The original-style dicts.
    """
    try:
        with open(file_path, "r", encoding="utf-8") as f:
            raw_list = json.load(f)
        return [unformat_node_dict(d) for d in raw_list]
    except (json.JSONDecodeError, FileNotFoundError, OSError) as e:
        raise ValueError(f"Error reading from file: {e}") from e
