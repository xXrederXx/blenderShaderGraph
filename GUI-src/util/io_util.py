"""This contains some IO functions as well as the paths needed to save data to"""

import tempfile
import os
from io import BytesIO
from pathlib import Path
import threading
from typing import Any, Dict, List
from PIL import Image
import customtkinter as ctk
import requests
import numpy as np
from util.export_util import nodes_to_json
from log import logger as log


def _get_hash(content: List[Dict[str, Any]]) -> str:
    ret_hash = 0x123456789ABCDEF
    for node in content:
        for k, v in node.items():
            ret_hash ^= hash(v)
    return np.base_repr(ret_hash, 36)


def request_image_async(display: ctk.CTkLabel, content: List[Dict[str, Any]]) -> None:
    def task():
        try:
            json_data = nodes_to_json(content)
            print(json_data)
            response = requests.post(
                "http://localhost:5000/generate-image",
                data=json_data,
                headers={"Content-Type": "application/json"},
                timeout=1000,
            )

            if response.status_code != 200:
                log.error(response.text)
                display.after(0, lambda: display.configure(text=response.text))
                return

            save_as_tmp(response.content, content)

            image = Image.open(BytesIO(response.content))
            img = ctk.CTkImage(image, size=(512, 512))

            def update_ui():
                display.configure(image=img)
                display.configure(text="")

            display.after(0, update_ui)

        except Exception as err:
            log.error(str(err))
            display.after(0, lambda err=err: display.configure(text=str(err)))

    threading.Thread(target=task, daemon=True).start()


def save_as_tmp(data: bytes, source: List[Dict[str, Any]]):
    filename = _get_hash(source) + ".png"
    path = get_tmp_path() / filename
    with open(path, "wb") as f:
        f.write(data)


def get_from_tmp(source: List[Dict[str, Any]], display: ctk.CTkLabel):
    filename = _get_hash(source) + ".png"
    path = get_tmp_path() / filename

    if not Path.exists(path):
        log.warn("File Not found: " + str(path))
        return

    content = None
    with open(path, "rb") as f:
        content = f.read()

    image = Image.open(BytesIO(content))
    img = ctk.CTkImage(image, size=(512, 512))

    def update_ui():
        display.configure(image=img)
        display.configure(text="")

    display.after(0, update_ui)


def get_persistant_path() -> Path:
    return Path(os.getenv("LOCALAPPDATA")) / "BSG"


def get_log_path() -> Path:
    return get_persistant_path() / "logs"


def get_tmp_path() -> Path:
    return Path(tempfile.gettempdir()) / "BSG"
