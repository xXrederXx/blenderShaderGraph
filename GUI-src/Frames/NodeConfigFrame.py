"""Avoid lint"""

from typing import Callable, Dict

import customtkinter as ctk

from globals.nodes import NEW_NODE_TYPES
from globals.my_logger import MyLogger
from globals.style import StyleManager
from util.node_util import get_my_entry
from custom_components.my_entry import MyEntry
from util.color_util import dimm_color

log = MyLogger()
style = StyleManager.get()


class NodeConfigFrame(ctk.CTkFrame):
    """Frame for configuring and editing a selected node."""

    def __init__(
        self,
        master,
        on_update: Callable[[], None],
        on_req_img: Callable[[], None],
        on_auto_req: Callable[[], None],
    ) -> None:
        super().__init__(master, **style.frame_kwargs)
        self.on_update = on_update
        self.on_req_img = on_req_img
        self.on_auto_req = on_auto_req

        self.node_colors: Dict[str, str] = {}
        for g in NEW_NODE_TYPES:
            for n in g.nodes:
                self.node_colors[n.name] = g.color

        self.top_bar = ctk.CTkFrame(
            self,
            border_color="black",
            border_width=2,
            corner_radius=style.corner_radius_medium,
        )
        self.top_bar.pack(fill="x", padx=style.pad_small, pady=style.pad_small)
        self.top_bar.columnconfigure(0, weight=1)
        self.details_label = ctk.CTkLabel(
            self.top_bar,
            text="Select a node",
            font=style.header_font,
            text_color=style.text_color,
            anchor="w",
        )
        self.details_label.grid(
            row=0, column=0, sticky="nswe", pady=style.pad_large, padx=style.pad_large
        )

        self.category_label = ctk.CTkLabel(self.top_bar, text="", **style.label_kwargs)
        self.category_label.grid(
            row=0, column=1, sticky="nswe", pady=style.pad_medium, padx=style.pad_medium
        )

        self.custom_fields_frame = ctk.CTkFrame(self, fg_color=style.frame_bg_col)
        self.custom_fields_frame.pack(pady=style.pad_medium, fill="both", expand=False)

        bottom_container = ctk.CTkFrame(self, fg_color=style.frame_bg_col)
        bottom_container.pack(fill="x", padx=style.pad_small, pady=style.pad_small)
        bottom_container.columnconfigure((0, 1), weight=1)

        self.update_button = ctk.CTkButton(
            bottom_container,
            text="Update Node",
            command=self.on_update,
            fg_color=style.primary_button_bg_color,
            hover_color=dimm_color(style.primary_button_bg_color),
            **style.button_kwargs,
        )
        self.update_button.grid(
            row=0, column=0, sticky="nswe", pady=style.pad_medium, padx=style.pad_small
        )

        self.gen_img_btn = ctk.CTkButton(
            bottom_container,
            text="Generate Image",
            command=self.on_req_img,
            fg_color=style.secondary_button_bg_color,
            hover_color=dimm_color(style.secondary_button_bg_color),
            **style.button_kwargs,
        )
        self.gen_img_btn.grid(
            row=0, column=1, sticky="nswe", pady=style.pad_medium, padx=style.pad_small
        )

        self.image_label = ctk.CTkLabel(self, text="", wraplength=400)
        self.image_label.pack(pady=style.pad_medium)

        self.custom_field_entries: Dict[str, MyEntry] = {}

    def show_details(self, node: Dict) -> None:
        """Displays details of the selected node in the config frame."""
        self.details_label.configure(text=f"Editing: {node.get('id:S', 'Unknown')}")
        node_type = node.get("type:S", "Unknown")
        self.category_label.configure(text=node_type)
        self.top_bar.configure(border_color=self.node_colors.get(node_type, "black"))
        self.clear_custom_fields()

        for field_name, value in node.items():
            if field_name == "typeS":
                continue
            splited_name = field_name.split(":")
            if len(splited_name) != 2:
                log.warn("len of field_name is not 2. field_name: " + field_name)
                continue
            name = splited_name[0]
            types = splited_name[1]

            field_frame = ctk.CTkFrame(
                self.custom_fields_frame, fg_color=style.frame_bg_col
            )
            field_frame.pack(pady=style.pad_small, fill="x")
            label = ctk.CTkLabel(
                field_frame, text=name, width=180, anchor="w", **style.label_kwargs
            )
            label.pack(side="left", padx=style.pad_large)

            entry = get_my_entry(
                types, master=field_frame, width=100, **style.entry_kwargs
            )
            entry.on_end_editing = self.on_update
            entry.pack(side="left", fill="x", expand=True)

            entry.insert(0, str(value))
            self.custom_field_entries[field_name] = entry

        self.image_label.configure(text="No image provided", image="")
        self.on_auto_req()

    def clear_custom_fields(self) -> None:
        """Removes all field entry widgets from the frame."""
        for widget in self.custom_fields_frame.winfo_children():
            widget.destroy()
        self.custom_field_entries.clear()
