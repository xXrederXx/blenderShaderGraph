"""Avoid lint"""

import threading
import time
from io import BytesIO
from pathlib import Path
from typing import Any, List
from typing import Optional

from tkinter import filedialog as fd
import customtkinter as ctk
import requests
from PIL import Image

from Frames.AddNodeFrame import AddNodeFrame
from Frames.NodeConfigFrame import NodeConfigFrame
from nodes import NEW_NODE_TYPES
from Frames.NodeListFrame import NodeListFrame
from util.my_json import from_json_file, to_json_string
from style import FRAME_GRID_KWARGS, MAIN_BG_COL

class NodeAppMainFrame(ctk.CTkFrame):
    """Main application for managing nodes."""

    def __init__(self, master) -> None:
        super().__init__(master, fg_color=MAIN_BG_COL)
        self.nodes: List[dict[str, Any]] = []
        self.selected_node_index: Optional[int] = None

        self._setup_layout()
        self._create_frames()

    def _setup_layout(self) -> None:
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure((1, 2), weight=2)
        self.grid_rowconfigure(0, weight=1)

    def _create_frames(self) -> None:
        self.node_list_frame = NodeListFrame(
            self,
            on_node_select=self.show_details,
            on_node_pos_change=self.change_node_pos,
        )
        self.node_list_frame.grid(row=0, column=0, **FRAME_GRID_KWARGS)

        self.config_frame = NodeConfigFrame(
            self, on_update=self.update_node, on_req_img=self.request_node_image
        )
        self.config_frame.grid(row=0, column=1, **FRAME_GRID_KWARGS)

        self.add_node_frame = AddNodeFrame(
            self,
            on_add_node=self.add_node,
            on_generate_image=self.generate_image,
            on_group_change=self.update_node_type_menu,
        )
        add_node_frame_style = FRAME_GRID_KWARGS
        add_node_frame_style.pop("padx")
        add_node_frame_style.pop("pady")
        self.add_node_frame.grid(row=0, column=2, **add_node_frame_style)

    def change_node_pos(self, idx: int, offset: int) -> None:
        """Changes node pos in list

        Args:
            idx (int): the current index
            offset (int): the offset
        """
        if idx + offset < 0 or idx + offset >= len(self.nodes):
            return

        node = self.nodes.pop(idx)
        self.nodes.insert(idx + offset, node)

        self.node_list_frame.update_node_list(self.nodes)

    def add_node(self) -> None:
        """Adds a new node to the list."""
        name = self.add_node_frame.new_id_entry.get()
        description = self.add_node_frame.new_desc_entry.get()
        selected_type_name = self.add_node_frame.node_type_var.get()
        selected_group_name = self.add_node_frame.node_groups_var.get()

        if not name or not selected_type_name:
            return

        node_group = next(
            (g for g in NEW_NODE_TYPES if g.name == selected_group_name), None
        )
        if not node_group:
            return

        node_type = next(
            (nt for nt in node_group.nodes if nt.name == selected_type_name), None
        )
        if not node_type:
            return

        new_node = dict(node_type.params)
        new_node["idS"] = name
        new_node["descriptionS"] = description
        new_node["typeS"] = selected_type_name

        self.nodes.append(new_node)
        self.node_list_frame.update_node_list(self.nodes)

        self.add_node_frame.new_id_entry.delete(0, "end")
        self.add_node_frame.new_desc_entry.delete(0, "end")

    def show_details(self, idx: int) -> None:
        """Display details of selected node."""
        self.selected_node_index = idx
        selected_node = self.nodes[idx]
        self.config_frame.show_details(selected_node)

    def update_node(self) -> None:
        """Update the currently selected node from config fields."""
        if self.selected_node_index is None:
            return

        node = self.nodes[self.selected_node_index]
        for field_name, entry in self.config_frame.custom_field_entries.items():
            value = entry.get()
            try:
                if field_name.endswith("N"):
                    node[field_name] = int(value)
                elif field_name.endswith("B"):
                    node[field_name] = value.lower() in ("true", "1", "yes")
                else:
                    node[field_name] = str(value)
            except ValueError:
                print(f"Invalid input for field {field_name}: {value}")

        self.node_list_frame.update_node_list(self.nodes)
        self.show_details(self.selected_node_index)

    def update_node_type_menu(self, selected_group: str) -> None:
        """Update node type dropdown and color on group change."""
        self.add_node_frame.update_node_type_menu(selected_group)

    def generate_image(self) -> None:
        """Stub for generating an image."""
        self._request_image_async(self.add_node_frame.generated_image, self.nodes)

    def request_node_image(self) -> None:
        """Requests an image for the currently selected node"""
        nodes = self.nodes[0 : self.selected_node_index + 1]
        self._request_image_async(self.config_frame.image_label, nodes)

    def _request_image_async(
        self, display: ctk.CTkLabel, content: list[dict[str, Any]]
    ) -> None:
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

    def _save_image_to_file(self, content: bytes) -> str:
        filename = time.strftime("%d_%H-%M-%S", time.localtime()) + ".png"
        directory = Path.cwd() / "tmp" / "img"
        directory.mkdir(parents=True, exist_ok=True)
        path = directory / filename
        with open(path, "wb") as f:
            f.write(content)
        return str(path)

    def on_save_btn(self):
        """Used to save to a file"""
        fp = fd.asksaveasfilename(
            defaultextension=".bsg",
            filetypes=[("bsg file", "*.bsg"), ("json file", "*.json")],
        )
        if not fp:
            return
        content = to_json_string(self.nodes)
        with open(fp, "w", encoding="utf-8") as f:
            f.write(content)

    def on_load_btn(self):
        """Used to load from a file"""
        fp = fd.askopenfilename(
            filetypes=[("bsg file", "*.bsg"), ("json file", "*.json")]
        )
        if not fp:
            return
        self.nodes = from_json_file(fp)
        self.node_list_frame.update_node_list(self.nodes)

    def on_export_btn(self):
        """used to export to a file"""
        self.on_save_btn()
