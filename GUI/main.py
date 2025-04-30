from typing import List, Optional
import customtkinter as ctk
from PIL import Image, ImageTk

from CTkLabledEntry import CTkLabledEntry
from color_util import dimm_color
from myJson import to_json_string
from Nodes import NEW_NODE_TYPES

# App Settings
ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("blue")


def replace_ctkentry_text(entry: ctk.CTkEntry, text: str):
    """Replaces the text of an entry

    Args:
        entry (ctk.CTkEntry): The entry
        text (str): The text to write
    """
    entry.delete(0, 'end')
    entry.insert(0, text)


class NodeListFrame(ctk.CTkScrollableFrame):
    """Frame that displays a list of nodes as buttons."""

    def __init__(self, master, on_node_select) -> None:
        super().__init__(master)
        self.on_node_select = on_node_select
        self.node_buttons: List[ctk.CTkButton] = []

        self.node_colors: dict[str, str] = {}
        for g in NEW_NODE_TYPES:
            for n in g.nodes:
                self.node_colors[n.name] = g.color

    def update_node_list(self, nodes: List[dict]) -> None:
        """Rebuilds the node list from scratch."""
        for btn in self.node_buttons:
            btn.destroy()
        self.node_buttons.clear()
        for idx, node in enumerate(nodes):
            btn = ctk.CTkButton(
                self, text=node["idS"], fg_color=self.node_colors[node["typeS"]], hover_color=dimm_color(self.node_colors[node["typeS"]]), command=lambda idx=idx: self.on_node_select(idx))
            btn.pack(fill='x', pady=2)
            self.node_buttons.append(btn)


class NodeConfigFrame(ctk.CTkFrame):
    """Frame for configuring and editing a selected node."""

    def __init__(self, master, on_update) -> None:
        super().__init__(master)
        self.on_update = on_update

        self.details_label = ctk.CTkLabel(
            self, text="Select a node", font=ctk.CTkFont(size=20, weight="bold"))
        self.details_label.pack(pady=10)

        self.update_button = ctk.CTkButton(
            self, text="Update Node", command=self.on_update)
        self.update_button.pack(pady=10)

        self.category_label = ctk.CTkLabel(self, text="Category: -")
        self.category_label.pack(pady=5)

        self.custom_fields_frame = ctk.CTkFrame(self)
        self.custom_fields_frame.pack(pady=10, fill='both', expand=False)

        self.image_label = ctk.CTkLabel(self, text="")
        self.image_label.pack(pady=10)

        self.custom_field_entries: dict[str, ctk.CTkEntry] = {}

    def show_details(self, node: dict) -> None:
        """Displays details of the selected node in the config frame."""
        self.details_label.configure(text=f"Editing: {node['idS']}")
        self.category_label.configure(text=f"Category: {node['typeS']}")
        self.clear_custom_fields()

        for field_name, value in node.items():
            if field_name == "typeS":
                continue
            labled_entry = CTkLabledEntry(
                self.custom_fields_frame, field_name[0:-1], 180, 100, 24, "x", True)
            labled_entry.entry.insert(0, str(value))
            self.custom_field_entries[field_name] = labled_entry.entry

        self.image_label.configure(text="No image provided", image="")

    def clear_custom_fields(self) -> None:
        """Removes all field entry widgets from the frame."""
        for widget in self.custom_fields_frame.winfo_children():
            widget.destroy()
        self.custom_field_entries.clear()


class AddNodeFrame(ctk.CTkFrame):
    """Frame for adding a new node."""

    def __init__(self, master, on_add_node, on_generate_image, on_group_change) -> None:
        super().__init__(master)
        self.on_add_node = on_add_node
        self.on_group_change = on_group_change

        self.node_groups: List[str] = [group.name for group in NEW_NODE_TYPES]
        self.node_groups_colors: dict[str, str] = {
            group.name: group.color for group in NEW_NODE_TYPES}
        self.node_types_dict: dict[str, List[str]] = {
            group.name: [node.name for node in group.nodes] for group in NEW_NODE_TYPES
        }

        self.rowconfigure((0, 1), weight=1)
        self.columnconfigure(0, weight=1)

        add_node_frame = ctk.CTkFrame(self)
        add_node_frame.grid(row=0, column=0, sticky="nswe", padx=10, pady=10)

        ctk.CTkLabel(add_node_frame, text="Add New Node",
                     font=ctk.CTkFont(size=20, weight="bold")).pack(pady=10)

        self.node_groups_var = ctk.StringVar(value=self.node_groups[0])
        self.node_groups_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_groups_var,
            values=self.node_groups,
            command=self.on_group_change
        )
        self.node_groups_menu.pack(pady=5, padx=24, fill="x")

        initial_group = self.node_groups[0]
        self.node_type_var = ctk.StringVar(
            value=self.node_types_dict[initial_group][0])
        self.node_type_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_type_var,
            values=self.node_types_dict[initial_group]
        )
        self.node_type_menu.pack(pady=5, padx=24, fill="x")

        self.new_id_entry = ctk.CTkEntry(add_node_frame, placeholder_text="Id")
        self.new_id_entry.pack(pady=5, padx=24, fill="x")

        self.new_desc_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Description")
        self.new_desc_entry.pack(pady=5, padx=24, fill="x")

        self.add_button = ctk.CTkButton(
            add_node_frame, text="Add Node", command=self.on_add_node)
        self.add_button.pack(pady=20, padx=24, fill="x")

        image_display_frame = ctk.CTkFrame(self)
        image_display_frame.grid(
            row=1, column=0, sticky="nswe", padx=10, pady=10)

        generate_image_button = ctk.CTkButton(
            image_display_frame, text="Generate Image", command=on_generate_image)
        generate_image_button.pack(fill="x", padx=8, pady=8)

        self.generated_image = ctk.CTkLabel(
            image_display_frame, text="No Image Generated", image=None)
        self.generated_image.pack(fill="both", padx=8, pady=8, expand=True)

        self.update_menu_color(self.node_groups_var.get())

    def update_node_type_menu(self, selected_group: str):
        """Update the node types shown in the dropdown."""
        new_types = self.node_types_dict[selected_group]
        self.node_type_menu.configure(values=new_types)
        self.node_type_var.set(new_types[0])
        self.update_menu_color(selected_group)

    def update_menu_color(self, selected_group: str):
        """Update the color of the node group menu."""
        color = self.node_groups_colors[selected_group]
        self.node_groups_menu.configure(fg_color=color)


