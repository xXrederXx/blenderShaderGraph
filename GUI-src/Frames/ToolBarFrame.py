from typing import Callable
import customtkinter as ctk
from style import TOOLBAR_BG_COL, LABEL_KWARGS, PAD_SMALL
from util.color_util import dimm_color
from custom_components.tool_dropdown import ToolDropdown
from log import logger as log


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
        super().__init__(master, corner_radius=0, fg_color=TOOLBAR_BG_COL)
        grid_props = {"sticky": "nswe", "padx": PAD_SMALL}

        file_btn = ctk.CTkButton(
            self,
            text="File",
            width=70,
            fg_color=TOOLBAR_BG_COL,
            hover_color=dimm_color(TOOLBAR_BG_COL),
            command=on_export,
            corner_radius=0,
            **LABEL_KWARGS
        )
        file_btn.grid(row=0, column=0, **grid_props)

        ToolDropdown(
            self,
            {
                "New Project": on_new_project,
                "Load Project": on_load,
                "Save Project": on_save,
                "Export Project": on_export,
            },
            300,
            file_btn,
        )

        debug_btn = ctk.CTkButton(
            self,
            text="Debug",
            width=70,
            fg_color=TOOLBAR_BG_COL,
            hover_color=dimm_color(TOOLBAR_BG_COL),
            command=on_export,
            corner_radius=0,
            **LABEL_KWARGS
        )
        debug_btn.grid(row=0, column=1, **grid_props)

        ToolDropdown(
            self,
            {
                "Toggle Console Logging": lambda: log.set_logger_output_console(
                    not log.is_logging_to_console
                ),
                "Set Log-LvL: DEBUG": lambda: log.log.setLevel(10),
                "Set Log-LvL: INFO": lambda: log.log.setLevel(20),
                "Set Log-LvL: WARN": lambda: log.log.setLevel(30),
                "Set Log-LvL: ERROR": lambda: log.log.setLevel(40),
                "Set Log-LvL: CRITICAL": lambda: log.log.setLevel(50),
            },
            300,
            debug_btn,
        )
