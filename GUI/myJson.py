import json


def FormatNodeDict(_dict: dict[str, any]) -> dict[str, any]:
    if _dict.get("id") and _dict.get("type") and _dict.get("params"):
        return _dict

    ret: dict[str, any] = {}
    params: dict[str, any] = {}
    for key, value in _dict.items():
        if key == "idS":
            ret["id"] = value
        elif key == "typeS":
            ret["type"] = value
        elif key == "descriptionS":
            ret["description"] = value
        else:
            params[key[0:-1]] = value
    ret["params"] = params
    return ret


def ToJsonString(dics: list[dict[str, any]]):
    arr: list[dict[str, any]] = []
    for dic in dics:
        arr.append(FormatNodeDict(dic))
    return json.dumps(arr)
