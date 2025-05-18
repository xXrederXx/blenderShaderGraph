"""Color util"""

from PIL import ImageColor
from util.math import clamp0to255


def hex_to_rgb(hex_val: str) -> tuple[int, int, int]:
    """converts hex to rgb

    Args:
        hex_val (str): hex value with #

    Returns:
        tuple[int, int, int]: r, g, b
    """
    return ImageColor.getrgb(hex_val)


def rgb_to_hex(rgb: tuple[int, int, int]) -> str:
    """Converts rgb to hex

    Args:
        rgb (tuple[int, int, int]): r, g, b

    Returns:
        str: hex with #
    """
    return (
        f"#{clamp0to255(rgb[0]):02x}{clamp0to255(rgb[1]):02x}{clamp0to255(rgb[2]):02x}"
    )


def dimm_color(hex_val: str, ammount: int = 10) -> str:
    """Dimms hex value

    Args:
        hex_val (str): hex color with #
        ammount (int, optional): The amount to dimm. Defaults to 10.

    Returns:
        str: dimmed hex color with #
    """
    r, g, b = hex_to_rgb(hex_val)
    r -= ammount
    g -= ammount
    b -= ammount
    return rgb_to_hex((r, g, b))
