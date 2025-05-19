import tkinter
import customtkinter as ctk


class CharLimEntry(ctk.CTkEntry):
    """Creates an entry with a max char limit"""
    def __init__(
        self,
        master,
        max_chars: int,
        width=140,
        height=28,
        corner_radius=None,
        border_width=None,
        bg_color="transparent",
        fg_color=None,
        border_color=None,
        text_color=None,
        placeholder_text_color=None,
        placeholder_text=None,
        font=None,
        state=tkinter.NORMAL,
        **kwargs
    ):
        self.textvariable = ctk.StringVar()
        self.max_chars = max_chars
        super().__init__(
            master,
            width,
            height,
            corner_radius,
            border_width,
            bg_color,
            fg_color,
            border_color,
            text_color,
            placeholder_text_color,
            self.textvariable,
            placeholder_text,
            font,
            state,
            **kwargs
        )

        self.textvariable.trace_add("write", lambda *args: self.check_entry())

    def check_entry(self):
        """Checks entry if over lim"""
        text = self.textvariable.get()
        if len(text) > self.max_chars:
            self.textvariable.set(text[: self.max_chars])
