"""Avoid lint"""

from typing import Any, Callable, Dict, List

import customtkinter as ctk

from globals.nodes import NEW_NODE_TYPES
from globals.style import (
    FRAME_KWARGS,
    MAIN_BG_COL,
    FRAME_GRID_KWARGS,
    PRIMARY_BUTTON_BG_COLOR,
    DROPDOWN_KWARGS,
    ENTRY_KWARGS,
    LABEL_KWARGS,
    PAD_SMALL,
    BUTTON_KWARKS,
    PAD_LARGE,
    HEADER_FONT,
    TEXT_COLOR,
)
from globals.my_logger import logger as log


class AddNodeFrame(ctk.CTkFrame):
    """Frame for adding a new node."""

    def __init__(
        self,
        master,
        on_add_node: Callable[[], None],
        on_generate_image: Callable[[], None],
        on_group_change: Callable[[str], None],
    ) -> None:
        super().__init__(master, fg_color=MAIN_BG_COL)
        self.on_add_node = on_add_node
        self.on_group_change = on_group_change
        self.on_generate_image = on_generate_image

        self.node_groups: List[str] = [group.name for group in NEW_NODE_TYPES]
        self.node_groups_colors: Dict[str, str] = {
            group.name: group.color for group in NEW_NODE_TYPES
        }
        self.node_types_Dict: Dict[str, List[str]] = {
            group.name: [node.name for node in group.nodes] for group in NEW_NODE_TYPES
        }

        self.rowconfigure((0, 1), weight=1)
        self.columnconfigure(0, weight=1)

        self._create_add_node_section()
        self._create_image_section()

    def _create_add_node_section(self) -> None:
        add_node_frame = ctk.CTkFrame(self, **FRAME_KWARGS)
        add_node_frame.grid(row=0, column=0, **FRAME_GRID_KWARGS)

        ctk.CTkLabel(
            add_node_frame, text="Add New Node", text_color=TEXT_COLOR, font=HEADER_FONT
        ).pack(pady=PAD_LARGE)

        label_pack_props = {"pady": PAD_SMALL, "padx": PAD_LARGE, "fill": "x"}
        self.node_groups_var = ctk.StringVar(value=self.node_groups[0])
        self.node_groups_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_groups_var,
            values=self.node_groups,
            command=self._on_group_change_wrapper,
            **DROPDOWN_KWARGS
        )
        self.node_groups_menu.pack(**label_pack_props)

        initial_group = self.node_groups_var.get()
        self.node_type_var = ctk.StringVar(value=self.node_types_Dict[initial_group][0])
        self.node_type_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_type_var,
            values=self.node_types_Dict[initial_group],
            **DROPDOWN_KWARGS
        )
        self.node_type_menu.pack(**label_pack_props)

        self.new_id_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Id", **ENTRY_KWARGS
        )
        self.new_id_entry.pack(**label_pack_props)

        self.new_desc_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Description", **ENTRY_KWARGS
        )
        self.new_desc_entry.pack(**label_pack_props)

        self.add_button = ctk.CTkButton(
            add_node_frame,
            text="Add Node",
            command=self.on_add_node,
            fg_color=PRIMARY_BUTTON_BG_COLOR,
            **BUTTON_KWARKS
        )
        self.add_button.pack(pady=PAD_LARGE, padx=PAD_LARGE, fill="x")

        self.update_node_type_menu(initial_group)

    def _create_image_section(self) -> None:
        image_display_frame = ctk.CTkFrame(self, **FRAME_KWARGS)
        image_display_frame.grid(row=1, column=0, **FRAME_GRID_KWARGS)

        generate_image_button = ctk.CTkButton(
            image_display_frame,
            text="Generate Image",
            command=self.on_generate_image,
            **BUTTON_KWARKS
        )
        generate_image_button.pack(fill="x", padx=PAD_LARGE, pady=PAD_LARGE)

        self.generated_image = ctk.CTkLabel(
            image_display_frame,
            text="No Image Generated",
            wraplength=400,
            image=None,
            **LABEL_KWARGS
        )
        self.generated_image.pack(
            fill="both", padx=PAD_LARGE, pady=PAD_LARGE, expand=True
        )

    def _on_group_change_wrapper(self, selected_group: str) -> None:
        self.update_node_type_menu(selected_group)
        self.on_group_change(selected_group)

    def update_node_type_menu(self, selected_group: str) -> None:
        """Update the node types shown in the dropdown."""
        new_types = self.node_types_Dict[selected_group]
        self.node_type_menu.configure(values=new_types)
        self.node_type_var.set(new_types[0])
        self.update_menu_color(selected_group)

    def update_menu_color(self, selected_group: str):
        """Update the color of the node group menu."""
        color = self.node_groups_colors[selected_group]
        self.node_groups_menu.configure(fg_color=color)

    def get_node(self) -> Dict[str, Any]:
        """returns a node Dict

        Returns:
            Dict[str, Any]: node
        """
        name = self.new_id_entry.get()
        description = self.new_desc_entry.get()
        selected_type_name = self.node_type_var.get()
        selected_group_name = self.node_groups_var.get()
        if not name or not selected_type_name:
            log.info("Cant generate a node with an empty name or type")
            return {}

        node_group = next(
            (g for g in NEW_NODE_TYPES if g.name == selected_group_name), None
        )
        if not node_group:
            log.warn("Cant find the node group in which the node type lives ")
            return {}

        node_type = next(
            (nt for nt in node_group.nodes if nt.name == selected_type_name), None
        )
        if not node_type:
            log.warn("Cant find the node type in the node group")
            return {}

        new_node = dict(node_type.params)
        new_node["id:S"] = name
        new_node["description:S"] = description
        new_node["type:S"] = selected_type_name

        self.new_id_entry.delete(0, "end")
        self.new_desc_entry.delete(0, "end")

        return new_node
