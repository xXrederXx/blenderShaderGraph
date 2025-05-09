def change_node_pos(list: list, idx: int, offset: int) -> None:
    """Changes node pos in list

    Args:
        idx (int): the current index
        offset (int): the offset
    """
    if idx + offset < 0 or idx + offset >= len(list):
        return

    node = list.pop(idx)
    list.insert(idx + offset, node)

def convert_value(key:str, value:any) -> any:
        """Update the currently selected node from config fields."""
        if key.endswith("I"):
            return int(value)
        elif key.endswith("F"):
            return float(value)
        elif key.endswith("B"):
            return value.lower() in ("true", "1", "yes")
        else:
            return str(value)