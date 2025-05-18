"""Contains styling information"""

from typing import Union, Tuple
from customtkinter import CTkFont
from util.color_util import dimm_color
from globals.my_logger import logger as log

FRAME_BG_COL = "#27282d"
FRAME_GRID_PADX = 4
FRAME_GRID_PADY = 4
FRAME_GRID_KWARGS = {"padx": FRAME_GRID_PADX, "pady": FRAME_GRID_PADY, "sticky": "nswe"}

MAIN_BG_COL = "#1e1f22"

TOOLBAR_BG_COL = "#2f3236"

PRIMARY_BUTTON_BG_COLOR = "#224567"
SECONDARY_BUTTON_BG_COLOR = "#393a3e"

TEXT_COLOR = "#f9fdfe"
_FONT_NAME = "Inter"
TEXT_FONT: Union[Tuple[str, int], CTkFont] = (_FONT_NAME, 13)
HEADER_FONT: Union[Tuple[str, int], CTkFont] = (_FONT_NAME, 20, "bold")


def init_fonts():
    """This replaces the Tuple font type with a propper CTKFont"""
    log.debug("init proper CTkFonts")
    global TEXT_FONT
    global HEADER_FONT
    TEXT_FONT = CTkFont(_FONT_NAME, 13, "normal", "roman", False, False)
    HEADER_FONT = CTkFont(_FONT_NAME, 20, "normal", "roman", False, False)


CORNER_RADIUS_SMALL = 8
CORNER_RADIUS_MEDIUM = 16
CORNER_RADIUS_LARGE = 32

PAD_SMALL = 4
PAD_MEDIUM = 8
PAD_LARGE = 16

FRAME_KWARGS = {"corner_radius": CORNER_RADIUS_MEDIUM, "fg_color": FRAME_BG_COL}

LABEL_KWARGS = {"font": TEXT_FONT, "text_color": TEXT_COLOR}

DROPDOWN_KWARGS = {
    **LABEL_KWARGS,
    "fg_color": SECONDARY_BUTTON_BG_COLOR,
    "button_color": SECONDARY_BUTTON_BG_COLOR,
    "button_hover_color": dimm_color(SECONDARY_BUTTON_BG_COLOR),
    "corner_radius": CORNER_RADIUS_SMALL,
}

ENTRY_KWARGS = {
    **LABEL_KWARGS,
    "fg_color": SECONDARY_BUTTON_BG_COLOR,
    "border_width": 1,
    "corner_radius": CORNER_RADIUS_SMALL,
}


BUTTON_KWARKS = {**LABEL_KWARGS, "corner_radius": CORNER_RADIUS_SMALL}
