import logging as log
import time
from pathlib import Path
import sys
from typing import Optional


class MyLogger:
    def __init__(self, log_level: int, formatter: Optional[log.Formatter] = None):
        self.log = log.getLogger(__name__)

        if formatter:
            self.formatter = formatter
        else:
            self.formatter = log.Formatter(
                '%(levelname)s - "%(pathname)s", line %(lineno)d, in %(module)s\n\t%(message)s'
            )

        self.log_level = log_level
        self.is_logging_to_console = False
        self._file_stream_hdlr = None
        self._console_stream_hdlr = None
        
        self.debug = self.log.debug
        self.info = self.log.info
        self.warn = self.log.warning
        self.warning = self.log.warning
        self.error = self.log.error
        self.critical = self.log.critical

    def set_up_logger(self):
        filename = time.strftime("%Y-%m-%d_%H-%M-%S") + ".log"
        directory = Path.cwd() / "tmp" / "log"
        directory.mkdir(parents=True, exist_ok=True)
        path = directory / filename

        self.log.setLevel(self.log_level)

        # Clear existing handlers (in case this function is called again)
        self.log.handlers.clear()

        self.is_logging_to_console = False

        # File handler
        self._file_stream_hdlr = log.FileHandler(path, encoding="utf-8", mode="a")
        self._file_stream_hdlr.setLevel(self.log_level)
        self._file_stream_hdlr.setFormatter(self.formatter)
        self.log.addHandler(self._file_stream_hdlr)

    def set_logger_output_console(self, shouldLog: bool) -> None:
        """
        Dynamically enables or disables logging to the console.

        If shouldLog is True, logging goes to both file and console.
        If False, logging goes only to the file.
        """

        if shouldLog and not self.is_logging_to_console:
            # Add a console handler if not already added
            self._console_stream_hdlr = log.StreamHandler(sys.stdout)
            self._console_stream_hdlr.setLevel(self.log_level)
            self._console_stream_hdlr.setFormatter(self.formatter)
            self.log.addHandler(self._console_stream_hdlr)
            self.is_logging_to_console = True

        elif not shouldLog and self.is_logging_to_console:
            # Remove all StreamHandlers to stop console logging
            self.log.removeHandler(self._console_stream_hdlr)
            self.is_logging_to_console = False
    
logger = MyLogger(log.DEBUG)