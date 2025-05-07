"""Avoid lint"""

import customtkinter as ctk
from style import FRAME_BG_COL, LABEL_KWARGS, ENTRY_KWARGS

class CTkLabledEntry:
    """An entry with a lable"""

    def __init__(
        self,
        master: ctk.CTkFrame,
        lable_name: str,
        width_entry: float,
        width_lable: float,
        padx: float,
        fill: str,
        expand: bool,
    ):
        self.field_frame = ctk.CTkFrame(master, fg_color=FRAME_BG_COL)
        # Stretch across the parent frame
        self.field_frame.pack(pady=2, fill=fill)

        self.label = ctk.CTkLabel(
            self.field_frame, text=lable_name, width=width_lable, anchor="w", **LABEL_KWARGS
        )
        # Some padding between label and entry
        self.label.pack(side="left", padx=padx)

        self.entry = ctk.CTkEntry(self.field_frame, width=width_entry, **ENTRY_KWARGS)

        # Let entry take extra space if needed
        self.entry.pack(side="left", fill="x", expand=expand)
