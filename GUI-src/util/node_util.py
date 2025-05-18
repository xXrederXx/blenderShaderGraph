from typing import Any
from custom_components.my_entry import (
    MyEntry,
    StringEntry,
    EnumEntry,
    IntEntry,
    BoolEntry,
    FloatEntry,
)
from log import logger as log


def change_node_pos(_list: list, idx: int, offset: int) -> None:
    """Changes node pos in list

    Args:
        idx (int): the current index
        offset (int): the offset
    """
    if idx + offset < 0 or idx + offset >= len(_list):
        log.info("Cant move Node out of bounds")
        return

    node = _list.pop(idx)
    _list.insert(idx + offset, node)


def convert_value(key: str, value: Any) -> Any:
    """Update the currently selected node from config fields."""
    splits = key.split(":")
    if len(splits) != 2:
        log.error("The provided Key cant be split into tow parts. The key: " + key)
        raise ValueError("Key dosnt split into tow")
    splits = splits[1]

    if splits.startswith("E-"):
        return value

    types = [splits[i] for i in range(len(splits))]
    for type in types:
        try:
            if type == "I":
                return int(value)
            if type == "F":
                return float(value)
            if type == "B":
                return value.lower() in ("true", "1", "yes")
            if type == "S":
                return str(value)
        except:
            log.debug(
                "Type not able to convert Trying the next one. Type tryed: " + type
            )
            continue
    log.error("There was no matching conversion found for the key " + key)
    raise TypeError("No matching conversion found")


def get_my_entry(key: str, **kwargs) -> MyEntry:
    """Maps each key to an entry based on the type suffix

    Args:
        key (str): the key with the type suffix

    Returns:
        MyEntry: The needed Entry
    """
    if not len(key) == 1 and not key.startswith("E-"):
        log.info("Using string, because there are multiple valid types")
        return StringEntry(**kwargs)

    if key == "I":
        return IntEntry(**kwargs)
    if key == "F":
        return FloatEntry(**kwargs)
    if key == "B":
        return BoolEntry(**kwargs)
    if key.startswith("E-"):
        key = key.removeprefix("E-")
        enum = key.split("-")
        return EnumEntry(enum=enum, **kwargs)
    return StringEntry(**kwargs)
