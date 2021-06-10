import pathlib
import re

info_re = re.compile(r"(?P<name>[\w_]+?)_(?P<phase>\d{1,2})_(?P<wobble>\d)\.png")

FILES_PATH = pathlib.Path("E:/SteamLibrary/steamapps/common/Baba Is You/Data")

SPRITES_PATH = FILES_PATH / 'Sprites'
VALUES_FILE_PATH = FILES_PATH / 'Editor' / 'editor_objectlist.lua'

CUSTOM_FILES_PATH = pathlib.Path(__file__).absolute().parent / 'custom'
CUSTOM_SPRITES_PATH = CUSTOM_FILES_PATH / 'images'
CUSTOM_VALUES_FILE_PATH = CUSTOM_FILES_PATH / 'CustomColors.json'

STORE_PATH = pathlib.Path(__file__).absolute().parent / 'store'

VISUAL_OUTPUT_DIRECTORY = pathlib.Path(__file__).absolute().parent / 'baba_analysis'
OUTPUT_DIRECTORY = pathlib.Path(__file__).absolute().parent.parent / 'BabaSource' / 'BabaGame' / 'Content'
