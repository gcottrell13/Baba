#! py -3.11

import json
from modules.analysis import ANALYSIS_OUTPUT, analyze_facing_and_animation, analyze_joinable, analyze_simple
from modules.output_gifs.imagesToSpritesheet import pack_images_into_spritesheet
from modules.types import ImageCollection, AnalysisResult

from modules.vars import DIRECTIONS, STORE_PATH, OUTPUT_DIRECTORY
from modules.load_object_information import load_information
from modules.imageUtils import *
from modules.output_tileset import pack_images_into_tileset



def analyze_images(images: ImageCollection) -> AnalysisResult:
    OBJECT_INFO, _ = load_information()

    OUTPUT: AnalysisResult = {}
    for name, info in OBJECT_INFO.items():

        values = images.get(info.get('sprite', name))
        output: ANALYSIS_OUTPUT = {}

        if not values:
            continue

        has_facing = all(
            direction in values for direction in DIRECTIONS
        ) and name not in ['cliff']
        if has_facing and len(values) == 4:
            # this object has separate sprites for each direction
            print(f"{name} has facing sprites")
            output = analyze_facing_and_animation(values)

        elif has_facing and len(values) > 4:
            # this object has separate sprites for each direction, AND has animations
            print(f"{name} has facing sprites AND animations")
            output = analyze_facing_and_animation(values)

        elif len(values) == 16 or name == 'cliff':
            # this object can 'join' with others nearby, like WALL or WATER
            print(f"{name} can join with others")
            output = analyze_joinable()

        elif len(values) > 1:
            # this object has no direction, but does have animation sprites
            print(f"{name} has an animation, but no facing sprites")
            output = analyze_simple(values)
        else:
            # this object is simple
            print(f"{name} is simple")
            output = analyze_simple(values)

        if output:
            OUTPUT[name] = pack_images_into_spritesheet(name, output, values)

    with open(OUTPUT_DIRECTORY / f"json/ANIMATIONS.json", "w") as f:
        f.write(json.dumps(OUTPUT, indent=4))

    return OUTPUT


def create_tileset(images: ImageCollection):

    with open(STORE_PATH / f"tileset_order.json", "r") as f:
        existing_tileset_info: list[str] = json.loads(f.read())
        sheet_info, new_order = pack_images_into_tileset(
            images, existing_tileset_info)

    with open(STORE_PATH / f"tileset_order.json", "w") as f:
        f.write(json.dumps(new_order, indent=4))

    with open(OUTPUT_DIRECTORY / f"json/TILESET.json", "w") as f:
        f.write(json.dumps(sheet_info, indent=4))


def save_object_info():
    OBJECT_INFO, _ = load_information()
    with open(OUTPUT_DIRECTORY / f'json/OBJECTS.json', 'w') as f:
        f.write(json.dumps(OBJECT_INFO, indent=4))


if __name__ == "__main__":
    images = open_all_images()
    analyze_images(images)
    create_tileset(images)
    save_object_info()
