from style import (
    FRAME_GRID_KWARGS,
    FRAME_GRID_PADX,
    FRAME_GRID_PADY,
    MAIN_BG_COL,
    TOOLBAR_BG_COL,
    init_fonts,
)
from Frames.NodeAppMainFrame import NodeAppMainFrame


import customtkinter as ctk


class NodeApp(ctk.CTk):
    """Main application for managing nodes."""

    def __init__(self):
        super().__init__()
        self.title("Node Manager")
        self.geometry("1400x800")

        init_fonts()

        self.columnconfigure(0, weight=1)
        self.rowconfigure(0, weight=0)
        self.rowconfigure(1, weight=1)

        self.tool_bar = ctk.CTkFrame(
            self, fg_color=TOOLBAR_BG_COL, height=48, corner_radius=0
        )
        self.tool_bar.grid(row=0, column=0, sticky="nswe")

        self.main = ctk.CTkFrame(self, fg_color=MAIN_BG_COL, corner_radius=0)
        self.main.grid(
            row=1, column=0, **FRAME_GRID_KWARGS
        )
        self.main.columnconfigure(0, weight=1)
        self.main.rowconfigure(0, weight=1)
        self.app = NodeAppMainFrame(self.main)
        self.app.grid(sticky="nswe")
