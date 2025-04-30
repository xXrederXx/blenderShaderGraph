"""Used for dataclasses (Easy way to stor data)"""
from dataclasses import dataclass


@dataclass
class NodeType:
    """Contains name and params of a Node Type"""
    name: str
    params: dict[str, any]

    def __post_init__(self):
        self.params["typeS"] = self.name


@dataclass
class NodeTypeGroup:
    """Contains a group name, a group color, and a list of node types for the group"""
    name: str
    color: str
    nodes: list[NodeType]


NEW_NODE_TYPES: list[NodeTypeGroup] = [
    NodeTypeGroup("TextureNodes", "#7a4405", [
        NodeType("NoiseTexture", {
                 "widthN": 1024, "heightN": 1024, "detailN": 0, "roughnessN": 0, "sizeN": 0})
    ]
    ),
    NodeTypeGroup("Color", "#9c9c00", [
        NodeType("MixColor", {"aS": 1024, "bS": 1024,
                 "factorS": 0, "modeS": 0})
    ]
    )
]
