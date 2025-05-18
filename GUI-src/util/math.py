"""Contains some Math util"""


def clamp0to255(x: int) -> int:
    """Clamps x to 0 and 255

    Args:
        x (int): value to clamp

    Returns:
        int: clamped
    """
    return max(0, min(x, 255))
