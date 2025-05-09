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
