"""Avoid lint"""

from functools import partial
from typing import Callable, List

import customtkinter as ctk

from util.color_util import dimm_color
from nodes import NEW_NODE_TYPES


class NodeListFrame(ctk.CTkScrollableFrame):
    """Frame that displays a list of nodes as buttons."""

    def __init__(
        self,
        master,
        on_node_select: Callable[[int], None],
        on_node_pos_change: Callable[[int, int], None],
    ) -> None:
        super().__init__(master)
        self.on_node_select = on_node_select
        self.on_node_pos_change = on_node_pos_change
        self.node_buttons: List[ctk.CTkFrame] = []

        self.node_colors: dict[str, str] = {
            n.name: g.color for g in NEW_NODE_TYPES for n in g.nodes
        }

    def update_node_list(self, nodes: List[dict]) -> None:
        """Rebuilds the node list from scratch."""
        for btn in self.node_buttons:
            btn.destroy()
        self.node_buttons.clear()
        for idx, node in enumerate(nodes):
            print(idx)
            frame = ctk.CTkFrame(self)
            frame.pack(fill="x", pady=2)
            frame.columnconfigure(0, weight=1)

            btn = ctk.CTkButton(
                frame,
                text=node["idS"],
                fg_color=self.node_colors[node["typeS"]],
                hover_color=dimm_color(self.node_colors[node["typeS"]]),
                corner_radius=0,
                command=partial(self.on_node_select, idx),
            )
            btn.grid(row=0, column=0, sticky="nswe", pady=2)

            up_btn = ctk.CTkButton(
                frame,
                text="⬆",
                width=12,
                fg_color=self.node_colors[node["typeS"]],
                hover_color=dimm_color(self.node_colors[node["typeS"]]),
                corner_radius=0,
                command=partial(self.on_node_pos_change, idx, -1),
            )
            up_btn.grid(row=0, column=1)

            down_btn = ctk.CTkButton(
                frame,
                text="⬇",
                width=12,
                fg_color=self.node_colors[node["typeS"]],
                hover_color=dimm_color(self.node_colors[node["typeS"]]),
                corner_radius=0,
                command=partial(self.on_node_pos_change, idx, 1),
            )
            down_btn.grid(row=0, column=2)

            self.node_buttons.append(frame)
