import math
from PIL import Image

from modules.formats import Vector2
from modules.imageUtils import get_new_gif, copy_animation_across, get_output_coord


def create_sheet(images: list[Image.Image], width: int = 0) -> tuple[list[Image.Image], list[Vector2]]:
    block_width, block_height = map(max, zip(*map(lambda x: x.size, images)))
    if width:
        height = math.ceil(len(images) / width)
        output_image = get_new_gif(1, width, height, block_width, block_height)
    else:
        square = math.ceil(math.sqrt(len(images)))
        width = square
        height = square
        output_image = get_new_gif(1, square, square, block_width, block_height)

    mapping = []

    for index, image in enumerate(images):
        coord = (index % width, index // width)
        mapping.append(coord)
        copy_animation_across([image], output_image, get_output_coord(*coord, *image.size), True, block_width,
                              block_height)

    return output_image, mapping
