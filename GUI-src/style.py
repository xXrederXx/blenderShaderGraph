"""Contains styling"""

from customtkinter import CTkFont

FRAME_CORNER_RAD = 8
FRAME_BG_COL = "#27282d"
FRAME_KWARGS = {"corner_radius": FRAME_CORNER_RAD, "fg_color": FRAME_BG_COL}
FRAME_GRID_PADX = 4
FRAME_GRID_PADY = 4
FRAME_GRID_KWARGS = {"padx": FRAME_GRID_PADX, "pady": FRAME_GRID_PADY, "sticky": "nswe"}

MAIN_BG_COL = "#1e1f22"

TOOLBAR_BG_COL = "#2f3236"

PRIMARY_BUTTON_BG_COLOR = "#224567"
SECONDARY_BUTTON_BG_COLOR = "#393a3e"

TEXT_COLOR = "#f9fdfe"
TEXT_FONT = None
HEADER_FONT = None

def init_fonts():
    global TEXT_FONT
    global HEADER_FONT
    TEXT_FONT = CTkFont("Cascadia Mono", 11, "normal", "roman", False, False)
    HEADER_FONT = CTkFont("Cascadia Mono SemiBold", 20, "normal", "roman", False, False)



DROPDOWN_KWARGS = {
    "text_color": TEXT_COLOR,
    "fg_color": SECONDARY_BUTTON_BG_COLOR,
    "button_color": SECONDARY_BUTTON_BG_COLOR,
    "font": TEXT_FONT,
}

ENTRY_KWARGS = {
    "fg_color": SECONDARY_BUTTON_BG_COLOR,
    "text_color": TEXT_COLOR,
    "border_width": 1,
    "font": TEXT_FONT,
}

LABEL_KWARGS = {
    "font": TEXT_FONT,
    "text_color": TEXT_COLOR
}