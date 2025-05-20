"""Contains the Node Group data type"""

from dataclasses import dataclass
from typing import List
from my_types.node.node import Node


@dataclass
class NodeGroup:
    """Contains a group name, a group color, and a List of node types for the group"""

    name: str
    color: str
    nodes: List[Node]
