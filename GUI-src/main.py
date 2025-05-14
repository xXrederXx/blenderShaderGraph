"""Avoid lint"""

import customtkinter as ctk
from NodeApp import NodeApp
from log import logger

if __name__ == "__main__":
    logger.set_up_logger()  # Set up logging to file
    logger.set_logger_output_console(True)
    
    ctk.set_appearance_mode("Dark")
    ctk.set_default_color_theme("dark-blue")
    
    root = NodeApp()
    root.mainloop()
