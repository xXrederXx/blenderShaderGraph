"""Used for dataclasses (Easy way to stor data)"""

from dataclasses import dataclass


@dataclass
class NodeType:
    """Contains name and params of a Node Type"""

    name: str
    params: dict[str, any]

    def __post_init__(self):
        self.params["type:S"] = self.name


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
                    "width:I": 1024,
                    "height:I": 1024,
                    "detail:F": 2,
                    "roughness:F": 0.5,
                    "size:F": 1,
                    "Lacunarity:F": 2,
                },
            ),
            NodeType(
                "BrickTexture",
                {
                    "width:I": 1024,
                    "height:I": 1024,
                    "offset:F": 0.5,
                    "offsetFrequency:I": 2,
                    "squash:F": 1,
                    "squashFrequency:I": 0,
                    "color1:S": "Black",
                    "color2:S": "Black",
                    "colorMotar:S": "White",
                    "motarSize:F": 5,
                    "motarSmoothness:F": 0,
                    "bias:F": 0,
                    "brickWidt:F": 30,
                    "rowHeight:I": 12,
                    "forceTilable:B": False,
                },
            ),
            NodeType(
                "MaskTexture",
                {
                    "width:I": 1024,
                    "height:I": 1024,
                    "dots:I": 10,
                    "maxSize:I": 124,
                    "minSize:I": 24,
                    "betterDistCalc:B": False,
                    "mode:S": "square",
                },
            ),
        ],
    ),
    NodeTypeGroup(
        "Color",
        "#6e6e1d",
        [NodeType("MixColor", {"a:S": "", "b:S": "", "factor:FS": "", "mode:E-mix-lighten-darken-linearlight-hue-value-saturation": ""})],
    ),
    NodeTypeGroup(
        "Converter",
        "#246283",
        [
            NodeType(
                "ColorRamp",
                {"image:S": "", "colorStops:S": "0.4-black,0.5-white", "mode:E-linear-constant": "linear"},
            )
        ],
    ),
    NodeTypeGroup(
        "Input",
        "#83314a",
        [
            NodeType(
                "TextureCoordinate",
                {"width:I": 1024, "height:I": 1024, "mode:E-object": "object"},
            )
        ],
    ),
    NodeTypeGroup(
        "Other",
        "#344621",
        [
            NodeType("Resize", {"width:I": 1024, "height:I": 1024, "image:S": ""}),
            NodeType("TileFixer", {"blur:F": 16, "image:S": ""}),
        ],
    ),
    NodeTypeGroup(
        "Vector",
        "#3c3c83",
        [
            NodeType(
                "Bump",
                {
                    "heightMap:S": "",
                    "strength:F": 1,
                    "distance:F": 1,
                    "invert:B": False,
                    "isDX:B": False,
                },
            )
        ],
    ),
]
