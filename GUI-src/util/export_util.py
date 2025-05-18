"""Used for parsing JSON"""

import json
from dataclasses import dataclass
from typing import Any, Dict, List, Optional
from globals.my_logger import logger as log


@dataclass
class BSGData:
    """This stores all data inside a .bsg file"""
    nodes: List[Dict[str, Any]]
    selected_node_index: int


def _format_node_Dict(_Dict: Dict[str, Any]) -> Dict[str, Any]:
    """This takes a params dic of a node and returns it with all the types and keys ajusted

    Args:
        _Dict (Dict[str, Any]): The params Dict

    Returns:
        Dict[str, Any]: the ajusted Dict
    """

    if _Dict.get("id") and _Dict.get("type") and _Dict.get("params"):
        log.info("The Dictionary passed in to format_node_Dict is allready formatted")
        return _Dict

    ret: Dict[str, Any] = {}
    params: Dict[str, Any] = {}
    for key, value in _Dict.items():
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
    """This converts BSGData to a json string"""
    return json.dumps(
        {
            "settings": {"selected_node_index": data.selected_node_index},
            "nodes": data.nodes,
        }
    )


def nodes_from_bsg(data: str) -> BSGData:
    """This turns a json string into BSGData"""
    idx = 0
    nodes = []

    json_data: Dict[str, Any] = json.loads(data)

    if isinstance(json_data, list):
        # Old way
        return BSGData(json_data, 0)

    settings: Optional[Dict[str, Any]] = json_data.get("settings", None)
    if settings:
        idx = settings.get("selected_node_index", 0)

    nodes = json_data.get("nodes", [])
    return BSGData(nodes, idx)


def nodes_to_json(dics: List[Dict[str, Any]]) -> str:
    """Takes a list of nodes and creates json from them

    Args:
        dics (List[Dict[str, Any]]): the lis of nodes (params)

    Returns:
        json_string (str): The json as a string
    """
    arr: List[Dict[str, Any]] = []
    for dic in dics:
        arr.append(_format_node_Dict(dic))
    return json.dumps(arr)


def nodes_to_cs(nodes: List[Dict[str, Any]]) -> str:
    """This tryes to convert the nodes to c-sharp code"""
    content = ""
    for node in nodes:
        node = _format_node_Dict(node)
        var_name: str = node.get("id")
        class_name: str = node.get("type")
        params: Dict[str, Any] = node.get("params")

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
