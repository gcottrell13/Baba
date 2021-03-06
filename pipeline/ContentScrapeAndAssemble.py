from PIL import Image
import glob
import re
import pathlib
from collections import defaultdict
import json
import math

from vars import *
from load_object_information import load_information
from imageUtils import *

info_re = re.compile(r"(?P<name>[\w_]+?)_(?P<phase>\d{1,2})_(?P<wobble>\d)\.png")

AnalysisResult = dict[str, dict[str, list[list[list[int]]]]]
ImageCollection = dict[str, dict[int, dict[int, Image.Image]]]


OBJECT_INFO, COLORS = load_information()

ALL_FILES = glob.glob(
    str(CUSTOM_SPRITES_PATH / "*.png")
)

class RecursiveDefaultDict(defaultdict):
    def __init__(self, default):
        self.default = default
        super().__init__(lambda: RecursiveDefaultDict(default))


def get_text_name(name: str):
    if name in WORD_MAP:
        return get_text_name(WORD_MAP[name])
    return f"text_{name}"


def output_facing_and_animation(name: str, images: dict[int, dict[int, Image.Image]]):
    directions: dict[str, list[list[Image.Image]]] = defaultdict(list)

    output_data: dict[str, list[list[str]]] = defaultdict(list)

    # print(sorted(images.keys()))

    for phase in sorted(images.keys()):
        if phase in SLEEP:
            direc = SLEEP[phase]
        elif phase < 8:
            direc = "right"
        elif phase < 16:
            direc = "up"
        elif phase < 24:
            direc = "left"
        else:
            direc = "down"

        directions[direc].append(
            [wobble_sprite for wobble_sprite in images[phase].values()]
        )
        output_data[direc].append(
            [f"{phase}_{wobble}" for wobble in images[phase].keys()]
        )

        # print(name, phase, len(images[phase]))

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

    return output_data


