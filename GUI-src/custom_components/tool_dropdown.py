import customtkinter as ctk
import tkinter as tk
from log import logger as log
from style import (
    FRAME_BG_COL,
    LABEL_KWARGS,
    PAD_SMALL,
    PAD_MEDIUM,
    CORNER_RADIUS_SMALL,
    PRIMARY_BUTTON_BG_COLOR,
)
from typing import Callable
from util.color_util import dimm_color


class ToolDropdownBase:
    """
    A dropdown-style tool menu similar to those found in many applications.
    Clicking the toggle button shows a floating frame of tools; clicking outside hides it.
    """

    def __init__(
        self,
        master: ctk.CTkFrame,
        button: ctk.CTkButton,
        transparent_color: str,
        window_width: int,
        window_height: int,
    ):
        self._master = master
        self._root = master.winfo_toplevel()
        self.window_width = window_width
        self.window_height = window_height
        self.dropdown_visible = False

        # Main toggle button inside the small container
        self.toggle_button = button
        self.toggle_button.configure(command=self.toggle_dropdown)

        # Dropdown menu as a toplevel floating frame
        self.dropdown_window = tk.Toplevel(self._root)
        self.dropdown_window.overrideredirect(True)  # No titlebar or borders
        self.dropdown_window.withdraw()  # Start hidden
        self.dropdown_window.configure(bg=transparent_color)
        self.dropdown_window.wm_attributes("-transparentcolor", transparent_color)

        self._root.bind("<Configure>", self._on_click_outside, add=True)
        self._root.bind("<Button-1>", self._on_click_outside, add=True)

    def toggle_dropdown(self):
        if self.dropdown_visible:
            self.hide_dropdown()
        else:
            self.show_dropdown()

    def show_dropdown(self):
        """Show the dropdown just below the toggle button."""
        self.dropdown_visible = True

        # Get screen position of the button
        bx = self.toggle_button.winfo_rootx()
        by = self.toggle_button.winfo_rooty()
        bh = self.toggle_button.winfo_height()

        # Position dropdown just under the button
        self.dropdown_window.geometry(
            f"{self.window_width}x{self.window_height}+{bx}+{by + bh}"
        )
        self.dropdown_window.deiconify()
        log.debug("Dropdown shown")

    def hide_dropdown(self):
        """Hide the dropdown."""
        self.dropdown_visible = False
        self.dropdown_window.withdraw()
        log.debug("Dropdown hidden")

    def _on_click_outside(self, event):
        if self.dropdown_visible:
            if not self._clicked_inside_dropdown(event.widget):
                self.hide_dropdown()

    def _clicked_inside_dropdown(self, widget):
        """Check if click was inside dropdown window."""
        while widget:
            if widget == self.dropdown_window or widget == self.toggle_button:
                return True
            widget = widget.master
        return False

    def _run_and_close(self, command):
        """Run the button command and close the dropdown."""
        command()
        self.hide_dropdown()


class BtnToolDropdown(ToolDropdownBase):
    """
    Just like dropdown base with easyer usage. By default every entry in the tool_buttons
    will be converted to a button. But there are some special types. These types are:
        - "-line-" -> reults in a line
    To use multiple just add a prefix or a suffix
    """

    def __init__(
        self,
        master: ctk.CTkFrame,
        button: ctk.CTkButton,
        tool_buttons: dict[str, Callable[[], None]],
        transparent_color: str = "#010101",
        window_width: int = 300,
        window_height: int = 600,
    ):
        super().__init__(master, button, transparent_color, window_width, window_height)
        self.tool_buttons = tool_buttons

        # CTk-compatible frame inside Toplevel
        self.dropdown_frame = ctk.CTkFrame(
            self.dropdown_window,
            corner_radius=CORNER_RADIUS_SMALL,
            bg_color=FRAME_BG_COL,
        )
        self.dropdown_frame.pack(fill="both", ipady=PAD_SMALL)

        self._populate_dropdown()

    def _populate_dropdown(self):
        """Fill the dropdown with tool buttons."""
        for widget in self.dropdown_frame.winfo_children():
            widget.destroy()

        for label, command in self.tool_buttons.items():
            if label.find("-line-") != -1: 
                self._make_line()
            else:
                self._make_button(label, command)

    def _make_button(self, label, command):
        btn = ctk.CTkButton(
            self.dropdown_frame,
            text=label,
            command=lambda cmd=command: self._run_and_close(cmd),
            fg_color="transparent",
            hover_color=dimm_color(PRIMARY_BUTTON_BG_COLOR),
            anchor="w",
            **LABEL_KWARGS,
        )
        btn.pack(fill="x", padx=PAD_MEDIUM, pady=PAD_SMALL)
        
    def _make_line(self):
        line = ctk.CTkFrame(
            self.dropdown_frame,
            fg_color=PRIMARY_BUTTON_BG_COLOR,
            height=2
        )
        line.pack(fill="x", padx=PAD_MEDIUM)
