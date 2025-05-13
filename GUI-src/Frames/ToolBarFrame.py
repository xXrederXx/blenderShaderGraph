from typing import Callable
import customtkinter as ctk
from style import TOOLBAR_BG_COL, LABEL_KWARGS, PAD_SMALL
from util.color_util import dimm_color

class ToolBarFrame(ctk.CTkFrame):
    """Frame that displays a list of nodes as buttons."""

    def __init__(
        self,
        master,
        on_save: Callable[[], None],
        on_load: Callable[[], None],
        on_export: Callable[[], None],
    ) -> None:
        super().__init__(master, corner_radius=0, fg_color=TOOLBAR_BG_COL)
        grid_props = {"sticky": "nswe", "padx": PAD_SMALL}
        save_btn = ctk.CTkButton(
            self,
            text="Save",
            width=60,
            fg_color=TOOLBAR_BG_COL,
            hover_color=dimm_color(TOOLBAR_BG_COL),
            command=on_save,
            corner_radius=0,
            **LABEL_KWARGS
        )
        save_btn.grid(row=0, column=0, **grid_props)

        load_btn = ctk.CTkButton(
            self,
            text="Load",
            width=60,
            fg_color=TOOLBAR_BG_COL,
            hover_color=dimm_color(TOOLBAR_BG_COL),
            command=on_load,
            corner_radius=0,
            **LABEL_KWARGS
        )
        load_btn.grid(row=0, column=1, **grid_props)

        export_btn = ctk.CTkButton(
            self,
            text="Export",
            width=70,
            fg_color=TOOLBAR_BG_COL,
            hover_color=dimm_color(TOOLBAR_BG_COL),
            command=on_export,
            corner_radius=0,
            **LABEL_KWARGS
        )
        export_btn.grid(row=0, column=2, **grid_props)
