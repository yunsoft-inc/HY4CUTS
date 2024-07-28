import sys
import os
import time
from PyQt5.QtWidgets import *
from PyQt5 import uic
from PyQt5.QtGui import *

def resource_path(relative_path):
    base_path = getattr(sys, "_MEIPASS", os.path.dirname(os.path.abspath(__file__)))
    return os.path.join(base_path, relative_path)

startup = resource_path('startup.ui')
startup_class = uic.loadUiType(startup)[0]
photo_type_sel = resource_path('photo_type_sel.ui')
photo_type_sel_class = uic.loadUiType(photo_type_sel)[0]

class startup(QMainWindow, startup_class):
    def __init__(self):
        super().__init__()
        self.setupUi(self)
        self.go_main_btn.clicked.connect(self.photo_type_sel)
    def photo_type_sel(self):
        
        self.startup = photo_type_sel_class()
        self.startup.showFullScreen()
        self.close()
class photo_type_sel_class(QMainWindow, photo_type_sel_class):
    def __init__(self):
        super(photo_type_sel_class, self).__init__()
        self.setupUi(self)
        self.sel_ai_2.clicked.connect(self.startup)
    def startup(self):
        self.close()
        self.photo_type_sel = startup()
        self.photo_type_sel.showFullScreen()
if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = startup()
    window.showFullScreen()
    sys.exit(app.exec()) 