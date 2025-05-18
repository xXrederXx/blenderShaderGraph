"""Avoid lint"""

from functools import partial
from typing import Callable, Dict, List

import customtkinter as ctk

from util.color_util import dimm_color
from nodes import NEW_NODE_TYPES

from style import (
    FRAME_KWARGS,
    PAD_SMALL,
    CORNER_RADIUS_MEDIUM,
    LABEL_KWARGS,
    TEXT_FONT,
    FRAME_BG_COL,
)


class NodeListFrame(ctk.CTkScrollableFrame):
    """Frame that displays a list of nodes as buttons."""

    def __init__(
        self,
        master,
        on_node_select: Callable[[int], None],
        on_node_pos_change: Callable[[int, int], None],
        on_delete_node: Callable[[int], None],
    ) -> None:
        super().__init__(master, **FRAME_KWARGS)
        self.on_node_select = on_node_select
        self.on_node_pos_change = on_node_pos_change
        self.on_delete_node = on_delete_node
        self.node_buttons: List[ctk.CTkFrame] = []

        self.node_colors: Dict[str, str] = {
            n.name: g.color for g in NEW_NODE_TYPES for n in g.nodes
        }

    def update_node_list(self, nodes: List[Dict]) -> None:
        """Rebuilds the node list from scratch."""
        for btn in self.node_buttons:
            btn.destroy()
        self.node_buttons.clear()
        for idx, node in enumerate(nodes):
            frame = ctk.CTkFrame(
                self, corner_radius=CORNER_RADIUS_MEDIUM, fg_color="transparent"
            )
            frame.pack(fill="x", pady=PAD_SMALL)
            frame.columnconfigure(0, weight=1)

            btn = ctk.CTkButton(
                frame,
                text=node["id:S"],
                fg_color=self.node_colors[node["type:S"]],
                hover_color=dimm_color(self.node_colors[node["type:S"]]),
                command=partial(self.on_node_select, idx),
                corner_radius=CORNER_RADIUS_MEDIUM,
                **LABEL_KWARGS
            )
            btn.grid(row=0, column=0, sticky="nswe", pady=PAD_SMALL)

            up_btn = ctk.CTkButton(
                frame,
                text="⬆",
                width=12,
                fg_color=FRAME_BG_COL,
                hover_color=dimm_color(FRAME_BG_COL),
                corner_radius=0,
                command=partial(self.on_node_pos_change, idx, -1),
                **LABEL_KWARGS
            )
            up_btn.grid(row=0, column=1)

            down_btn = ctk.CTkButton(
                frame,
                text="⬇",
                width=12,
                fg_color=FRAME_BG_COL,
                hover_color=dimm_color(FRAME_BG_COL),
                corner_radius=0,
                command=partial(self.on_node_pos_change, idx, 1),
                **LABEL_KWARGS
            )
            down_btn.grid(row=0, column=2)

            delete_btn = ctk.CTkButton(
                frame,
                text="x",
                width=12,
                fg_color=FRAME_BG_COL,
                hover_color=dimm_color(FRAME_BG_COL),
                corner_radius=0,
                command=partial(self.on_delete_node, idx),
                text_color="red",
                font=TEXT_FONT,
            )
            delete_btn.grid(row=0, column=3)

            self.node_buttons.append(frame)
