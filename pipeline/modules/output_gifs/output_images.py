
from collections import defaultdict
from PIL import Image
from modules.analysis import ANALYSIS_OUTPUT
from modules.imageUtils import load_sprites, open_all_images

from modules.load_object_information import load_information
from modules.utils import get_text_name

from ..vars import IMAGE_MODE, SLEEP, WORD_MAP, up, down, right, left


def output_facing_and_animation(name: str, data: ANALYSIS_OUTPUT):
    all_images = open_all_images()
    directions: dict[str, list[list[Image.Image]]] = {
        direc: [
            [
                all_images[name][phase][wobble]
                for wobble in wobbles
            ]
            for phase, wobbles in phases.items()
        ]
        for direc, phases in data.items()
    }

    animation_lengths = {len(anim) for _, anim in directions.items()} | {3}

    layout = [
        ["name_active", up, down, right, left],
        ["name_inactive", "up", "down", "right", "left"],
    ]

    if any(key in directions for key in SLEEP.values()):
        layout.append(
            ["text_sleep", "sleep_up", "sleep_down", "sleep_right", "sleep_left"]
        )

    animation_length = 1
    for l in animation_lengths:
        animation_length *= l

    output_images = get_new_gif(
        animation_length, max([len(row) for row in layout]), len(layout)
    )

    for row_id, row in enumerate(layout):
        for col_id, item in enumerate(row):
            if not item:
                continue

            sprites = []
            if item in [up, left, down, right, "text_sleep"]:
                sprites = load_sprites(item)
                sprites = colorized_images(item, sprites)
            elif item in ["up", "left", "down", "right"]:
                sprites = [i for j in directions[item] for i in j]
                sprites = colorized_images(name, sprites)
            elif item in ["sleep_up", "sleep_left", "sleep_down", "sleep_right"]:
                sprites = [i for j in directions[item] for i in j]
                sprites = colorized_images(name, sprites)
            elif item == "name_active":
                sprites = load_sprites(get_text_name(name))
                sprites = colorized_images(f"text_{name}", sprites)
            elif item == "name_inactive":
                sprites = load_sprites(get_text_name(name))
                sprites = colorized_images(f"text_{name}_inactive", sprites)
            # print(item, len(sprites))
            if sprites:
                copy_animation_across(
                    sprites, output_images, get_output_coord(col_id, row_id)
                )

    save_gif(name, output_images)

    # fun gifs
    alone_output = get_new_gif(
        12, 1, 1
    )
    copy_animation_across(colorized_images(name, [i for j in directions['right'] for i in j]), alone_output, (2, 2))
    save_gif(name + '_alone', alone_output)


def output_simple(name: str, data: ANALYSIS_OUTPUT):
    all_images = open_all_images()
    directions: dict[str, list[list[Image.Image]]] = {
        direc: [
            [
                all_images[name][phase][wobble]
                for wobble in wobbles
            ]
            for phase, wobbles in phases.items()
        ]
        for direc, phases in data.items()
    }

    if name.startswith("text"):
        layout = [['sprite', 'sprite_inactive']]
        alone_images = get_new_gif(
            3, 1, 1
        )
        sprites = [
            image for phase in images.values() for image in phase.values()
        ]
        copy_animation_across(
            colorized_images(name, sprites), alone_images, (2, 2)
        )
        save_gif(name + '_alone', alone_images)

    else:
        layout = [
            ["sprite", "name"],
            ["is", "name_inactive"],
        ]

    animation_length = len(images) * 3

    output_images = get_new_gif(
        animation_length, max([len(row) for row in layout]), len(layout)
    )
    for row_id, row in enumerate(layout):
        for col_id, item in enumerate(row):
            sprites = []
            if item == "name":
                sprites = load_sprites(get_text_name(name))
                sprites = colorized_images(f"text_{name}", sprites)
            elif item == "name_inactive":
                sprites = load_sprites(get_text_name(name))
                sprites = colorized_images(f"text_{name}_inactive", sprites)
            elif item == "is":
                sprites = load_sprites(f"text_is")
            elif item == "text":
                sprites = load_sprites(f"text_text")
                sprites = colorized_images("text_text", sprites)
            elif item == 'sprite_inactive':
                sprites = [
                    image for phase in images.values() for image in phase.values()
                ]
                sprites = colorized_images(f'{name}_inactive', sprites)
            elif item == "sprite":
                sprites = [
                    image for phase in images.values() for image in phase.values()
                ]
                sprites = colorized_images(name, sprites)

            if sprites:
                copy_animation_across(
                    sprites, output_images, get_output_coord(col_id, row_id)
                )

    OBJECT_INFO, COLORS = load_information()
    if not name.startswith("text_") or name[5:] not in OBJECT_INFO:
        save_gif(name, output_images)


def output_joinable(name: str, data: ANALYSIS_OUTPUT):
    output_images = get_new_gif(3, 12, 12)

    above = list(images[8].values())
    below = list(images[2].values())
    beside_right = list(images[4].values())
    beside_left = list(images[1].values())

    above = colorized_images(name, above)
    below = colorized_images(name, below)
    beside_right = colorized_images(name, beside_right)
    beside_left = colorized_images(name, beside_left)

    for mask in range(16):
        col = (mask % 4) * 3 + 1
        row = (mask // 4) * 3 + 1
        if mask & 2:
            copy_animation_across(above, output_images, get_output_coord(col, row - 1))
        if mask & 8:
            copy_animation_across(below, output_images, get_output_coord(col, row + 1))
        if mask & 4:
            copy_animation_across(
                beside_left, output_images, get_output_coord(col - 1, row)
            )
        if mask & 1:
            copy_animation_across(
                beside_right, output_images, get_output_coord(col + 1, row)
            )

        center = list(images[mask].values())
        center = colorized_images(name, center)
        copy_animation_across(center, output_images, get_output_coord(col, row))

    text_name = load_sprites(get_text_name(name))
    copy_animation_across(
        colorized_images(f"text_{name}", text_name),
        output_images,
        get_output_coord(0, 0),
    )
    copy_animation_across(
        colorized_images(f"text_{name}_inactive", text_name),
        output_images,
        get_output_coord(0, 1),
    )

    save_gif(name, output_images)


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

    _, COLORS = load_information()
    info = COLORS.get(name, {})
    default = (255, 255, 255)

    if inactive:
        p = info.get("color_inactive", default)
    else:
        p = info.get("color", default)

    pr, pg, pb = p[0] / 255, p[1] / 255, p[2] / 255
    r = r.point(lambda i: int(i * pr))
    g = g.point(lambda i: int(i * pg))
    b = b.point(lambda i: int(i * pb))
    return Image.merge(IMAGE_MODE, (r, g, b, a))


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
            dest.paste(anim, (x, y))
        else:
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
        disposal=2,
        optimize=True,
        loop=0,
    )


def save_spritesheet(name: str, frames: list[Image.Image]):
    frames[0].save(str(OUTPUT_DIRECTORY / f"Sheets/{name}.png"))
