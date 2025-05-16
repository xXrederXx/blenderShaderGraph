"""Used for parsing JSON"""

import json
from log import logger as log
from dataclasses import dataclass
from typing import Optional


@dataclass
class BSGData:
    nodes: list[dict[str, any]]
    selected_node_index: int


def _format_node_dict(_dict: dict[str, any]) -> dict[str, any]:
    """This takes a params dic of a node and returns it with all the types and keys ajusted

    Args:
        _dict (dict[str, any]): The params dict

    Returns:
        dict[str, any]: the ajusted dict
    """

    if _dict.get("id") and _dict.get("type") and _dict.get("params"):
        log.info("The dictionary passed in to format_node_dict is allready formatted")
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


def nodes_to_bsg(data: BSGData) -> str:
    return json.dumps(
        {
            "settings": {"selected_node_index": data.selected_node_index},
            "nodes": data.nodes,
        }
    )


def nodes_from_bsg(data: str) -> BSGData:
    idx = 0
    nodes = []

    json_data: dict[str, any] = json.loads(data)
    
    if isinstance(json_data, list):
        #Old way
        return BSGData(json_data, 0)

    settings: Optional[dict[str, any]] = json_data.get("settings", None)
    if settings:
        idx = settings.get("selected_node_index", 0)

    nodes = json_data.get("nodes", [])
    return BSGData(nodes, idx)


def nodes_to_json(dics: list[dict[str, any]]) -> str:
    """Takes a list of nodes and creates json from them

    Args:
        dics (list[dict[str, any]]): the lis of nodes (params)

    Returns:
        json_string (str): The json as a string
    """
    arr: list[dict[str, any]] = []
    for dic in dics:
        arr.append(_format_node_dict(dic))
    return json.dumps(arr)


def nodes_to_cs(nodes: list[dict[str, any]]) -> str:
    content = ""
    for node in nodes:
        node = _format_node_dict(node)
        var_name: str = node.get("id")
        class_name: str = node.get("type")
        params: dict[str, any] = node.get("params")

        if not var_name or not class_name or not params:
            log.warn(
                f"Could not get every needed value from the node, after formatting. Var Name: {var_name}, Class Name: {class_name}, Params Dict: {params}"
            )
            continue

        class_name = class_name[0].lower() + class_name[1:]

        paramsStr = "new("

        for k, v in params.items():
            paramsStr += f"{k} = {v},"
        paramsStr = paramsStr[:-1]
        paramsStr += ")"

        content += (
            f"var {var_name} = NodeInstances.{class_name}.ExecuteNode({paramsStr});\n"
        )

    return content
