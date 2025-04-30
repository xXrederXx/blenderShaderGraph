class NodeType:
    def __init__(self, type_name:str, params: dict[str, any]):
        self.name = type_name
        self.params = params
        self.params["typeS"] = type_name

class NodeTypeGroup:
    def __init__(self, group_name:str, group_color:str, nodes: list[NodeType]):
        self.name = group_name
        self.color = group_color
        self.nodes = nodes

NEW_NODE_TYPES: list[NodeTypeGroup] = [
    NodeTypeGroup("TextureNodes", "Orange", [
        NodeType("NoiseTexture", {"widthN": 1024, "heightN": 1024, "detailN": 0, "roughnessN": 0, "sizeN": 0})
        ]
    ),
    NodeTypeGroup("Color", "Yellow", [
        NodeType("MixColor", {"aS": 1024, "bS": 1024, "factorS": 0, "modeS": 0})
        ]
    )
]


def GetDict(name: str) -> dict[str, any]:
    ret: dict[str, any] = {}
    for types in NODE_TYPES:
        if types[0] == name:
            ret = types[1]
            ret["typeS"] = name
    return ret
