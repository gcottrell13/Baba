import functools
import glob
import pathlib
from PIL import Image

from modules.types import ImageCollection
from .vars import ALL_FILES, CUSTOM_FILES_PATH, IMAGE_MODE, SPRITES_PATH, info_re


CACHED_SPRITES: dict[str, list[Image.Image]] = {}

__all__ = [
    'load_image',
    'open_all_images',
    'load_sprites',
]


@functools.lru_cache
def load_sprites(name: str) -> list[Image.Image]:
    matches = glob.glob(str(SPRITES_PATH / f"{name}_*.png"))
    if not matches:
        matches = glob.glob(str(CUSTOM_FILES_PATH / f"{name}_*.png"))
    images = [load_image(file) for file in matches]
    return images
    
def load_image(file: str):
    img = Image.open(file).convert(IMAGE_MODE)
    return img


@functools.lru_cache
def open_all_images() -> ImageCollection:
    objects: ImageCollection = {}
    for file in ALL_FILES:
        try:
            m = info_re.match(pathlib.Path(file).name)
            if not m:
                continue
            name, phase, wobble = m.groups()
            phase, wobble = int(phase), int(wobble)
            img = load_image(file)
            objects.setdefault(name, {}).setdefault(phase, {})[wobble] = img
        except:
            print(file)
    return objects