def output_simple(name: str, images: dict[int, dict[int, Image.Image]]):
    output_data = {
        "all": [
            [f"{phase}_{wobble}" for wobble in p.keys()] for phase, p in images.items()
        ],
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

    if not name.startswith("text_") or name[5:] not in OBJECT_INFO:
        save_gif(name, output_images)
    return output_data


def output_joinable(name: str, images: dict[int, dict[int, Image.Image]]):
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


def create_sheet(images: list[Image.Image], width: int = None):
    if width:
        height = math.ceil(len(images) / width)
        output_image = get_new_gif(1, width, height)
    else:
        square = math.ceil(math.sqrt(len(images)))
        width = square
        height = square
        output_image = get_new_gif(1, square, square)
    
    mapping = []

    for index, image in enumerate(images):
        coord = [index % width, index // width]
        mapping.append(coord)
        copy_animation_across([image], output_image, get_output_coord(*coord))

    return output_image, mapping


def open_all_images() -> ImageCollection:
    objects = RecursiveDefaultDict(int)
    for file in ALL_FILES:
        try:
            name, phase, wobble = info_re.match(pathlib.Path(file).name).groups()
            phase, wobble = int(phase), int(wobble)
            img = load_image(name, file)
            objects[name][phase][wobble] = img
        except:
            print(file)
    return objects


def pack_images_into_spritesheet(
    name: str,
    data: dict[str, list[list[str]]],
    images: dict[int, dict[int, Image.Image]],
):
    if data == {"type": "joinable"}:
        all_images = {
            f"{phase}_{wobble}": image
            for phase in range(16)
            for wobble, image in images[phase].items()
        }
    else:
        all_images: dict[str, Image.Image] = {
            f"{phase}_{wobble}": image
            for phase, wobbles in images.items()
            for wobble, image in wobbles.items()
        }

    reverse_map = [(k, v) for k, v in all_images.items()]
    sheet, coordinates = create_sheet([v for k, v in reverse_map])

    mapping = {
        image_name: coordinates[index]
        for index, (image_name, image) in enumerate(reverse_map)
    }

    save_spritesheet(name, sheet)

    try:
        return {k: [[mapping[j] for j in i] for i in v] for k, v in data.items()}
    except KeyError as e:
        return {
            str(phase): [
                [mapping[j] for j in all_images.keys() if j.startswith(f"{phase}_")]
            ]
            for phase in range(16)
        }


def pack_images_into_tileset(
    images: ImageCollection,
    existing_tileset_info: list[str],
):
    representatives: list[Image.Image] = []
    names: list[str] = []

    new_order: list[str] = []

    def add(name, animations):
        new_order.append(name)
        if 4 in animations:
            names.append(name)
            representatives.append(colorized_image(name, animations[0][1]))
        elif 24 in animations:
            names.append(f"{name}-right")
            names.append(f"{name}-up")
            names.append(f"{name}-left")
            names.append(f"{name}-down")

            representatives.extend(
                [
                    colorized_image(name, animations[0][1]),
                    colorized_image(name, animations[8][1]),
                    colorized_image(name, animations[16][1]),
                    colorized_image(name, animations[24][1]),
                ]
            )
        else:
            names.append(name)
            representatives.append(colorized_image(name, animations[0][1]))

    for name in existing_tileset_info:
        add(name, images[OBJECT_INFO[name].get('sprite', name)])

    existing_objects = set(existing_tileset_info)
    for name, info in OBJECT_INFO.items():

        animations = images[info.get('sprite', name)]

        if name.startswith('text_'):
            if name[5:] in OBJECT_INFO:
                continue

        if name not in existing_objects:
            add(name, animations)

            txt = f'text_{name}'
            if txt in OBJECT_INFO:
                add(txt, images[txt])

    sheet, coordinates = create_sheet(representatives, width=24)

    sprite_sheet_info = {
        f"{index+1}": names[index] for index, (x, y) in enumerate(coordinates)
    }

    save_spritesheet("tileset", sheet)

    return sprite_sheet_info, new_order


def analyze_images(images: ImageCollection) -> AnalysisResult:

    OUTPUT: AnalysisResult = {}
    for name, info in OBJECT_INFO.items():

        values = images.get(info.get('sprite', name))
        output = None

        has_facing = all(direction in values for direction in DIRECTIONS) and name not in ['cliff']
        if has_facing and len(values) == 4:
            # this object has separate sprites for each direction
            print(f"{name} has facing sprites")
            output = output_facing_and_animation(name, values)

        elif has_facing and len(values) > 4:
            # this object has separate sprites for each direction, AND has animations
            print(f"{name} has facing sprites AND animations")
            output = output_facing_and_animation(name, values)

        elif len(values) == 16 or name == 'cliff':
            # this object can 'join' with others nearby, like WALL or WATER
            print(f"{name} can join with others")
            output_joinable(name, values)
            output = {
                "type": "joinable",
            }

        elif len(values) > 1:
            # this object has no direction, but does have animation sprites
            print(f"{name} has an animation, but no facing sprites")
            output = output_simple(name, values)
        else:
            # this object is simple
            print(f"{name} is simple")
            output = output_simple(name, values)

        if output:
            output = pack_images_into_spritesheet(name, output, values)
            OUTPUT[name] = output

    with open(OUTPUT_DIRECTORY / f"json/ANIMATIONS.json", "w") as f:
        f.write(json.dumps(OUTPUT, indent=4))

    return OUTPUT


def create_tileset(images: ImageCollection):

    with open(STORE_PATH / f"tileset_order.json", "r") as f:
        existing_tileset_info: list[str] = json.loads(f.read())
        sheet_info, new_order = pack_images_into_tileset(images, existing_tileset_info)

    with open(STORE_PATH / f"tileset_order.json", "w") as f:
        f.write(json.dumps(new_order, indent=4))

    with open(OUTPUT_DIRECTORY / f"json/TILESET.json", "w") as f:
        f.write(json.dumps(sheet_info, indent=4))


def save_object_info():
    with open(OUTPUT_DIRECTORY / f'json/OBJECTS.json', 'w') as f:
        f.write(json.dumps(OBJECT_INFO, indent=4))


if __name__ == "__main__":
    images = open_all_images()
    analyze_images(images)
    create_tileset(images)
    save_object_info()
