import pathlib
import re
import os

info_re = re.compile(r"(?P<name>[\w_]+?)_(?P<phase>\d{1,2})_(?P<wobble>\d)\.png")

FILES_PATH = pathlib.Path(r"C:\Program Files (x86)\Steam\steamapps\common\Baba Is You") / "Data"

PIPELINE_PATH = pathlib.Path(__file__).absolute().parent

del pathlib
del re

SPRITES_PATH = FILES_PATH / 'Sprites'
VALUES_FILE_PATH = FILES_PATH / 'Editor' / 'editor_objectlist.lua'

CUSTOM_FILES_PATH = PIPELINE_PATH / 'custom'
CUSTOM_SPRITES_PATH = CUSTOM_FILES_PATH / 'images'
CUSTOM_VALUES_FILE_PATH = CUSTOM_FILES_PATH / 'CustomColors.json'

STORE_PATH = PIPELINE_PATH / 'store'

VISUAL_OUTPUT_DIRECTORY = PIPELINE_PATH / 'baba_analysis'
OUTPUT_DIRECTORY = PIPELINE_PATH.parent / 'BabaSource' / 'BabaGame' / 'Content'

MAPS_DIRECTORY = PIPELINE_PATH.parent / 'BabaSource' / 'BabaGame' / 'Content' / 'Maps'

os.makedirs(VISUAL_OUTPUT_DIRECTORY, exist_ok=True)
os.makedirs(OUTPUT_DIRECTORY / 'Sheets', exist_ok=True)
os.makedirs(MAPS_DIRECTORY, exist_ok=True)

WIDTH = 24
HEIGHT = 24
WPAD = 2
HPAD = 2

DIRECTIONS = {
    0: "right",
    8: "up",
    16: "left",
    24: "down",
}

SLEEP = {
    7: "sleep_up",
    15: "sleep_left",
    23: "sleep_down",
    31: "sleep_right",
}

JOINABLE_FLAGS = {
    1: "right",
    2: "up",
    4: "left",
    8: "down",
}

WORD_MAP = {
    "seastar": "star",
    "tree2": "tree",
}

IMAGE_MODE = "RGBA"


up = r"text_up"
left = r"text_left"
right = r"text_right"
down = r"text_down"
