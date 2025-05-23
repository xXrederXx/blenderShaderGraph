"""Contains Style classes"""

from dataclasses import dataclass, field
from typing import Callable, Dict, Union, Tuple
from customtkinter import CTkFont
from util.color_util import dimm_color
from globals.my_logger import logger as log
from util.singleton import Singleton


@dataclass
class BaseStyle:
    """
    This class is a base class for style classes
    It contains every field which is needed to determine the style of this app.
    """

    # Fonts
    text_font: Union[Tuple[str, int], CTkFont]
    header_font: Union[Tuple[str, int], CTkFont]

    # Colors
    frame_bg_col: str
    main_bg_col: str
    toolbar_bg_col: str
    primary_button_bg_color: str
    secondary_button_bg_color: str
    text_color: str

    # Radii and padding
    corner_radius_small: int = 8
    corner_radius_medium: int = 16
    corner_radius_large: int = 32
    pad_small: int = 4
    pad_medium: int = 8
    pad_large: int = 16
    frame_grid_padx: int = 4
    frame_grid_pady: int = 4

    # Derived style properties
    frame_grid_kwargs: dict = field(init=False)
    frame_kwargs: dict = field(init=False)
    label_kwargs: dict = field(init=False)
    dropdown_kwargs: dict = field(init=False)
    entry_kwargs: dict = field(init=False)
    button_kwargs: dict = field(init=False)

    def __post_init__(self):
        self.frame_grid_kwargs = {
            "padx": self.frame_grid_padx,
            "pady": self.frame_grid_pady,
            "sticky": "nswe",
        }

        self.frame_kwargs = {
            "corner_radius": self.corner_radius_medium,
            "fg_color": self.frame_bg_col,
        }

        self.label_kwargs = {
            "font": self.text_font,
            "text_color": self.text_color,
        }

        self.dropdown_kwargs = {
            **self.label_kwargs,
            "fg_color": self.secondary_button_bg_color,
            "button_color": self.secondary_button_bg_color,
            "button_hover_color": dimm_color(self.secondary_button_bg_color),
            "corner_radius": self.corner_radius_small,
        }

        self.entry_kwargs = {
            **self.label_kwargs,
            "fg_color": self.secondary_button_bg_color,
            "border_width": 1,
            "corner_radius": self.corner_radius_small,
        }

        self.button_kwargs = {
            **self.label_kwargs,
            "corner_radius": self.corner_radius_small,
        }


def _init_fonts():
    log.debug("Initializing CTk fonts")
    return (
        ("Inter", 13),
        ("Inter", 20),
    )


def get_dark_theme() -> BaseStyle:
    """returns a dark-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#27282d",
        main_bg_col="#1e1f22",
        toolbar_bg_col="#2f3236",
        primary_button_bg_color="#224567",
        secondary_button_bg_color="#393a3e",
        text_color="#f9fdfe",
    )


def get_light_theme() -> BaseStyle:
    """returns a light-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#f0f0f0",
        main_bg_col="#ffffff",
        toolbar_bg_col="#dddddd",
        primary_button_bg_color="#007acc",
        secondary_button_bg_color="#e0e0e0",
        text_color="#1a1a1a",
    )


def get_night_theme() -> BaseStyle:
    """returns a night-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#121212",
        main_bg_col="#0d0d0d",
        toolbar_bg_col="#1f1f1f",
        primary_button_bg_color="#0a84ff",
        secondary_button_bg_color="#2c2c2e",
        text_color="#e5e5e7",
    )


def get_purple_theme() -> BaseStyle:
    """returns a purple-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#2e1a47",
        main_bg_col="#1f1033",
        toolbar_bg_col="#3c2166",
        primary_button_bg_color="#7e57c2",
        secondary_button_bg_color="#5e35b1",
        text_color="#f3e5f5",
    )


def get_blue_theme() -> BaseStyle:
    """returns a blue-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#e3f2fd",
        main_bg_col="#bbdefb",
        toolbar_bg_col="#90caf9",
        primary_button_bg_color="#1976d2",
        secondary_button_bg_color="#64b5f6",
        text_color="#0d47a1",
    )


def get_high_contrast_light_theme() -> BaseStyle:
    """returns a high-contrast light-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#ffffff",
        main_bg_col="#ffffff",
        toolbar_bg_col="#cccccc",
        primary_button_bg_color="#000000",
        secondary_button_bg_color="#ffffff",
        text_color="#000000",
    )


def get_high_contrast_dark_theme() -> BaseStyle:
    """returns a high-contrast dark-themed Style Class"""
    text_font, header_font = _init_fonts()
    return BaseStyle(
        text_font=text_font,
        header_font=header_font,
        frame_bg_col="#000000",
        main_bg_col="#000000",
        toolbar_bg_col="#1a1a1a",
        primary_button_bg_color="#ffffff",
        secondary_button_bg_color="#333333",
        text_color="#ffffff",
    )


class StyleManager(metaclass=Singleton):
    _theme: BaseStyle = get_dark_theme()
    _current_theme_name: str = "Dark"
    _name_map: Dict[str, Callable[[], BaseStyle]] = {
        "Dark": get_dark_theme,
        "Light": get_light_theme,
        "Night": get_night_theme,
        "Purple": get_purple_theme,
        "High Contrast Light": get_high_contrast_light_theme,
        "High Contrast Dark": get_high_contrast_dark_theme,
    }

    @classmethod
    def use_theme(cls, theme_name: str):
        log.debug(f"Switching theme: {theme_name}")
        if not cls._name_map.get(theme_name):
            log.error("Theme %s is not found", theme_name)
            return

        cls._theme = cls._name_map.get(theme_name, get_dark_theme)()
        cls._current_theme_name = theme_name

    @classmethod
    def get(cls) -> BaseStyle:
        return cls._theme

    @classmethod
    def get_theme(cls) -> str:
        return cls._current_theme_name
