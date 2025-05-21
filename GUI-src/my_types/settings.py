"""Contains settings datatype and load / save functions"""

import os
import json
from dataclasses import dataclass
from typing import Any, Dict
from globals.paths import SETTINGS_FILE_PATH
from globals.style import StyleManager
import customtkinter as ctk
from util.singleton import Singleton


@dataclass
class Settings(metaclass=Singleton):
    """Stores Settings"""

    ctk_apperance_mode: str = "Dark"
    style_theme: str = "Dark"


def load_settings() -> Settings:
    """Used To load the data from the settings file"""
    settings:Settings = Settings()
    if not os.path.exists(SETTINGS_FILE_PATH):
        return settings

    with open(SETTINGS_FILE_PATH, "r", encoding="utf-8") as f:
        json_data: Dict[str:Any] = json.loads(f.read())
        settings.style_theme = json_data.get("style_theme", "Dark")
        settings.ctk_apperance_mode = json_data.get("ctk_apperance_mode", "Dark")
    
    return settings


def save_settings():
    """Used To save the data to the settings file"""
    with open(SETTINGS_FILE_PATH, "w", encoding="utf-8") as f:
        json_data: Dict[str:Any] = {
            "style_theme": StyleManager.get_theme(),
            "ctk_apperance_mode": ctk.get_appearance_mode(),
        }

        f.write(json.dumps(json_data))
