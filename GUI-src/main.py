"""Avoid lint"""

import customtkinter as ctk
from NodeApp import NodeApp
from log import logger as log

if __name__ == "__main__":
    log.set_up_logger()  # Set up logging to file
    log.set_logger_output_console(True)
    
    log.info("Starting Node App")
    
    ctk.set_appearance_mode("Dark")
    ctk.set_default_color_theme("dark-blue")
    
    root = NodeApp()
    root.mainloop()
    
    log.info("Closing Node App")
