import math
from PIL import Image

from modules.output_gifs.output_images import copy_animation_across, get_new_gif, get_output_coord
from ..types import Vector2


def create_sheet(images: list[Image.Image], width: int = 0) -> tuple[list[Image.Image], list[Vector2]]:
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

