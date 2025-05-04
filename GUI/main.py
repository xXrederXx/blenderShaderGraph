"""Avoid lint"""

import customtkinter as ctk
from Frames.NodeApp import NodeApp

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


if __name__ == "__main__":
    app = NodeApp()
    app.mainloop()
