from typing import Literal


def GetNoiseDict() -> dict[str, any]:
    return {"widthN": 1024, "heightN": 1024, "detailN": 0, "roughnessN": 0, "sizeN": 0}


def GetMixColorDict() -> dict[str, any]:
    return {"aS": 1024, "bS": 1024, "factorS": 0, "modeS": 0}

# The first value is the type of the node, the second is a dict with its Params,
# The last letter of the key is used to determine the Type: S -> String / N -> Number
NODE_TYPES: list[tuple[str, dict[str, any]]] = [
    ("NoiseTexture", {"widthN": 1024, "heightN": 1024, "detailN": 0, "roughnessN": 0, "sizeN": 0}),
    ("MixColor", {"aS": 1024, "bS": 1024, "factorS": 0, "modeS": 0})]


def GetDict(name: str) -> dict[str, any]:
    ret: dict[str, any] = {}
    for types in NODE_TYPES:
        if types[0] == name:
            ret = types[1]
            ret["typeS"] = name
    return ret
