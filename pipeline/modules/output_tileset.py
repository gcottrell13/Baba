from PIL import Image
from modules.load_object_information import load_information
from modules.output_gifs.output_images import colorized_image, save_spritesheet
from modules.output_gifs.sheet import create_sheet
from modules.types import ImageCollection


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
    OBJECT_INFO, _ = load_information()
    for name in existing_tileset_info:
        id = OBJECT_INFO[name]["sprite"]
        add(name, images.get(id))

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
