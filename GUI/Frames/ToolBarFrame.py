from typing import Callable
import customtkinter as ctk
from style import TOOLBAR_BG_COL

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
        style = {"sticky": "nswe", "pady": 2, "padx": 2}
        save_btn = ctk.CTkButton(self, text="Save", command=on_save)
        save_btn.grid(row=0, column=0, **style)

        load_btn = ctk.CTkButton(self, text="Load", command=on_load)
        load_btn.grid(row=0, column=1, **style)

        export_btn = ctk.CTkButton(self, text="Export", command=on_export)
        export_btn.grid(row=0, column=2, **style)
