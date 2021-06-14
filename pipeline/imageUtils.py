from PIL import Image
from vars import IMAGE_MODE, WORD_MAP
from load_object_information import load_information
from vars import *
import glob

OBJECT_INFO, COLORS = load_information()


CACHED_SPRITES: dict[str, list[Image.Image]] = {}

__all__ = [
    'load_image',
    'colorized_images',
    'colorized_image',
    'load_sprites',
    'copy_animation_across',
    'get_output_coord',
    'get_new_gif',
    'save_gif',
    'save_spritesheet',
    'resize_sprites',
]

def load_image(name: str, file: str):
    img = Image.open(file).convert(IMAGE_MODE)
    return img


def colorized_images(name: str, images: list[Image.Image]):
    return [colorized_image(name, img) for img in images]


def colorized_image(name: str, img: Image.Image):
    r, g, b, a = img.split()
    inactive = False

    if name.endswith("_inactive"):
        name = name[: -len("_inactive")]
        inactive = True

    if name in WORD_MAP:
        name = WORD_MAP[name]

    if name.startswith("text_") and (notxt := name.replace("text_", "")) in WORD_MAP:
        name = f"text_{WORD_MAP[notxt]}"

    info = COLORS.get(name, {})
    default = (255, 255, 255)

    if inactive:
        p = info.get("color_inactive", default)
    else:
        p = info.get("color", default)

    pr, pg, pb = p[0] / 255, p[1] / 255, p[2] / 255
    r = r.point(lambda i: i * pr)
    g = g.point(lambda i: i * pg)
    b = b.point(lambda i: i * pb)
    return Image.merge(IMAGE_MODE, (r, g, b, a))


def load_sprites(name: str) -> list[Image.Image]:
    if name in CACHED_SPRITES:
        return CACHED_SPRITES[name]

    matches = glob.glob(str(SPRITES_PATH / f"{name}_*.png"))
    if not matches:
        matches = glob.glob(str(CUSTOM_FILES_PATH / f"{name}_*.png"))
    images = [load_image(name, file) for file in matches]
    CACHED_SPRITES[name] = images
    return images


def resize_sprites(sprites: list[Image.Image], scale: float) -> list[Image.Image]:
    return [
        img.resize((int(img.width * scale), int(img.height * scale)))
        for img in sprites
    ]


def copy_animation_across(
    animation: list[Image.Image], destination: list[Image.Image], coord: tuple[int, int], center=True
):
    for i, dest in enumerate(destination):
        anim = animation[i % len(animation)]

        if center:
            x = coord[0] - (anim.width - WIDTH) // 2
            y = coord[1] - (anim.height - HEIGHT) // 2
            coord = (x, y)
        dest.paste(anim, coord)


def get_output_coord(block_x: int, block_y: int, wpad=None, hpad=None):
    wpad = WPAD if wpad is None else wpad
    hpad = HPAD if hpad is None else hpad
    return wpad + (WIDTH + wpad) * block_x, hpad + (HEIGHT + hpad) * block_y


def get_new_gif(num_frames: int, width_in_blocks: int, height_in_blocks: int, wpad=None, hpad=None):
    new_width, new_height = get_output_coord(width_in_blocks, height_in_blocks, wpad, hpad)
    return [
        Image.new(IMAGE_MODE, (new_width, new_height), (0, 0, 0, 0))
        for i in range(num_frames)
    ]


def save_gif(name: str, frames: list[Image.Image]):
    frames[0].save(
        str(VISUAL_OUTPUT_DIRECTORY / f"{name}.gif"),
        save_all=True,
        append_images=frames[1:],
        duration=300,
        optimize=True,
        loop=0,
    )


def save_spritesheet(name: str, frames: list[Image.Image]):
    frames[0].save(str(OUTPUT_DIRECTORY / f"Sheets/{name}.png"))
