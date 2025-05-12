from io import BytesIO
from pathlib import Path
import threading
import time
from PIL import Image
import customtkinter as ctk
import requests

from util.my_json import to_json_string


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
                print(response.text)
                display.after(0, lambda: display.configure(text=response.text))
                return

            image = Image.open(BytesIO(response.content))
            img = ctk.CTkImage(image, size=(512, 512))

            def update_ui():
                display.configure(image=img)
                display.configure(text="")

            display.after(0, update_ui)

        except Exception as err:
            print(f"Error: {str(err)}")
            display.after(0, lambda err=err: display.configure(text=str(err)))

    threading.Thread(target=task, daemon=True).start()


def save_image_to_file(content: bytes) -> str:
    filename = time.strftime("%d_%H-%M-%S", time.localtime()) + ".png"
    directory = Path.cwd() / "tmp" / "img"
    directory.mkdir(parents=True, exist_ok=True)
    path = directory / filename
    with open(path, "wb") as f:
        f.write(content)
    return str(path)
