from io import BytesIO
from pathlib import Path
import threading
from PIL import Image
import customtkinter as ctk
import requests
import numpy as np
from util.export_util import to_json_string
from log import logger as log

def _get_hash(content: list[dict[str, any]]) -> str:
    ret_hash = 0x123456789abcdef
    for node in content:
        for k, v in node.items():
            ret_hash ^= hash(v)
    return np.base_repr(ret_hash, 36)

def request_image_async(display: ctk.CTkLabel, content: list[dict[str, any]]) -> None:
    def task():
        try:
            json_data = to_json_string(content)
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


def save_as_tmp(data: bytes, source: list[dict[str, any]]):
    filename = _get_hash(source) + ".png"
    directory = Path.cwd() / "tmp" / "img"
    directory.mkdir(parents=True, exist_ok=True)
    path = directory / filename
    with open(path, "wb") as f:
        f.write(data)
        
def get_from_tmp(source: list[dict[str, any]], display: ctk.CTkLabel):
    filename = _get_hash(source) + ".png"
    directory = Path.cwd() / "tmp" / "img"
    directory.mkdir(parents=True, exist_ok=True)
    path = directory / filename
    
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
    
    