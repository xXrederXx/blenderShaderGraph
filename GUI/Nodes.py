def GetNoiseDict() -> dict[str, any]:
    return {"widthN": 1024, "heightN": 1024, "detailN": 0, "roughnessN": 0, "sizeN": 0}


def GetDict(name: str) -> dict[str, any]:
    ret: dict[str, any] = {}
    if name.lower() == "noise":
        ret = GetNoiseDict()
        ret["typeS"] = "noise"
    return ret
