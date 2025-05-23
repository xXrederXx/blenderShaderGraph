"""This is the main app class which contains all GUI"""

import time
import json
from tkinter import filedialog as fd
import customtkinter as ctk
from util.export_util import (
    nodes_to_json,
    nodes_to_cs,
    nodes_from_bsg,
    nodes_to_bsg,
    BSGData,
)
from globals.style import StyleManager
from globals.my_logger import MyLogger
from globals.paths import AUTO_SAVE_PATH
from Frames.NodeAppMainFrame import NodeAppMainFrame
from Frames.ToolBarFrame import ToolBarFrame

style = StyleManager.get()
log = MyLogger()


class NodeApp(ctk.CTk):
    """Main application for managing nodes."""

    def __init__(self):
        log.debug("Init Node App")
        super().__init__(fg_color=style.main_bg_col)
        self.title("Node Manager")
        self.geometry("1400x800")
        self.protocol("WM_DELETE_WINDOW", self.on_closing)

        self.auto_save_interval = 5 * 60 * 1000  # 5 minutes
        self.after(self.auto_save_interval, self.auto_save)

        self.columnconfigure(0, weight=1)
        self.rowconfigure(0, weight=0)
        self.rowconfigure(1, weight=1)

        self.top = ctk.CTkFrame(
            self, fg_color=style.toolbar_bg_col, height=48, corner_radius=0
        )
        self.top.grid(row=0, column=0, sticky="nswe")

        self.tool_bar = ToolBarFrame(
            self.top,
            self.on_save_btn,
            self.on_load_btn,
            self.on_export_btn,
            self.on_new_project,
        )
        self.top.columnconfigure(0, weight=1)
        self.top.rowconfigure(0, weight=1)
        self.tool_bar.grid(sticky="nswe")

        self.main = ctk.CTkFrame(self, fg_color=style.main_bg_col, corner_radius=0)
        self.main.grid(row=1, column=0, **style.frame_grid_kwargs)
        self.main.columnconfigure(0, weight=1)
        self.main.rowconfigure(0, weight=1)

        self.app = NodeAppMainFrame(self.main)
        self.app.grid(sticky="nswe")

    def on_save_btn(self):
        """Used to save to a file"""
        fp = fd.asksaveasfilename(
            defaultextension=".bsg",
            initialfile=self.tool_bar.proj_name_entry.get(),
            filetypes=[("bsg file", "*.bsg"), ("json file", "*.json")],
        )
        if not fp:
            log.info("No file path found after asksaveasfilename")
            return

        content = nodes_to_bsg(
            BSGData(
                self.app.nodes,
                self.app.selected_node_index,
                self.tool_bar.proj_name_entry.get(),
            )
        )
        with open(fp, "w", encoding="utf-8") as f:
            f.write(content)
        log.debug("Saved content to %s", fp)

    def on_load_btn(self):
        """Used to load from a file"""
        fp = fd.askopenfilename(
            filetypes=[("bsg file", "*.bsg"), ("json file", "*.json")]
        )
        if not fp:
            log.info("No file path found after askopenfilename")
            return

        content = ""
        with open(fp, "r", encoding="utf-8") as f:
            content = f.read()
        data = nodes_from_bsg(content)
        self.app.nodes = data.nodes
        self.app.selected_node_index = data.selected_node_index
        self.app.node_list_frame.update_node_list(self.app.nodes)

        if data.selected_node_index:
            self.app.config_frame.show_details(data.nodes[data.selected_node_index])

        self.tool_bar.proj_name_entry.textvariable.set(data.project_name)
        log.debug("Loaded content from %s", fp)

    def on_export_btn(self):
        """used to export to a file"""
        fp = fd.asksaveasfilename(
            defaultextension=".bsg",
            initialfile=self.tool_bar.proj_name_entry.textvariable.get(),
            filetypes=[("json file", "*.json"), ("c-sharp file", "*.cs")],
        )
        if not fp:
            log.info("No file path found after asksaveasfilename")
            return

        content = ""

        if fp.endswith(".cs"):
            content = nodes_to_cs(self.app.nodes)
        else:
            content = nodes_to_json(self.app.nodes)

        with open(fp, "w", encoding="utf-8") as f:
            f.write(content)
        log.debug("Exportet content to %s", fp)

    def on_new_project(self):
        """Used to load a new project"""
        content = "[]"
        self.app.nodes = json.loads(content)
        self.app.node_list_frame.update_node_list(self.app.nodes)
        self.app.config_frame.show_details({})
        log.debug("New Project init")

    def auto_save(self):
        """Used to save to a file"""
        fn = (
            self.tool_bar.proj_name_entry.get()
            + "_"
            + time.strftime("%Y%m%d_%H%M%S")
            + ".bsg"
        )
        fp = AUTO_SAVE_PATH / fn
        content = nodes_to_bsg(
            BSGData(
                self.app.nodes,
                self.app.selected_node_index,
                self.tool_bar.proj_name_entry.get(),
            )
        )
        with open(fp, "w", encoding="utf-8") as f:
            f.write(content)
        log.debug("Auto Saved content to %s", fp)

        self.after(self.auto_save_interval, self.auto_save)

    def on_closing(self):
        """Gets run when window gets closed"""
        self.auto_save()
        self.destroy()
