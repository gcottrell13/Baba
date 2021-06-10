import pathlib
import re

info_re = re.compile(r"(?P<name>[\w_]+?)_(?P<phase>\d{1,2})_(?P<wobble>\d)\.png")

FILES_PATH = pathlib.Path("E:/SteamLibrary/steamapps/common/Baba Is You/Data")

SPRITES_PATH = FILES_PATH / 'Sprites'
VALUES_FILE_PATH = FILES_PATH / 'values.lua'

CUSTOM_FILES_PATH = pathlib.Path(__file__).absolute().parent.parent
CUSTOM_SPRITES_PATH = CUSTOM_FILES_PATH / 'custom_images'
CUSTOM_VALUES_FILE_PATH = CUSTOM_FILES_PATH / 'json/CustomColors.json'

OUTPUT_DIRECTORY = pathlib.Path(__file__).absolute().parent.parent
