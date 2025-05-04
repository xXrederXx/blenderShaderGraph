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
                    "widthN": 1024,
                    "heightN": 1024,
                    "detailN": 2,
                    "roughnessN": 0.5,
                    "sizeN": 1,
                    "LacunarityN": 2,
                },
            ),
            NodeType(
                "BrickTexture",
                {
                    "widthN": 1024,
                    "heightN": 1024,
                    "offsetN": 0.5,
                    "offsetFrequencyN": 2,
                    "squashN": 1,
                    "squashFrequencyN": 0,
                    "color1S": "Black",
                    "color2S": "Black",
                    "colorMotarS": "White",
                    "motarSizeN": 5,
                    "motarSmoothnessN": 0,
                    "biasN": 0,
                    "brickWidthN": 30,
                    "rowHeightN": 12,
                    "forceTilableB": False,
                },
            ),
            NodeType(
                "MaskTexture",
                {
                    "widthN": 1024,
                    "heightN": 1024,
                    "dotsN": 10,
                    "maxSizeN": 124,
                    "minSizeN": 24,
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
                {"widthN": 1024, "heightN": 1024, "modeS": "object"},
            )
        ],
    ),
    NodeTypeGroup(
        "Other",
        "#344621",
        [
            NodeType("Resize", {"widthN": 1024, "heightN": 1024, "imageS": ""}),
            NodeType("TileFixer", {"blurN": 16, "imageS": ""}),
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
                    "strengthN": 1,
                    "distanceN": 1,
                    "invertB": False,
                    "isDXB": False,
                },
            )
        ],
    ),
]
