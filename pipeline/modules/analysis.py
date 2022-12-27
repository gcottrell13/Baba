from typing import Any

from modules.formats import ImageCollection, ObjectSprites, Wobbler, FacingOnMove, Joinable, AnimateOnMove
from modules.load_object_information import InfoItem
from modules.vars import SLEEP, DIRECTIONS


def analyze_facing_and_animation(name: str, images: dict[int, dict[int, Any]]) -> FacingOnMove:
    output_data: dict[str, list[Wobbler]] = {}

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
        output_data.setdefault(direc, []).append(analyze_simple(f'{name}.{direc}.{len(output_data[direc])}', images[phase]))

    return FacingOnMove(name, **{
        k: AnimateOnMove(f'{name}.{k}', *f) for k, f in output_data.items()
    })


def analyze_simple(name: str, phase: dict[int, Any]) -> Wobbler:
    return Wobbler(name, [phase[i] for i in sorted(phase.keys())])


def analyze_simple_animated_on_move(name: str, images: dict[int, dict[int, Any]]):
    return AnimateOnMove(
        name,
        *[analyze_simple(f'{name}.{i}', x) for i, x in images.items()],
    )


def analyze_joinable(name: str, images: dict[int, dict[int, Any]]) -> Joinable:
    def d():
        for phase in sorted(images.keys())[:16]:
            pname = name + '.'
            if phase == 0:
                pname += '0'
            if phase & Joinable.up:
                pname += 'u'
            if phase & Joinable.down:
                pname += 'd'
            if phase & Joinable.left:
                pname += 'l'
            if phase & Joinable.right:
                pname += 'r'
            yield analyze_simple(pname, images[phase])

    return Joinable(
        name,
        *d(),
    )


def analyze_images(images: ImageCollection, all_info: dict[str, InfoItem]) -> dict[str, ObjectSprites]:
    OUTPUT: dict[str, ObjectSprites] = {}
    for name, info in all_info.items():

        values = images.get(info.get('sprite', name))
        if not values:
            continue

        output: ObjectSprites

        has_facing = all(
            direction in values for direction in DIRECTIONS
        ) and name not in ['cliff']

        if has_facing and len(values) == 4:
            # this object has separate sprites for each direction
            print(f"{name} has facing sprites")
            output = analyze_facing_and_animation(name, values)

        elif has_facing and len(values) > 4:
            # this object has separate sprites for each direction, AND has animations
            print(f"{name} has facing sprites AND animations")
            output = analyze_facing_and_animation(name, values)

        elif len(values) == 16 or name == 'cliff':
            # this object can 'join' with others nearby, like WALL or WATER
            print(f"{name} can join with others")
            output = analyze_joinable(name, values)
        elif len(values) > 1:
            # this object has no direction, but does have animation sprites
            print(f"{name} has an animation, but no facing sprites")
            output = analyze_simple_animated_on_move(name, values)
        else:
            # this object is simple
            print(f"{name} is simple")
            output = analyze_simple(name, values[0])

        OUTPUT[name] = output

    return OUTPUT
