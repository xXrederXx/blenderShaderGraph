"""Avoid lint"""

import customtkinter as ctk
from Frames.NodeApp import NodeAppMainFrame
from style import *

# App Settings
ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("blue")


def replace_ctkentry_text(entry: ctk.CTkEntry, text: str):
    """Replaces the text of an entry

    Args:
        entry (ctk.CTkEntry): The entry
        text (str): The text to write
    """
    entry.delete(0, "end")
    entry.insert(0, text)

class NodeApp(ctk.CTk):
    """Main application for managing nodes."""
    
    def __init__(self):
        super().__init__(fg_color=MAIN_BG_COL)
        self.title("Node Manager")
        self.geometry("1400x800")
        
        self.columnconfigure(0, weight=1)
        self.rowconfigure(0, weight=0)
        self.rowconfigure(1, weight=1)

        self.tool_bar = ctk.CTkFrame(self, fg_color=TOOLBAR_BG_COL, height=48, corner_radius=0)
        self.tool_bar.grid(row=0, column=0, sticky="nswe")

        self.main = ctk.CTkFrame(self, fg_color=MAIN_BG_COL, corner_radius=0)
        self.main.grid(
            row=1, column=0, sticky="nswe", padx=FRAME_GRID_PADX, pady=FRAME_GRID_PADY
        )
        self.main.columnconfigure(0, weight=1)
        self.main.rowconfigure(0, weight=1)

        self.app = NodeAppMainFrame(self.main)
        self.app.grid(sticky="nswe")
        
if __name__ == "__main__":
    root = NodeApp()
    root.mainloop()
