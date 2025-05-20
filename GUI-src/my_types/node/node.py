"""Contains the Node data type"""

from dataclasses import dataclass
from typing import Dict, Any


@dataclass
class Node:
    """Contains name and params of a Node Type"""

    name: str
    params: Dict[str, Any]

    def __post_init__(self):
        self.params["type:S"] = self.name
