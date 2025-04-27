from abc import ABC, abstractmethod
from typing import Optional

# === Base Node Class ===


class Node(ABC):
    def __init__(self, name: str, description: str, image_path: Optional[str] = None) -> None:
        self.name = name
        self.description = description
        self.image_path = image_path

    @abstractmethod
    def get_category(self) -> str:
        pass

    @abstractmethod
    def get_custom_fields(self) -> dict[str, any]:
        """Return dict of field_name -> value."""
        pass

    @abstractmethod
    def set_custom_fields(self, fields: dict[str, any]) -> None:
        """Set field_name -> value."""
        pass

# === Example subclass


class SimpleNode(Node):
    def __init__(self, name: str, description: str, image_path: Optional[str] = None, width: int = 100, height: int = 100) -> None:
        super().__init__(name, description, image_path)
        self.width = width
        self.height = height

    def get_category(self) -> str:
        return "Simple"

    def get_custom_fields(self) -> dict[str, any]:
        return {"width": self.width, "height": self.height}

    def set_custom_fields(self, fields: dict[str, any]) -> None:
        self.width = int(fields.get("width", self.width))
        self.height = int(fields.get("height", self.height))
