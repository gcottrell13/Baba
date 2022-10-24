from PIL import Image
from modules.analysis import ANALYSIS_OUTPUT

from modules.output_gifs.output_images import save_spritesheet

from .sheet import create_sheet


def pack_images_into_spritesheet(
    name: str,
    data: ANALYSIS_OUTPUT,
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
