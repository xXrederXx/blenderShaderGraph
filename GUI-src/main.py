"""Avoid lint"""

import customtkinter as ctk
from NodeApp import NodeApp

# App Settings
ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("dark-blue")

if __name__ == "__main__":
    root = NodeApp()
    root.mainloop()
