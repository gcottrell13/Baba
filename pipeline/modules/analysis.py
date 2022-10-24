

from typing import Any

from modules.vars import SLEEP


# data[direction_name][phase number] = [wobble number]

ANALYSIS_OUTPUT = dict[str, dict[int, list[int]]]


def analyze_facing_and_animation(images: dict[int, dict[int, Any]]) -> ANALYSIS_OUTPUT:
    
    output_data: ANALYSIS_OUTPUT = {}

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
        output_data.setdefault(direc, {})[phase] = list(images[phase].keys())
    
    return output_data


def analyze_simple(images: dict[int, dict[int, Any]]) -> ANALYSIS_OUTPUT:
    output_data = {
        "all": {
            phase: list(p.keys())
            for phase, p in images.items()
        },
    }
    return output_data


def analyze_joinable() -> ANALYSIS_OUTPUT:
    return {}