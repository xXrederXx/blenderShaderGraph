"""Contains Paths which are used"""

from pathlib import Path
import os
import tempfile

PERSISTANT_PATH: Path = Path(os.getenv("LOCALAPPDATA")) / "BSG"
LOG_PATH: Path = PERSISTANT_PATH / "logs"
AUTO_SAVE_PATH: Path = PERSISTANT_PATH / "auto-saves"
TMP_PATH: Path = Path(tempfile.gettempdir()) / "BSG"

SETTINGS_FILE_PATH: Path = PERSISTANT_PATH / "settings.json"
