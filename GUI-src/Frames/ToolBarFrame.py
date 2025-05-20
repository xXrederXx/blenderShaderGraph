"""This contains the Toolbar"""

import os
from typing import Callable
import customtkinter as ctk
from globals.style import StyleManager
from util.color_util import dimm_color
from custom_components.tool_dropdown import BtnToolDropdown
from globals.my_logger import MyLogger
from globals.paths import PERSISTANT_PATH, TMP_PATH
from custom_components.char_lim_entry import CharLimEntry


log = MyLogger()
style = StyleManager.get()


class ToolBarFrame(ctk.CTkFrame):
    """Frame that displays a list of nodes as buttons."""

    def __init__(
        self,
        master,
        on_save: Callable[[], None],
        on_load: Callable[[], None],
        on_export: Callable[[], None],
        on_new_project: Callable[[], None],
    ) -> None:
        super().__init__(master, corner_radius=0, fg_color=style.toolbar_bg_col)
        self.columnconfigure(10, weight=100)
        grid_props = {"sticky": "nswe", "padx": style.pad_small}

        file_btn = ctk.CTkButton(
            self,
            text="File",
            width=70,
            fg_color=style.toolbar_bg_col,
            hover_color=dimm_color(style.toolbar_bg_col),
            command=on_export,
            corner_radius=0,
            **style.label_kwargs
        )
        file_btn.grid(row=0, column=0, **grid_props)

        BtnToolDropdown(
            self,
            file_btn,
            {
                "New Project": on_new_project,
                "1-line-": None,
                "Load Project": on_load,
                "Save Project": on_save,
                "2-line-": None,
                "Export Project": on_export,
            },
        )

        debug_btn = ctk.CTkButton(
            self,
            text="Debug",
            width=70,
            fg_color=style.toolbar_bg_col,
            hover_color=dimm_color(style.toolbar_bg_col),
            command=on_export,
            corner_radius=0,
            **style.label_kwargs
        )
        debug_btn.grid(row=0, column=1, **grid_props)

        BtnToolDropdown(
            self,
            debug_btn,
            {
                "Toggle Console Logging": self.toogle_console_out,
                "1-line-": None,
                "Set Log-LvL: DEBUG": lambda: log.log.setLevel(10),
                "Set Log-LvL: INFO": lambda: log.log.setLevel(20),
                "Set Log-LvL: WARN": lambda: log.log.setLevel(30),
                "Set Log-LvL: ERROR": lambda: log.log.setLevel(40),
                "Set Log-LvL: CRITICAL": lambda: log.log.setLevel(50),
                "2-line-": None,
                "Open tmp Folder": lambda: os.startfile(os.path.realpath(TMP_PATH)),
                "Open persistant Folder": lambda: os.startfile(
                    os.path.realpath(PERSISTANT_PATH)
                ),
            },
        )

        self.proj_name_entry = CharLimEntry(
            self,
            max_chars=18,
            border_width=0,
            bg_color="transparent",
            fg_color="transparent",
            placeholder_text="Project Name",
            **style.label_kwargs
        )
        self.proj_name_entry.grid(row=0, column=10, sticky="e", padx=style.pad_small)

    def toogle_console_out(self, btn: ctk.CTkButton) -> None:
        """Toggles the log.is_logging_to_console and changes the button text"""
        log.set_logger_output_console(not log.is_logging_to_console)
        if log.is_logging_to_console:
            btn.configure(text="Disable Console Logging")
        else:
            btn.configure(text="Enable Console Logging")
