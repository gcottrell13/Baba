import json
import math
import xml.etree.ElementTree as ET
import glob
import pathlib
from PIL import Image
from vars import MAPS_DIRECTORY, OUTPUT_DIRECTORY, PIPELINE_PATH
from imageUtils import *
from load_object_information import load_information

INFO, COLORS = load_information()

MAP_WIDTH = 24
MAP_HEIGHT = 18


def save_thumbnail(name: str, frames: list[Image.Image]):
    frames[0].save(str(PIPELINE_PATH / f"thumbs/{name}.png"))


def load_xml(filename: str) -> ET.ElementTree:
    return ET.parse(filename)


def parse_group(group_name: str, group: dict[str, ET.ElementTree]):
    output: dict[str, list[list[str]]] = {}
    for map_name, doc in group.items():
        rows = doc.getroot()[1][0].text.strip().splitlines()
        if len(rows) != MAP_HEIGHT:
            raise Exception()
        data = [row.split(",") for row in rows]
        output[map_name] = data
    return output


def group_to_tileset(
    group_name: str, group: dict[str, list[list[str]]], tileset_info: dict[str, str]
):
    square = math.ceil(math.sqrt(len(group)))
    sheet = get_new_gif(1, MAP_WIDTH * square, MAP_HEIGHT * square, wpad=0, hpad=0)

    for i, map_name in enumerate(sorted(group.keys())):
        data = group[map_name]

        thumbnail = get_new_gif(1, MAP_WIDTH, MAP_HEIGHT, wpad=0, hpad=0)

        for y, row in enumerate(data):
            for x, item_id in enumerate(row):
                if item_name := tileset_info.get(item_id, None):

                    if "-" in item_name:
                        item_name, direction = item_name.split("-")
                    else:
                        direction = "up"

                    info = INFO[item_name]

                    sprites = load_sprites(info["sprite"])
                    sprites = [colorized_image(item_name, sprites[0])]
                    copy_animation_across(
                        sprites, thumbnail, get_output_coord(x, y, wpad=0, hpad=0)
                    )

        mx = i // square
        my = i % square
        copy_animation_across(
            thumbnail,
            sheet,
            get_output_coord(mx * MAP_WIDTH, my * MAP_HEIGHT, wpad=0, hpad=0),
            center=False,
        )

        thumbnail = resize_sprites(thumbnail, 0.25)
        save_thumbnail(map_name, thumbnail)
        print(f"created thumbnail {map_name}")

    sheet = resize_sprites(sheet, 0.25)
    save_spritesheet(f"{group_name}_map", sheet)


def create_groups():
    GROUPS: dict[str, dict[str, ET.ElementTree]] = {}

    maps = glob.glob(str(MAPS_DIRECTORY / "*.tmx"))
    for file in maps:
        if "TEMPLATE" in file:
            continue

        map_name = pathlib.Path(file).name.removesuffix(".tmx")

        group = map_name.split("_")[0]
        if group not in GROUPS:
            GROUPS[group] = {}

        GROUPS[group][map_name] = load_xml(file)

    return GROUPS


if __name__ == "__main__":
    groups = create_groups()

    with open(OUTPUT_DIRECTORY / f"json/TILESET.json", "r") as f:
        TILESET: dict[str, str] = json.loads(f.read())

    for group_name, group in groups.items():
        data = parse_group(group_name, group)
        group_to_tileset(group_name, data, TILESET)
