import customtkinter as ctk
from typing import List, Optional, Type
from PIL import Image, ImageTk

from GUI.Nodes import Node

# App Settings
ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("blue")

def ReplaceCtkEntryText(entry : ctk.CTkEntry, text: str):
    entry.delete(0, 'end')
    entry.insert(0, text)

# === Main App ===
class NodeApp(ctk.CTk):
    def __init__(self) -> None:
        super().__init__()

        self.title("Node Manager")
        self.geometry("1400x800")
        self.nodes: List[Node] = []
        self.selected_node: Optional[Node] = None
        self.selected_node_index: Optional[int] = None

        self.custom_field_entries: dict[str, ctk.CTkEntry] = {}

        self.grid_columnconfigure((0,1,2), weight=1)
        self.grid_rowconfigure(0, weight=1)

        # Left: Node List
        self.node_list_frame = ctk.CTkScrollableFrame(self)
        self.node_list_frame.grid(row=0, column=0, sticky="nswe", padx=10, pady=10)
        self.node_buttons: List[ctk.CTkButton] = []

        # Middle: Details View
        self.details_frame = ctk.CTkFrame(self)
        self.details_frame.grid(row=0, column=1, sticky="nswe", padx=10, pady=10)

        self.details_label = ctk.CTkLabel(self.details_frame, text="Select a node", font=ctk.CTkFont(size=20, weight="bold"))
        self.details_label.pack(pady=10)

        self.name_entry = ctk.CTkEntry(self.details_frame, placeholder_text="Name")
        self.name_entry.pack(pady=5)

        self.desc_entry = ctk.CTkEntry(self.details_frame, placeholder_text="Description")
        self.desc_entry.pack(pady=5)

        self.category_label = ctk.CTkLabel(self.details_frame, text="Category: -")
        self.category_label.pack(pady=5)

        self.custom_fields_frame = ctk.CTkFrame(self.details_frame)
        self.custom_fields_frame.pack(pady=10, fill='both', expand=True)

        self.image_label = ctk.CTkLabel(self.details_frame, text="")
        self.image_label.pack(pady=10)

        self.update_button = ctk.CTkButton(self.details_frame, text="Update Node", command=self.update_node)
        self.update_button.pack(pady=10)

        # Right: Add Node Form
        self.add_node_frame = ctk.CTkFrame(self)
        self.add_node_frame.grid(row=0, column=2, sticky="nswe", padx=10, pady=10)

        ctk.CTkLabel(self.add_node_frame, text="Add New Node", font=ctk.CTkFont(size=20, weight="bold")).pack(pady=10)

        self.node_types: List[Type[Node]] = Node.__subclasses__()
        self.node_type_var = ctk.StringVar(value=self.node_types[0].__name__)
        self.node_type_menu = ctk.CTkOptionMenu(
            self.add_node_frame,
            variable=self.node_type_var,
            values=[cls.__name__ for cls in self.node_types]
        )
        self.node_type_menu.pack(pady=5)

        self.new_name_entry = ctk.CTkEntry(self.add_node_frame, placeholder_text="Name")
        self.new_name_entry.pack(pady=5)

        self.new_desc_entry = ctk.CTkEntry(self.add_node_frame, placeholder_text="Description")
        self.new_desc_entry.pack(pady=5)

        self.new_image_path_entry = ctk.CTkEntry(self.add_node_frame, placeholder_text="Image Path (optional)")
        self.new_image_path_entry.pack(pady=5)

        self.add_button = ctk.CTkButton(self.add_node_frame, text="Add Node", command=self.add_node)
        self.add_button.pack(pady=20)

    def add_node(self) -> None:
        name = self.new_name_entry.get()
        description = self.new_desc_entry.get()
        image_path = self.new_image_path_entry.get() if self.new_image_path_entry.get() else None
        selected_type_name = self.node_type_var.get()

        if name and selected_type_name:
            node_cls = next((cls for cls in self.node_types if cls.__name__ == selected_type_name), None)
            if node_cls:
                new_node = node_cls(name=name, description=description, image_path=image_path)
                self.nodes.append(new_node)
                self.update_node_list()

            self.new_name_entry.delete(0, 'end')
            self.new_desc_entry.delete(0, 'end')
            self.new_image_path_entry.delete(0, 'end')

    def update_node_list(self) -> None:
        for btn in self.node_buttons:
            btn.destroy()
        self.node_buttons.clear()

        for idx, node in enumerate(self.nodes):
            btn = ctk.CTkButton(self.node_list_frame, text=node.name, command=lambda idx=idx: self.show_details(idx))
            btn.pack(fill='x', pady=2)
            self.node_buttons.append(btn)

    def show_details(self, idx: int) -> None:
        self.selected_node = self.nodes[idx]
        self.selected_node_index = idx

        if not self.selected_node:
            return
        
        self.details_label.configure(text=f"Editing: {self.selected_node.name}")
        
        ReplaceCtkEntryText(self.name_entry, self.selected_node.name)
        ReplaceCtkEntryText(self.desc_entry, self.selected_node.description)

        self.category_label.configure(text=f"Category: {self.selected_node.get_category()}")

        self.clear_custom_fields()

        custom_fields = self.selected_node.get_custom_fields()
        for field_name, value in custom_fields.items():
            field_frame = ctk.CTkFrame(self.custom_fields_frame)
            field_frame.pack(pady=2, fill="x")  # Stretch across the parent frame

            label = ctk.CTkLabel(field_frame, text=field_name, width=120, anchor="w")
            label.pack(side="left", padx=24)  # Some padding between label and entry

            entry = ctk.CTkEntry(field_frame, width=200)
            entry.insert(0, str(value))
            entry.pack(side="left", fill="x", expand=True)  # Let entry take extra space if needed

            self.custom_field_entries[field_name] = entry

        if self.selected_node.image_path:
            try:
                img = Image.open(self.selected_node.image_path)
                img = img.resize((200, 200))
                self.tk_image = ImageTk.PhotoImage(img)
                self.image_label.configure(image=self.tk_image, text="")
            except Exception:
                self.image_label.configure(text="Failed to load image", image="")
        else:
            self.image_label.configure(text="No image provided", image="")

    def clear_custom_fields(self) -> None:
        for widget in self.custom_fields_frame.winfo_children():
            widget.destroy()
        self.custom_field_entries.clear()

    def update_node(self) -> None:
        if self.selected_node_index is None:
            return
        
        node = self.nodes[self.selected_node_index]
        node.name = self.name_entry.get()
        node.description = self.desc_entry.get()

        custom_field_values = {
            field_name: entry.get()
            for field_name, entry in self.custom_field_entries.items()
        }
        node.set_custom_fields(custom_field_values)

        self.update_node_list()
        self.show_details(self.selected_node_index)

if __name__ == "__main__":
    app = NodeApp()
    app.mainloop()
