"""Avoid lint"""

if __name__ == "__main__":
    # Check paths created
    from globals.paths import TMP_PATH, PERSISTANT_PATH, LOG_PATH, AUTO_SAVE_PATH

    TMP_PATH.mkdir(parents=True, exist_ok=True)
    PERSISTANT_PATH.mkdir(parents=True, exist_ok=True)
    LOG_PATH.mkdir(parents=True, exist_ok=True)
    AUTO_SAVE_PATH.mkdir(parents=True, exist_ok=True)
    
    #Loading settings
    from my_types.settings import load_settings
    settings = load_settings()
    
    # Setting up logging
    from globals.my_logger import MyLogger

    log = MyLogger(10)
    log.set_up_logger()
    log.set_logger_output_console(True)

    log.info("Starting Node App")

    # Setting up ctk
    import customtkinter as ctk

    ctk.set_appearance_mode(settings.ctk_apperance_mode)
    ctk.set_default_color_theme("dark-blue")

    from globals.style import StyleManager

    StyleManager.use_theme(settings.style_theme)

    # Running app
    from NodeApp import NodeApp

    root = NodeApp()
    root.mainloop()

    log.info("Closing Node App")
