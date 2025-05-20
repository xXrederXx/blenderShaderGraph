"""Used for dataclasses (Easy way to stor data)"""

from typing import List
from my_types.node import Node, NodeGroup

NEW_NODE_TYPES: List[NodeGroup] = [
    NodeGroup(
        "TextureNodes",
        "#79461d",
        [
            Node(
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
            Node(
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
            Node(
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
    NodeGroup(
        "Color",
        "#6e6e1d",
        [
            Node(
                "MixColor",
                {
                    "a:S": "",
                    "b:S": "",
                    "factor:FS": "",
                    "mode:E-mix-lighten-darken-linearlight-hue-value-saturation": "",
                },
            )
        ],
    ),
    NodeGroup(
        "Converter",
        "#246283",
        [
            Node(
                "ColorRamp",
                {
                    "image:S": "",
                    "colorStops:S": "black-0,white-1",
                    "mode:E-linear-constant": "linear",
                },
            )
        ],
    ),
    NodeGroup(
        "Input",
        "#83314a",
        [
            Node(
                "TextureCoordinate",
                {"width:I": 1024, "height:I": 1024, "mode:E-object": "object"},
            )
        ],
    ),
    NodeGroup(
        "Other",
        "#344621",
        [
            Node("Resize", {"width:I": 1024, "height:I": 1024, "image:S": ""}),
            Node("TileFixer", {"blur:F": 16, "image:S": ""}),
        ],
    ),
    NodeGroup(
        "Vector",
        "#3c3c83",
        [
            Node(
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
