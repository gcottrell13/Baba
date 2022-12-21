from PIL import Image
import math

from modules.formats import Wobbler, AnimateOnMove, FacingOnMove, Joinable, ObjectSprites
from modules.imageUtils import get_new_gif, get_output_coord, copy_animation_across
from modules.load_object_information import load_information
from modules.vars import VISUAL_OUTPUT_DIRECTORY, WORD_MAP, IMAGE_MODE


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


def make_proof(layout: list[list[ObjectSprites | None]], actives: list[list[bool | int | None]]):
    length = math.lcm(*[item.frames_count() for row in layout for item in row if item])
    lx, ly = map(lambda x: max(*x), zip(*[item.largest_dimensions() for row in layout for item in row if item]))
    proof = get_new_gif(length, len(layout[0]), len(layout), block_width=lx, block_height=ly)
    for y, row in enumerate(layout):
        for x, sprite in enumerate(row):
            if sprite is None:
                continue
            copy_animation_across(
                colorized_images(sprite.name, list(sprite), actives[y][x]), proof, get_output_coord(x, y, lx, ly),
                block_width=lx, block_height=ly
            )
    return proof


def make_proof_wobbler(name: str, obj: Wobbler, data: dict[str, ObjectSprites]):
    if name.startswith('text_'):
        return make_proof([[obj, obj]], [[1, 0]])
    return make_proof([
        [data[f'text_{name}'], obj],
        [data['text_is'], data[f'text_{name}']]
    ], [
        [1, 1],
        [0, 0],
    ])


def make_proof_animate_on_move(name: str, obj: AnimateOnMove, data: dict[str, ObjectSprites]):
    if name.startswith('text_'):
        return make_proof([[obj, obj]], [[1, 0]])
    return make_proof([
        [obj, data[f'text_{name}']],
        [data['text_is'], data[f'text_{name}']]
    ], [
        [1, 1],
        [0, 0],
    ])


def make_proof_facing_move(name: str, obj: FacingOnMove, data: dict[str, ObjectSprites]):
    layout = []
    actives = []
    layout.append([data[f'text_{name}'], data['text_up'], data['text_down'], data['text_left'], data['text_right']])
    actives.append([1, 1, 1, 1, 1])
    layout.append([data[f'text_{name}'], obj.up, obj.down, obj.left, obj.right])
    actives.append([0, 1, 1, 1, 1])

    sleeps = [obj.sleep_up, obj.sleep_down, obj.sleep_left, obj.sleep_right]
    if any(sleeps):
        layout.append([data['text_sleep'], *sleeps])
        actives.append([1, 1, 1, 1, 1])

    return make_proof(layout, actives)


def make_proof_joinable(name: str, obj: Joinable, data: dict[str, ObjectSprites]):
    return []


def make_all_proofs(data: dict[str, ObjectSprites]):
    for name, obj in data.items():
        match obj:
            case Wobbler() as w:
                proof = make_proof_wobbler(name, w, data)
            case AnimateOnMove() as w:
                proof = make_proof_animate_on_move(name, w, data)
            case Joinable() as w:
                proof = make_proof_joinable(name, w, data)
            case FacingOnMove() as w:
                proof = make_proof_facing_move(name, w, data)
            case _:
                print(f'skip {name}, {obj}')
                continue
        if proof:
            save_gif(name, proof)

        solo = get_new_gif(obj.frames_count(), 1, 1, *obj.largest_dimensions())
        if solo:
            copy_animation_across(colorized_images(name, list(obj)), solo, (1, 1))
            save_gif(f'{name}-solo', solo)


def colorized_images(name: str, images: list[Image.Image], active: bool = True):
    return [colorized_image(name, img, active) for img in images]


def colorized_image(name: str, img: Image.Image, active: bool = True):
    r, g, b, a = img.split()

    if name in WORD_MAP:
        name = WORD_MAP[name]

    if name.startswith("text_") and (notxt := name.removeprefix("text_")) in WORD_MAP:
        name = f"text_{WORD_MAP[notxt]}"

    _, COLORS = load_information()
    info = COLORS.get(name, {})
    default = (255, 255, 255)

    if not active:
        p = info.get("color", default)
    else:
        p = info.get("color_active", default)

    pr, pg, pb = p[0] / 255, p[1] / 255, p[2] / 255
    r = r.point(lambda i: int(i * pr))
    g = g.point(lambda i: int(i * pg))
    b = b.point(lambda i: int(i * pb))
    return Image.merge(IMAGE_MODE, (r, g, b, a))