class NodeApp(ctk.CTk):
    """Main application for managing nodes."""

    def __init__(self) -> None:
        super().__init__()
        self.title("Node Manager")
        self.geometry("1400x800")

        self.nodes: List[dict[str, any]] = []
        self.selected_node_index: Optional[int] = None

        self.grid_columnconfigure((0), weight=1)
        self.grid_columnconfigure((1, 2), weight=2)
        self.grid_rowconfigure(0, weight=1)

        # Left Panel
        self.node_list_frame = NodeListFrame(
            self, on_node_select=self.show_details)
        self.node_list_frame.grid(
            row=0, column=0, sticky="nswe", padx=10, pady=10)

        # Middle Panel
        self.config_frame = NodeConfigFrame(self, on_update=self.update_node)
        self.config_frame.grid(
            row=0, column=1, sticky="nswe", padx=10, pady=10)

        # Right Panel
        self.add_node_frame = AddNodeFrame(
            self,
            on_add_node=self.add_node,
            on_generate_image=self.generate_image,
            on_group_change=self.update_node_type_menu
        )
        self.add_node_frame.grid(
            row=0, column=2, sticky="nswe", padx=10, pady=10)

    def add_node(self) -> None:
        """Adds a new node to the list."""
        name = self.add_node_frame.new_id_entry.get()
        description = self.add_node_frame.new_desc_entry.get()
        selected_type_name = self.add_node_frame.node_type_var.get()
        selected_group_name = self.add_node_frame.node_groups_var.get()

        if name and selected_type_name:
            node_group = next(
                (g for g in NEW_NODE_TYPES if g.name == selected_group_name), None)
            if node_group:
                node_type = next(
                    (nt for nt in node_group.nodes if nt.name == selected_type_name), None)
                if node_type:
                    new_node = dict(node_type.params)
                    new_node["idS"] = name
                    new_node["descriptionS"] = description
                    new_node["typeS"] = selected_type_name

                    self.nodes.append(new_node)
                    self.node_list_frame.update_node_list(self.nodes)

        self.add_node_frame.new_id_entry.delete(0, 'end')
        self.add_node_frame.new_desc_entry.delete(0, 'end')

    def show_details(self, idx: int) -> None:
        """Display details of selected node."""
        self.selected_node_index = idx
        selected_node = self.nodes[idx]
        self.config_frame.show_details(selected_node)

    def update_node(self) -> None:
        """Update the currently selected node from config fields."""
        if self.selected_node_index is None:
            return

        node = self.nodes[self.selected_node_index]
        for field_name, entry in self.config_frame.custom_field_entries.items():
            value = entry.get()
            if field_name.endswith("N"):
                node[field_name] = int(value)
            else:
                node[field_name] = str(value)

        self.node_list_frame.update_node_list(self.nodes)
        self.show_details(self.selected_node_index)
        print(to_json_string(self.nodes))

    def update_node_type_menu(self, selected_group: str) -> None:
        """Update node type dropdown and color on group change."""
        self.add_node_frame.update_node_type_menu(selected_group)

    def generate_image(self) -> None:
        """Stub for generating an image."""
        pass


if __name__ == "__main__":
    import requests
    json_data = [
        {
            "id": "n",
            "type": "NoiseTexture",
            "params": {
                "width": 1024,
                "height": 1024,
                "size": 10
            }
        }
    ]

    response = requests.post(
        "http://localhost:5000/generate-image", json=json_data)
    print("Status code:", response.status_code)
    if response.status_code != 200:
        print(response.text)  # Might contain C# exception string

    with open("output.png", "wb") as f:
        f.write(response.content)

    app = NodeApp()
    app.mainloop()
