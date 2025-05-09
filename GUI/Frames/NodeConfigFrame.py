"""Avoid lint"""

from typing import Callable

import customtkinter as ctk

from custom_components.ctk_labled_entry import CTkLabledEntry
from nodes import NEW_NODE_TYPES
from style import (
    FRAME_KWARGS,
    TEXT_COLOR,
    PRIMARY_BUTTON_BG_COLOR,
    SECONDARY_BUTTON_BG_COLOR,
    HEADER_FONT,
    LABEL_KWARGS,
    FRAME_BG_COL
)


class NodeConfigFrame(ctk.CTkFrame):
    """Frame for configuring and editing a selected node."""

    def __init__(
        self,
        master,
        on_update: Callable[[], None],
        on_req_img: Callable[[], None],
    ) -> None:
        super().__init__(master, **FRAME_KWARGS)
        self.on_update = on_update
        self.on_req_img = on_req_img

        self.node_colors: dict[str, str] = {}
        for g in NEW_NODE_TYPES:
            for n in g.nodes:
                self.node_colors[n.name] = g.color

        self.top_bar = ctk.CTkFrame(self, border_color="black", border_width=2)
        self.top_bar.pack(fill="x", padx=4, pady=4)
        self.top_bar.columnconfigure(0, weight=1)
        self.details_label = ctk.CTkLabel(
            self.top_bar,
            text="Select a node",
            font=HEADER_FONT,
            text_color=TEXT_COLOR,
            anchor="w",
        )
        self.details_label.grid(row=0, column=0, sticky="nswe", pady=12, padx=8)

        self.category_label = ctk.CTkLabel(self.top_bar, text="", **LABEL_KWARGS)
        self.category_label.grid(row=0, column=1, sticky="nswe", pady=8, padx=8)

        self.custom_fields_frame = ctk.CTkFrame(self, fg_color=FRAME_BG_COL)
        self.custom_fields_frame.pack(pady=10, fill="both", expand=False)

        bottom_container = ctk.CTkFrame(self, fg_color=FRAME_BG_COL)
        bottom_container.pack(fill="x", padx=4, pady=4)
        bottom_container.columnconfigure((0, 1), weight=1)

        self.update_button = ctk.CTkButton(
            bottom_container,
            text="Update Node",
            command=self.on_update,
            text_color=TEXT_COLOR,
            fg_color=PRIMARY_BUTTON_BG_COLOR,
        )
        self.update_button.grid(row=0, column=0, sticky="nswe", pady=8, padx=4)

        self.gen_img_btn = ctk.CTkButton(
            bottom_container,
            text="Generate Image",
            command=self.on_req_img,
            text_color=TEXT_COLOR,
            fg_color=SECONDARY_BUTTON_BG_COLOR,
        )
        self.gen_img_btn.grid(row=0, column=1, sticky="nswe", pady=8, padx=4)

        self.image_label = ctk.CTkLabel(self, text="")
        self.image_label.pack(pady=10)

        self.custom_field_entries: dict[str, ctk.CTkEntry] = {}

    def show_details(self, node: dict) -> None:
        """Displays details of the selected node in the config frame."""
        self.details_label.configure(text=f"Editing: {node.get('idS', 'Unknown')}")
        node_type = node.get("typeS", "Unknown")
        self.category_label.configure(text=node_type)
        self.top_bar.configure(border_color=self.node_colors.get(node_type, "black"))
        self.clear_custom_fields()

        for field_name, value in node.items():
            if field_name == "typeS":
                continue
            labeled_entry = CTkLabledEntry(
                self.custom_fields_frame, field_name[:-1], 180, 100, 24, "x", True
            )
            labeled_entry.entry.insert(0, str(value))
            self.custom_field_entries[field_name] = labeled_entry.entry

        self.image_label.configure(text="No image provided", image="")

    def clear_custom_fields(self) -> None:
        """Removes all field entry widgets from the frame."""
        for widget in self.custom_fields_frame.winfo_children():
            widget.destroy()
        self.custom_field_entries.clear()
