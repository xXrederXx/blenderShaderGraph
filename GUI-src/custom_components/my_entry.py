"""Contains custom entries with lint"""

from abc import ABC, abstractmethod
from typing import List, TypeVar, Generic, Callable
import customtkinter as ctk
from globals.style import StyleManager
from globals.my_logger import MyLogger

T = TypeVar("T")
style = StyleManager.get()
log = MyLogger()


class MyEntry(ctk.CTkEntry, Generic[T], ABC):
    """The base class"""

    def __init__(self, default_value: T, **kwargs):
        super().__init__(**kwargs)
        self.bind("<KeyRelease>", lambda e: self._on_key_relese())
        self.default = default_value
        self._after_id = None
        self.on_end_editing: Callable[[], None]

    def _on_key_relese(self):
        if self._after_id:
            self.after_cancel(self._after_id)
        if self.on_end_editing:
            self._after_id = self.after(1000, self.on_end_editing)

        self.input_validation()

    def input_validation(self):
        """This validates the input and colores the text accordingly"""
        current = self.get()
        if not self.validate_input(current):
            self.configure(text_color="red")
        else:
            self.configure(text_color=style.text_color)

    @abstractmethod
    def validate_input(self, text: str) -> bool:
        """This function determines if the text is valid"""
        log.warn("Your calling an empty abstarct method")

    @abstractmethod
    def convert_input(self, text: str) -> T:
        """This converts the string to type T"""
        log.warn("Your calling an empty abstarct method")

    def get_value(self) -> T:
        """This returns the value as T"""
        current = super().get()
        if not self.validate_input(current):
            return self.default
        return self.convert_input(current)


class IntEntry(MyEntry[int]):
    """Intiger variant of MyEntry"""

    def __init__(self, **kwargs):
        super().__init__(0, **kwargs)

    def validate_input(self, text: str) -> bool:
        try:
            int(text)
            return True
        except ValueError:
            return False

    def convert_input(self, text: str) -> int:
        return int(text)


class FloatEntry(MyEntry[float]):
    """Float variant of MyEntry"""

    def __init__(self, **kwargs):
        super().__init__(0.0, **kwargs)

    def validate_input(self, text: str) -> bool:
        try:
            float(text)
            return True
        except ValueError:
            return False

    def convert_input(self, text: str) -> float:
        return float(text)


class StringEntry(MyEntry[str]):
    """String variant of MyEntry"""

    def __init__(self, **kwargs):
        super().__init__("", **kwargs)

    def validate_input(self, text: str) -> bool:
        return True

    def convert_input(self, text: str) -> str:
        return text


class BoolEntry(MyEntry[bool]):
    """Bool variant of MyEntry"""

    def __init__(self, **kwargs):
        super().__init__(False, **kwargs)

    def validate_input(self, text: str) -> bool:
        return text.lower() in (
            "yes",
            "no",
            "0",
            "1",
            "true",
            "false",
        )

    def convert_input(self, text: str) -> bool:
        return text.lower() in ("yes", "1", "true")


class EnumEntry(MyEntry[str]):
    """Enum variant of MyEntry"""

    def __init__(self, enum: List[str], **kwargs):
        super().__init__("", **kwargs)
        self.enum = enum

    def validate_input(self, text: str) -> bool:
        return text in self.enum

    def convert_input(self, text: str) -> str:
        return text
