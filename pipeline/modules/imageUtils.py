import functools
import glob
import pathlib
from PIL import Image

from modules.formats import ImageCollection
from modules.vars import ALL_FILES, CUSTOM_FILES_PATH, IMAGE_MODE, SPRITES_PATH, info_re, WIDTH, HEIGHT, WPAD, HPAD

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


@functools.lru_cache
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
        except Exception as e:
            print(file, e)
    return objects


def resize_sprites(sprites: list[Image.Image], scale: float) -> list[Image.Image]:
    return [
        img.resize((int(img.width * scale), int(img.height * scale)))
        for img in sprites
    ]


def copy_animation_across(
        animation: list[Image.Image], destination: list[Image.Image], pixel_coord: tuple[int, int], center=True,
        block_width: int = None, block_height: int = None,
):
    block_width = WIDTH if block_width is None else block_width
    block_height = HEIGHT if block_height is None else block_height

    for i, dest in enumerate(destination):
        anim = animation[i % len(animation)]
        if center:
            x = pixel_coord[0] - (anim.width - block_width) // 2
            y = pixel_coord[1] - (anim.height - block_height) // 2
            dest.paste(anim, (x, y))
        else:
            dest.paste(anim, pixel_coord)


def get_output_coord(block_x: int, block_y: int,
                     block_width: int = None, block_height: int = None,
                     wpad=None,
                     hpad=None):
    wpad = WPAD if wpad is None else wpad
    hpad = HPAD if hpad is None else hpad
    block_width = WIDTH if block_width is None else block_width
    block_height = HEIGHT if block_height is None else block_height
    return wpad + (block_width + wpad) * block_x, hpad + (block_height + hpad) * block_y


def get_new_gif(num_frames: int, width_in_blocks: int, height_in_blocks: int,
                block_width: int = None, block_height: int = None,
                wpad=None, hpad=None):
    new_width, new_height = get_output_coord(width_in_blocks, height_in_blocks, block_width, block_height, wpad, hpad)
    return [
        Image.new(IMAGE_MODE, (new_width, new_height), (0, 0, 0, 0))
        for _ in range(num_frames)
    ]
