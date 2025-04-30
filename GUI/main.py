import customtkinter as ctk
from typing import List, Optional
from PIL import Image, ImageTk

from CTkLabledEntry import CTkLabledEntry
from myJson import ToJsonString
from Nodes import GetDict, NODE_TYPES

# App Settings
ctk.set_appearance_mode("Dark")
ctk.set_default_color_theme("blue")


def ReplaceCtkEntryText(entry: ctk.CTkEntry, text: str):
    entry.delete(0, 'end')
    entry.insert(0, text)

# === Main App ===


class NodeApp(ctk.CTk):
    def __init__(self) -> None:
        super().__init__()

        self._SetupWindowAndGrid()
        self._SetupNodeListFrame()
        self._SetupConfigFrame()
        self._SetupAddNodeFrame()

    def _SetupNodeListFrame(self):
        # Left: Node List
        self.node_list_frame = ctk.CTkScrollableFrame(self)
        self.node_list_frame.grid(
            row=0, column=0, sticky="nswe", padx=10, pady=10)
        self.node_buttons: List[ctk.CTkButton] = []

    def _SetupConfigFrame(self):
        # Middle: Details View
        details_frame = ctk.CTkFrame(self)
        details_frame.grid(
            row=0, column=1, sticky="nswe", padx=10, pady=10)

        self.details_label = ctk.CTkLabel(
            details_frame, text="Select a node", font=ctk.CTkFont(size=20, weight="bold"))
        self.details_label.pack(pady=10)

        self.update_button = ctk.CTkButton(
            details_frame, text="Update Node", command=self.update_node)
        self.update_button.pack(pady=10)
    

        self.category_label = ctk.CTkLabel(
            details_frame, text="Category: -")
        self.category_label.pack(pady=5)

        self.custom_fields_frame = ctk.CTkFrame(details_frame)
        self.custom_fields_frame.pack(pady=10, fill='both', expand=False)

        self.image_label = ctk.CTkLabel(details_frame, text="")
        self.image_label.pack(pady=10)

    def _SetupAddNodeFrame(self):
        # Right: Add Node Form
        right_container = ctk.CTkFrame(self)
        right_container.grid(
            row=0, column=2, sticky="nswe", padx=10, pady=10)
        right_container.rowconfigure((0, 1), weight=1)
        right_container.columnconfigure(0, weight=1)
        
        add_node_frame = ctk.CTkFrame(right_container)
        add_node_frame.grid(
            row=0, column=0, sticky="nswe", padx=10, pady=10)

        ctk.CTkLabel(add_node_frame, text="Add New Node",
                     font=ctk.CTkFont(size=20, weight="bold")).pack(pady=10)

        self.node_types: List[str] = [x[0] for x in NODE_TYPES]
        self.node_type_var = ctk.StringVar(value=self.node_types[0])
        self.node_type_menu = ctk.CTkOptionMenu(
            add_node_frame,
            variable=self.node_type_var,
            values=self.node_types
        )
        self.node_type_menu.pack(pady=5, padx=24, fill="x")

        self.new_id_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Id")
        self.new_id_entry.pack(pady=5, padx=24, fill="x")
        self.new_desc_entry = ctk.CTkEntry(
            add_node_frame, placeholder_text="Description")
        self.new_desc_entry.pack(pady=5, padx=24, fill="x")

        self.add_button = ctk.CTkButton(
            add_node_frame, text="Add Node", command=self.add_node)
        self.add_button.pack(pady=20, padx=24, fill="x")
        
        image_display_frame = ctk.CTkFrame(right_container)
        image_display_frame.grid(
            row=1, column=0, sticky="nswe", padx=10, pady=10)
        generate_image_button = ctk.CTkButton(image_display_frame, text="Generate Image", command=self.generate_image)
        generate_image_button.pack(fill="x", padx = 8, pady = 8)
        self.generated_image = ctk.CTkLabel(image_display_frame, text="No Image Generated", image=None)
        self.generated_image.pack(fill="both", padx = 8, pady = 8, expand=True)

    def _SetupWindowAndGrid(self):
        self.title("Node Manager")
        self.geometry("1400x800")
        self.nodes: List[dict[str, any]] = []
        self.selected_node: Optional[dict[str, any]] = None
        self.selected_node_index: Optional[int] = None

        self.custom_field_entries: dict[str, ctk.CTkEntry] = {}

        self.grid_columnconfigure((0), weight=1)
        self.grid_columnconfigure((1, 2), weight=2)
        self.grid_rowconfigure(0, weight=1)

    def add_node(self) -> None:
        name = self.new_id_entry.get()
        description = self.new_desc_entry.get()
        selected_type_name = self.node_type_var.get()

        if name and selected_type_name:
            node_cls = next(
                (cls for cls in self.node_types if cls == selected_type_name), None)
            if node_cls:
                new_node = GetDict(selected_type_name)
                new_node["idS"] = name
                new_node["descriptionS"] = description
                self.nodes.append(new_node)
                self.update_node_list()

            self.new_id_entry.delete(0, 'end')
            self.new_desc_entry.delete(0, 'end')

    def update_node_list(self) -> None:
        for btn in self.node_buttons:
            btn.destroy()
        self.node_buttons.clear()

        for idx, node in enumerate(self.nodes):
            btn = ctk.CTkButton(
                self.node_list_frame, text=node["idS"], command=lambda idx=idx: self.show_details(idx))
            btn.pack(fill='x', pady=2)
            self.node_buttons.append(btn)

    def show_details(self, idx: int) -> None:
        self.selected_node = self.nodes[idx]
        self.selected_node_index = idx

        if not self.selected_node:
            return

        self.details_label.configure(
            text=f"Editing: {self.selected_node['idS']}")


        self.category_label.configure(
            text=f"Category: {self.selected_node['typeS']}")

        self.clear_custom_fields()

        for field_name, value in self.selected_node.items():
            if field_name == "typeS":
                continue
            labled_entry = CTkLabledEntry(
                self.custom_fields_frame, field_name[0:-1], 180, 100, 24, "x", True)
            labled_entry.entry.insert(0, str(value))
            self.custom_field_entries[field_name] = labled_entry.entry

        if False:
            try:
                img = Image.open("path")
                img = img.resize((200, 200))
                self.tk_image = ImageTk.PhotoImage(img)
                self.image_label.configure(image=self.tk_image, text="")
            except Exception:
                self.image_label.configure(
                    text="Failed to load image", image="")
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

        for field_name, entry in self.custom_field_entries.items():
            if field_name.endswith("N"):
                node[field_name] = int(entry.get())
            elif field_name.endswith("S"):
                node[field_name] = str(entry.get())
            else:
                node[field_name] = entry.get()
        
        self.update_node_list()
        self.show_details(self.selected_node_index)
        print(ToJsonString(self.nodes))

    def generate_image(self):
        pass

if __name__ == "__main__":
    app = NodeApp()
    app.mainloop()
