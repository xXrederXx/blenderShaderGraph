import customtkinter as ctk
from abc import ABC, abstractmethod
from typing import TypeVar, Generic
from style import TEXT_COLOR

T = TypeVar("T")

class MyEntry(ctk.CTkEntry, Generic[T], ABC):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.bind("<KeyRelease>", lambda e: self.input_validation())

    def input_validation(self):
        current = self.get()
        if not self.validate_input(current):
            self.configure(text_color="red")
        else:
            self.configure(text_color=TEXT_COLOR)

    @abstractmethod
    def validate_input(self, text: str) -> bool:
        pass

    @abstractmethod
    def convert_input(self, text: str) -> T:
        pass

    def get_value(self, default: T) -> T:
        current = super().get()
        if not self.validate_input(current):
            return default
        return self.convert_input(current)


class IntEntry(MyEntry[int]):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)

    def validate_input(self, text: str) -> bool:
        try:
            int(text)
            return True
        except ValueError:
            return False

    def convert_input(self, text: str) -> int:
        return int(text)
    
class FloatEntry(MyEntry[float]):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)

    def validate_input(self, text: str) -> bool:
        try:
            float(text)
            return True
        except ValueError:
            return False

    def convert_input(self, text: str) -> float:
        return float(text)

class StringEntry(MyEntry[str]):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)

    def validate_input(self, text: str) -> bool:
        return True

    def convert_input(self, text: str) -> str:
        return text

class BoolEntry(MyEntry[bool]):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)

    def validate_input(self, text: str) -> bool:
        return text.lower() in ("yes", "no", "0", "1", "true", "false",)

    def convert_input(self, text: str) -> str:
        return text.lower() in ("yes", "1", "true")
    
class EnumEntry(MyEntry[bool]):
    def __init__(self, enum:list[str], **kwargs):
        super().__init__(**kwargs)
        self.enum = enum

    def validate_input(self, text: str) -> bool:
        return text in self.enum

    def convert_input(self, text: str) -> str:
        return text
