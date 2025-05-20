"""Avoid lint"""

import customtkinter as ctk
from NodeApp import NodeApp
from globals.my_logger import MyLogger
from globals.paths import TMP_PATH, PERSISTANT_PATH, LOG_PATH, AUTO_SAVE_PATH

if __name__ == "__main__":
    #Check paths created
    TMP_PATH.mkdir(parents=True, exist_ok=True)
    PERSISTANT_PATH.mkdir(parents=True, exist_ok=True)
    LOG_PATH.mkdir(parents=True, exist_ok=True)
    AUTO_SAVE_PATH.mkdir(parents=True, exist_ok=True)
    
    #Setting up logging
    log = MyLogger(10)
    log.set_up_logger()
    log.set_logger_output_console(True)

    log.info("Starting Node App")

    #Setting up ctk
    ctk.set_appearance_mode("Dark")
    ctk.set_default_color_theme("dark-blue")

    #Running app
    root = NodeApp()
    root.mainloop()

    log.info("Closing Node App")
