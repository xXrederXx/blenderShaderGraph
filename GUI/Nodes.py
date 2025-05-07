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
    NodeTypeGroup(
        "TextureNodes",
        "#79461d",
        [
            NodeType(
                "NoiseTexture",
                {
                    "widthI": 1024,
                    "heightI": 1024,
                    "detailF": 2,
                    "roughnessF": 0.5,
                    "sizeF": 1,
                    "LacunarityF": 2,
                },
            ),
            NodeType(
                "BrickTexture",
                {
                    "widthI": 1024,
                    "heightI": 1024,
                    "offsetF": 0.5,
                    "offsetFrequencyI": 2,
                    "squashF": 1,
                    "squashFrequencyI": 0,
                    "color1S": "Black",
                    "color2S": "Black",
                    "colorMotarS": "White",
                    "motarSizeF": 5,
                    "motarSmoothnessF": 0,
                    "biasF": 0,
                    "brickWidtF": 30,
                    "rowHeightI": 12,
                    "forceTilableB": False,
                },
            ),
            NodeType(
                "MaskTexture",
                {
                    "widthI": 1024,
                    "heightI": 1024,
                    "dotsI": 10,
                    "maxSizeI": 124,
                    "minSizeI": 24,
                    "betterDistCalcB": False,
                    "modeS": "square",
                },
            ),
        ],
    ),
    NodeTypeGroup(
        "Color",
        "#6e6e1d",
        [NodeType("MixColor", {"aS": "", "bS": "", "factorS": "", "modeS": ""})],
    ),
    NodeTypeGroup(
        "Converter",
        "#246283",
        [
            NodeType(
                "ColorRamp",
                {"imageS": "", "colorStopsA": "0.4-black,0.5-white", "modeS": "linear"},
            )
        ],
    ),
    NodeTypeGroup(
        "Input",
        "#83314a",
        [
            NodeType(
                "TextureCoordinate",
                {"widthI": 1024, "heightI": 1024, "modeS": "object"},
            )
        ],
    ),
    NodeTypeGroup(
        "Other",
        "#344621",
        [
            NodeType("Resize", {"widthI": 1024, "heightI": 1024, "imageS": ""}),
            NodeType("TileFixer", {"blurF": 16, "imageS": ""}),
        ],
    ),
    NodeTypeGroup(
        "Vector",
        "#3c3c83",
        [
            NodeType(
                "Bump",
                {
                    "heightMapS": "",
                    "strengthF": 1,
                    "distanceF": 1,
                    "invertB": False,
                    "isDXB": False,
                },
            )
        ],
    ),
]
