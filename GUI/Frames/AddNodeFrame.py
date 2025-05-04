"""Avoid lint"""

from typing import Callable, List

import customtkinter as ctk

from nodes import NEW_NODE_TYPES

class AddNodeFrame(ctk.CTkFrame):
    """Frame for adding a new node."""

    def __init__(
        self,
        master,
        on_add_node: Callable[[], None],
        on_generate_image: Callable[[], None],
        on_group_change: Callable[[str], None],
    ) -> None:
        super().__init__(master, corner_radius=0)
        self.on_add_node = on_add_node
        self.on_group_change = on_group_change
        self.on_generate_image = on_generate_image

        self.node_groups: List[str] = [group.name for group in NEW_NODE_TYPES]
        self.node_groups_colors: dict[str, str] = {
            group.name: group.color for group in NEW_NODE_TYPES
        }
        self.node_types_dict: dict[str, List[str]] = {
            group.name: [node.name for node in group.nodes] for group in NEW_NODE_TYPES
        }

        self.rowconfigure((0, 1), weight=1)
        self.columnconfigure(0, weight=1)

        self._create_add_node_section()
        self._create_image_section()

    def _create_add_node_section(self) -> None:
        add_node_frame = ctk.CTkFrame(self)
        add_node_frame.grid(row=0, column=0, sticky="nswe", padx=10, pady=10)

        ctk.CTkLabel(
            add_node_frame,
            text="Add New Node",
            font=ctk.CTkFont(size=20, weight="bold"),
        ).pack(pady=10)

        self.node_groups_var = ctk.StringVar(value=self.node_groups[0])
        self.node_groups_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_groups_var,
            values=self.node_groups,
            command=self._on_group_change_wrapper,
        )
        self.node_groups_menu.pack(pady=5, padx=24, fill="x")

        initial_group = self.node_groups_var.get()
        self.node_type_var = ctk.StringVar(value=self.node_types_dict[initial_group][0])
        self.node_type_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_type_var,
            values=self.node_types_dict[initial_group],
        )
        self.node_type_menu.pack(pady=5, padx=24, fill="x")

        self.new_id_entry = ctk.CTkEntry(add_node_frame, placeholder_text="Id")
        self.new_id_entry.pack(pady=5, padx=24, fill="x")

        self.new_desc_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Description"
        )
        self.new_desc_entry.pack(pady=5, padx=24, fill="x")

        self.add_button = ctk.CTkButton(
            add_node_frame, text="Add Node", command=self.on_add_node
        )
        self.add_button.pack(pady=20, padx=24, fill="x")

        self.update_node_type_menu(initial_group)

    def _create_image_section(self) -> None:
        image_display_frame = ctk.CTkFrame(self)
        image_display_frame.grid(row=1, column=0, sticky="nswe", padx=10, pady=10)

        generate_image_button = ctk.CTkButton(
            image_display_frame, text="Generate Image", command=self.on_generate_image
        )
        generate_image_button.pack(fill="x", padx=8, pady=8)

        self.generated_image = ctk.CTkLabel(
            image_display_frame, text="No Image Generated", wraplength=400, image=None
        )
        self.generated_image.pack(fill="both", padx=8, pady=8, expand=True)

    def _on_group_change_wrapper(self, selected_group: str) -> None:
        self.update_node_type_menu(selected_group)
        self.on_group_change(selected_group)

    def update_node_type_menu(self, selected_group: str) -> None:
        """Update the node types shown in the dropdown."""
        new_types = self.node_types_dict[selected_group]
        self.node_type_menu.configure(values=new_types)
        self.node_type_var.set(new_types[0])
        self.update_menu_color(selected_group)

    def update_menu_color(self, selected_group: str):
        """Update the color of the node group menu."""
        color = self.node_groups_colors[selected_group]
        self.node_groups_menu.configure(fg_color=color)