"""Avoid lint"""

from typing import Any, Dict, List
from typing import Optional

import customtkinter as ctk

from Frames.AddNodeFrame import AddNodeFrame
from Frames.NodeConfigFrame import NodeConfigFrame
from Frames.NodeListFrame import NodeListFrame
from util.node_util import change_node_pos, convert_value
from util.io_util import request_image_async, get_from_tmp
from globals.style import FRAME_GRID_KWARGS, MAIN_BG_COL
from globals.my_logger import logger as log


class NodeAppMainFrame(ctk.CTkFrame):
    """Main application for managing nodes."""

    def __init__(self, master) -> None:
        super().__init__(master, fg_color=MAIN_BG_COL)
        self.nodes: List[Dict[str, Any]] = []
        self.selected_node_index: Optional[int] = None

        self._setup_layout()
        self._create_frames()

    def _setup_layout(self) -> None:
        self.grid_columnconfigure(0, weight=1)
        self.grid_columnconfigure((1, 2), weight=2)
        self.grid_rowconfigure(0, weight=1)
        self.grid_propagate(False)

    def _create_frames(self) -> None:
        self.node_list_frame = NodeListFrame(
            self,
            on_node_select=self.show_details,
            on_node_pos_change=self.change_node_pos,
            on_delete_node=self.delete_node,
        )
        self.node_list_frame.grid(row=0, column=0, **FRAME_GRID_KWARGS)

        self.config_frame = NodeConfigFrame(
            self,
            on_update=self.update_node,
            on_req_img=self.request_node_image,
            on_auto_req=self.request_from_tmp,
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
        change_node_pos(self.nodes, idx, offset)
        self.node_list_frame.update_node_list(self.nodes)

    def add_node(self) -> None:
        """Adds a new node to the list."""
        node = self.add_node_frame.get_node()
        if not node:
            log.info("No node to be added")
            return
        self.nodes.append(node)
        self.node_list_frame.update_node_list(self.nodes)

    def show_details(self, idx: int) -> None:
        """Display details of selected node."""
        self.selected_node_index = idx
        selected_node = self.nodes[idx]
        self.config_frame.show_details(selected_node)

    def delete_node(self, idx: int):
        """Deletes the nod at the spesifyed index and updates the node_list"""
        self.nodes.pop(idx)
        self.node_list_frame.update_node_list(self.nodes)

    def update_node(self) -> None:
        """Update the currently selected node from config fields."""
        if self.selected_node_index is None:
            log.info("No node selected")
            return

        node = self.nodes[self.selected_node_index]
        for field_name, entry in self.config_frame.custom_field_entries.items():
            value = entry.get_value()
            try:
                node[field_name] = convert_value(field_name, value)
            except ValueError:
                log.error(f"Invalid input for field {field_name}: {value}")
            except TypeError:
                log.error(
                    f"No matching conversion found for field {field_name}: {value}"
                )

        self.node_list_frame.update_node_list(self.nodes)

    def update_node_type_menu(self, selected_group: str) -> None:
        """Update node type dropdown and color on group change."""
        self.add_node_frame.update_node_type_menu(selected_group)

    def generate_image(self) -> None:
        """Stub for generating an image."""
        self.update_node()
        request_image_async(self.add_node_frame.generated_image, self.nodes)

    def request_node_image(self) -> None:
        """Requests an image for the currently selected node"""
        self.update_node()
        nodes = self.nodes[0 : self.selected_node_index + 1]
        request_image_async(self.config_frame.image_label, nodes)

    def request_from_tmp(self) -> None:
        """Auto request image from tmp cache"""
        nodes = self.nodes[0 : self.selected_node_index + 1]
        get_from_tmp(nodes, self.config_frame.image_label)
