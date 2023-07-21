import sys
import pickle
from PyQt5.QtWidgets import QApplication, QMainWindow, QFileDialog, QVBoxLayout, QWidget, QLabel, QLineEdit, QPushButton, QCheckBox, QListWidget, QListWidgetItem, QMenuBar, QMenu, QAction
from PyQt5.QtCore import Qt
from CMS import CMS
from CMS import CMS_Data

class Form1(QMainWindow):

    try:
        def __init__(self):
            super().__init__()

            self.FileName = ""
            self.cms = []
            self.current = None
            self.Copy = None
            self.canPaste = False
            self.lck = True
            self.selective = []

            self.init_ui()

        def init_ui(self):
            self.setWindowTitle("CMSEditor")
            self.setGeometry(100, 100, 600, 400)

            layout = QVBoxLayout()
            main_widget = QWidget()
            main_widget.setLayout(layout)
            self.setCentralWidget(main_widget)

            menubar = self.menuBar()
            file_menu = menubar.addMenu("File")

            open_action = QAction("Open", self)
            open_action.triggered.connect(self.open_file)
            file_menu.addAction(open_action)

            save_action = QAction("Save", self)
            save_action.triggered.connect(self.save_file)
            file_menu.addAction(save_action)

            file_menu.addSeparator()

            selective_save_action = QAction("Selective Save", self)
            selective_save_action.triggered.connect(self.selective_save)
            file_menu.addAction(selective_save_action)

            self.cbList = QListWidget()
            self.cbList.currentRowChanged.connect(self.on_item_selected)
            layout.addWidget(self.cbList)

            # Create "Tools" menu
            tools_menu = menubar.addMenu("Tools")

            # Add action to add an item
            add_action = QAction("Add", self)
            add_action.triggered.connect(self.add_item)
            tools_menu.addAction(add_action)

            # Add action to remove an item
            remove_action = QAction("Remove", self)
            remove_action.triggered.connect(self.remove_item)
            tools_menu.addAction(remove_action)

            # Add action to copy an item
            copy_action = QAction("Copy", self)
            copy_action.triggered.connect(self.copy_item)
            tools_menu.addAction(copy_action)

            # Add action to paste an item
            paste_action = QAction("Paste", self)
            paste_action.triggered.connect(self.paste_item)
            tools_menu.addAction(paste_action)

            # Create the checkbox
            self.checkBox1 = QCheckBox("Selective Save", self)
            self.checkBox1.stateChanged.connect(self.on_checkBox1_changed)
            layout.addWidget(self.checkBox1)

            # Dark UI Style Sheet
            style_sheet = """
            QWidget {
                background-color: #333;
                color: #ddd;
                selection-background-color: #555;
                selection-color: #eee;
            }
            QLabel {
                color: #aaa;
            }
            QLineEdit, QListWidget {
                background-color: #444;
                color: #ddd;
            }
            QPushButton {
                background-color: #555;
                color: #ddd;
                border: 1px solid #777;
                padding: 5px;
            }
            QPushButton:hover {
                background-color: #666;
            }
            QPushButton:pressed {
                background-color: #333;
            }
            QCheckBox {
                color: #aaa;
            }
            """
            self.setStyleSheet(style_sheet)

            self.txtChar = QLineEdit()
            self.txtChar.textChanged.connect(self.on_txtChar_changed)
            layout.addWidget(QLabel("ID:"))
            layout.addWidget(self.txtChar)

            self.txtShortname = QLineEdit()
            self.txtShortname.textChanged.connect(self.on_txtShortname_changed)
            layout.addWidget(QLabel("Shortname:"))
            layout.addWidget(self.txtShortname)

            self.txtUnk1 = QLineEdit()
            self.txtUnk1.textChanged.connect(self.on_txtUnk1_changed)
            layout.addWidget(QLabel("Unk1:"))
            layout.addWidget(self.txtUnk1)

            self.txtUnk2 = QLineEdit()
            self.txtUnk2.textChanged.connect(self.on_txtUnk2_changed)
            layout.addWidget(QLabel("Unk2:"))
            layout.addWidget(self.txtUnk2)

            self.txtUnk3 = QLineEdit()
            self.txtUnk3.textChanged.connect(self.on_txtUnk3_changed)
            layout.addWidget(QLabel("Unk3:"))
            layout.addWidget(self.txtUnk3)

            self.txtUnk4 = QLineEdit()
            self.txtUnk4.textChanged.connect(self.on_txtUnk4_changed)
            layout.addWidget(QLabel("Unk4:"))
            layout.addWidget(self.txtUnk4)

            self.txtUnk5 = QLineEdit()
            self.txtUnk5.textChanged.connect(self.on_txtUnk5_changed)
            layout.addWidget(QLabel("Unk5:"))
            layout.addWidget(self.txtUnk5)

            self.txtPaths = []
            for i in range(7):
                txtPath = QLineEdit()
                txtPath.textChanged.connect(lambda text, i=i: self.on_txtPath_changed(text, i))
                self.txtPaths.append(txtPath)
                layout.addWidget(QLabel(f"Path {i+1}:"))
                layout.addWidget(txtPath)

        def open_file(self):
            options = QFileDialog.Options()
            options |= QFileDialog.ReadOnly
            fileName, _ = QFileDialog.getOpenFileName(self, "Browse for CMS File", "", "Xenoverse Char_Model_Spec (*.cms);;All Files (*)", options=options)
            if not fileName:
                return

            self.FileName = fileName
            self.cms = CMS()
            self.cms.Load(self.FileName)
            self.selective = [False] * len(self.cms.Data)  # Corrected line
            self.cbList.clear()
            for char_model in self.cms.Data:
                self.cbList.addItem(f"{char_model.ID:03d} - {char_model.ShortName}")

        def save_file(self):
            if self.FileName:
                self.cms.Save()
                print("File has been saved!")

        def add_item(self):
            c = CMS_Data()  # Create a CMS_Data object
            c.Paths = [""] * 7
            c.Unknown = [0] * 5  # Initialize the Unknown attribute with five elements
            self.cms.Data.append(c)  # Append the CMS_Data object to the Data list
            self.cbList.addItem(f"{c.ID:03d} - {c.ShortName}")
            self.selective.append(False)


            # Update the current selection to the newly added item
            self.current = c
            self.txtChar.setText(str(self.current.ID))
            self.txtShortname.setText(self.current.ShortName)
            self.txtUnk1.setText(str(self.current.Unknown[0]))
            self.txtUnk2.setText(str(self.current.Unknown[1]))
            self.txtUnk3.setText(str(self.current.Unknown[2]))
            self.txtUnk4.setText(str(self.current.Unknown[3]))
            self.txtUnk5.setText(str(self.current.Unknown[4]))
            for i in range(7):
                self.txtPaths[i].setText(self.current.Paths[i])
            self.cbList.setCurrentRow(self.cbList.count() - 1)  # Set the current selection to the newly added item


        def remove_item(self):
            if self.cbList.currentRow() >= 0:
                del self.cms.Data[self.cbList.currentRow()]  # Corrected line to remove from the Data list
                del self.selective[self.cbList.currentRow()]
                self.cbList.takeItem(self.cbList.currentRow())

        def copy_item(self):
            if self.cbList.currentRow() >= 0:
                self.Copy = CMS_Data()
                self.Copy.__dict__ = self.cms.Data[self.cbList.currentRow()].__dict__

        def paste_item(self):
            if self.Copy:
                self.current = CMS_Data()
                self.current.__dict__ = self.Copy.__dict__
                self.cms.Data[self.cbList.currentRow()] = self.current  # Corrected line
                self.txtChar.setText(str(self.current.ID))
                self.txtShortname.setText(self.current.ShortName)
                self.txtUnk1.setText(str(self.current.Unknown[0]))
                self.txtUnk2.setText(str(self.current.Unknown[1]))
                self.txtUnk3.setText(str(self.current.Unknown[2]))
                self.txtUnk4.setText(str(self.current.Unknown[3]))
                self.txtUnk5.setText(str(self.current.Unknown[4]))
                for i in range(7):  # Changed from 6 to 7 to match the number of paths
                    self.txtPaths[i].setText(self.current.Paths[i])
                self.cbList.setCurrentRow(self.cbList.currentRow())

        def selective_save(self):
            SelectCMS = [self.cms.Data[i] for i, s in enumerate(self.selective) if s]
            if SelectCMS:
                file_name, _ = QFileDialog.getSaveFileName(self, "Save Selective CMS File", "", "Xenoverse Char_Model_Spec (*.cms);;All Files (*)")
                if file_name:
                    with open(file_name, 'wb') as file:
                        pickle.dump(SelectCMS, file)
                        
        def on_item_selected(self, index):
            if index >= 0:
                self.lck = False
                self.current = self.cms.Data[index]  # Accessing CMS_Data from the list
                self.txtChar.setText(str(self.current.ID))
                self.txtShortname.setText(self.current.ShortName)
                self.txtUnk1.setText(str(self.current.Unknown[0]))
                self.txtUnk2.setText(str(self.current.Unknown[1]))
                self.txtUnk3.setText(str(self.current.Unknown[2]))
                self.txtUnk4.setText(str(self.current.Unknown[3]))
                self.txtUnk5.setText(str(self.current.Unknown[4]))
                for i in range(7):
                    self.txtPaths[i].setText(self.current.Paths[i])
                self.checkBox1.setChecked(self.selective[index])
                self.lck = True

        def on_txtChar_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                current_row = self.cbList.currentRow()
                self.current.id = int(text)
                self.cms.Data[current_row].ID = self.current.id
                self.update_cbList()
                self.cbList.setCurrentRow(current_row)  # Set the selected row again
                self.lck = True

        def on_txtShortname_changed(self, text):
            if self.lck and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.ShortName = text
                self.lck = True

        def on_txtUnk1_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Unknown = bytearray(self.current.Unknown)  # Convert bytes to a mutable bytearray
                self.current.Unknown[0] = int(text)
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_txtUnk2_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Unknown = bytearray(self.current.Unknown)  # Convert bytes to a mutable bytearray
                self.current.Unknown[1] = int(text)
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_txtUnk3_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Unknown = bytearray(self.current.Unknown)  # Convert bytes to a mutable bytearray
                self.current.Unknown[2] = int(text)
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_txtUnk4_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Unknown = bytearray(self.current.Unknown)  # Convert bytes to a mutable bytearray
                self.current.Unknown[3] = int(text)
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_txtUnk5_changed(self, text):
            if self.lck and text.isdigit() and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Unknown = bytearray(self.current.Unknown)  # Convert bytes to a mutable bytearray
                self.current.Unknown[4] = int(text)
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_txtPath_changed(self, text, index):
            if self.lck and self.cbList.currentRow() >= 0:
                self.lck = False
                self.current = self.cms.Data[self.cbList.currentRow()]  # Accessing CMS_Data from the list
                self.current.Paths[index] = text
                self.cms.Data[self.cbList.currentRow()] = self.current  # Update the modified object back into the list
                self.lck = True

        def on_checkBox1_changed(self, state):
            if self.cbList.currentRow() >= 0:
                self.selective[self.cbList.currentRow()] = state == Qt.Checked

        def update_cbList(self):
            self.cbList.clear()
            for char_model in self.cms.Data:
                self.cbList.addItem(f"{char_model.ID:03d} - {char_model.ShortName}")
    except Exception as e:
        print("Exception has occurred: " + str(e))

if __name__ == "__main__":
    app = QApplication(sys.argv)
    form = Form1()

    form.show()
    sys.exit(app.exec_())

