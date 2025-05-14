import customtkinter as ctk
import tkinter as tk
from log import logger as log
from style import FRAME_BG_COL, LABEL_KWARGS, PAD_SMALL, PAD_MEDIUM, CORNER_RADIUS_SMALL, PRIMARY_BUTTON_BG_COLOR
from typing import Callable
from util.color_util import dimm_color

class ToolDropdown:
    """
    A dropdown-style tool menu similar to those found in many applications.
    Clicking the toggle button shows a floating frame of tools; clicking outside hides it.
    """

    def __init__(
        self,
        master: ctk.CTkFrame,
        tool_buttons: dict[str, Callable[[], None]],
        width: int,
        button: ctk.CTkButton,
    ):
        self.master = master
        self.root = master.winfo_toplevel()
        self.tool_buttons = tool_buttons
        self.dropdown_visible = False
        self.width = width

        # Main toggle button inside the small container
        self.toggle_button = button
        self.toggle_button.configure(command=self.toggle_dropdown)

        # Dropdown menu as a toplevel floating frame
        self.dropdown_window = tk.Toplevel(self.root)
        self.dropdown_window.overrideredirect(True)  # No titlebar or borders
        self.dropdown_window.withdraw()  # Start hidden
        transparent_color = "#010101"
        self.dropdown_window.configure(bg=transparent_color)
        self.dropdown_window.wm_attributes("-transparentcolor", transparent_color)

        # CTk-compatible frame inside Toplevel
        self.dropdown_frame = ctk.CTkFrame(
            self.dropdown_window,
            corner_radius=CORNER_RADIUS_SMALL,
            bg_color=FRAME_BG_COL,
        )
        self.dropdown_frame.pack(fill="both", ipady=PAD_SMALL)

        self.root.bind("<Configure>", self._on_click_outside, add=True)
        self.root.bind("<Button-1>", self._on_click_outside, add=True)

        self._populate_dropdown()

    def _populate_dropdown(self):
        """Fill the dropdown with tool buttons."""
        for widget in self.dropdown_frame.winfo_children():
            widget.destroy()

        for label, command in self.tool_buttons.items():
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
        self.dropdown_window.geometry(f"{self.width}x600+{bx}+{by + bh}")
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
            if (
                widget == self.dropdown_window
                or widget == self.dropdown_frame
                or widget == self.toggle_button
            ):
                return True
            widget = widget.master
        return False

    def _run_and_close(self, command):
        """Run the button command and close the dropdown."""
        command()
        self.hide_dropdown()
