from PIL import ImageColor


def clamp0to255(x):
    return max(0, min(x, 255))


def hex_to_rgb(hex: str) -> tuple[int, int, int]:
    return ImageColor.getrgb(hex)


def rgb_to_hex(rgb: tuple[int, int, int]) -> str:
    return f"#{clamp0to255(rgb[0]):02x}{clamp0to255(rgb[1]):02x}{clamp0to255(rgb[2]):02x}"


def dimm_color(hex: str, ammount: int = 10) -> str:
    r, g, b = hex_to_rgb(hex)
    r -= ammount
    g -= ammount
    b -= ammount
    print(rgb_to_hex((r, g, b)))
    return rgb_to_hex((r, g, b))
