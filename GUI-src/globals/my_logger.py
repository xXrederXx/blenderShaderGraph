"""This contains the logger and exposes a default logger"""

import logging as log
import time
import sys
from typing import Optional
from globals.paths import LOG_PATH
from util.singleton import Singleton


class MyLogger(metaclass=Singleton):
    """My implementation of a logger"""

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
        """This sets up the defult logger, which logs to a file"""
        filename = time.strftime("%Y-%m-%d_%H-%M-%S") + ".log"
        path = LOG_PATH / filename
        self.log.setLevel(self.log_level)

        # Clear existing handlers (in case this function is called again)
        self.log.handlers.clear()

        self.is_logging_to_console = False

        # File handler
        self._file_stream_hdlr = log.FileHandler(path, encoding="utf-8", mode="a")
        self._file_stream_hdlr.setLevel(self.log_level)
        self._file_stream_hdlr.setFormatter(self.formatter)
        self.log.addHandler(self._file_stream_hdlr)

    def set_logger_output_console(self, should_log: bool) -> None:
        """
        Dynamically enables or disables logging to the console.

        If shouldLog is True, logging goes to both file and console.
        If False, logging goes only to the file.
        """

        if should_log and not self.is_logging_to_console:
            # Add a console handler if not already added
            self._console_stream_hdlr = log.StreamHandler(sys.stdout)
            self._console_stream_hdlr.setLevel(self.log_level)
            self._console_stream_hdlr.setFormatter(self.formatter)
            self.log.addHandler(self._console_stream_hdlr)
            self.is_logging_to_console = True

        elif not should_log and self.is_logging_to_console:
            # Remove all StreamHandlers to stop console logging
            self.log.removeHandler(self._console_stream_hdlr)
            self.is_logging_to_console = False

# Old way to accsses logger
logger = MyLogger(log.DEBUG)
