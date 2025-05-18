"""Avoid lint"""

import customtkinter as ctk
from NodeApp import NodeApp
from log import logger as log
from util.io_util import get_persistant_path, get_tmp_path, get_log_path

if __name__ == "__main__":
    log.set_up_logger()  # Set up logging to file
    log.set_logger_output_console(True)
    
    log.info("Starting Node App")
    
    ctk.set_appearance_mode("Dark")
    ctk.set_default_color_theme("dark-blue")
    
    get_persistant_path().mkdir(parents=True, exist_ok=True)
    get_tmp_path().mkdir(parents=True, exist_ok=True)
    get_log_path().mkdir(parents=True, exist_ok=True)
    
    root = NodeApp()
    root.mainloop()
    
    log.info("Closing Node App")
