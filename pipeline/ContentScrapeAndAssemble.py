#! py -3.11

import json
from modules.analysis import analyze_images
from modules.formats import ImageCollection, ObjectSprites
from modules.output_gifs.make_proofs import make_all_proofs

from modules.vars import STORE_PATH, OUTPUT_DIRECTORY
from modules.load_object_information import load_information
from modules.imageUtils import *
# from modules.output_tileset import pack_images_into_tileset


def create_tilesets(images: ImageCollection, results: dict[str, ObjectSprites]):
    sheet_info, new_order = pack_images_into_tileset(images)

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
    analysis_result = analyze_images(images, load_information()[0])
    # create_tilesets(images, analysis_result)
    # save_object_info()
    make_all_proofs(analysis_result)
