import functools
import json
from glob import glob
from pathlib import Path
from typing import Iterator, TypedDict
from PIL import Image
from modules.vars import CUSTOM_VALUES_FILE_PATH, FILES_PATH, VALUES_FILE_PATH

ColorsFormat = dict[str, dict[str, tuple[int, int, int]]]


class InfoItem(TypedDict):
    color: tuple[int, int]
    color_active: tuple[int, int]
    sprite: str
    layer: int
    unittype: str


ObjectInformationCollection = dict[str, InfoItem]


def parse_block(lines: Iterator[str]) -> dict:
    """
    for parsing a lua table that has a very specific format (this is incredibly brittle)
    """

    props = {}

    while True:
        line = next(lines).strip()

        if line.startswith('}'):
            break

        if 'customobjects' in line:
            for i in range(7):
                next(lines)
            continue

        if line.endswith('= {'):
            parts = line.split()
            d = parse_block(lines)
            props[str(parts[0].strip())] = d
        else:
            parts = line.strip().split('=')
            if len(parts) > 1:
                try:
                    props[parts[0].strip()] = eval(parts[1].replace('{', '[').replace('}', ']').rstrip(','))
                except:
                    pass
    return props


PALETTE_FILE_NAME = FILES_PATH / f'Palettes/default.png'


@functools.lru_cache
def all_palettes() -> dict[str, dict[tuple[int, int], tuple[int, int, int]]]:
    palettes = glob(str(FILES_PATH / f'Palettes/*.png'))
    palette_infos = {}
    for file_name in palettes:
        name = Path(file_name).name.removesuffix('.png')
        palette_file = Image.open(file_name)
        PALETTE_WIDTH = 7
        PALETTE_HEIGHT = 5
        PALETTE_FLAT_DATA = list(palette_file.getdata())
        PALETTE = {
            (j, i): PALETTE_FLAT_DATA[i * PALETTE_WIDTH + j]
            for j in range(PALETTE_WIDTH)
            for i in range(PALETTE_HEIGHT)
        }
        palette_infos[name] = PALETTE
    return palette_infos


@functools.lru_cache
def load_information() -> tuple[ObjectInformationCollection, ColorsFormat]:
    OBJECT_INFO: ObjectInformationCollection = {}

    with open(VALUES_FILE_PATH, 'r') as f:
        lines = iter(f.readlines())

        while not next(lines).startswith('editor_objlist = '):
            pass

        d = parse_block(lines)

        for obj, attrs in d.items():
            if 'name' not in attrs:
                continue
            OBJECT_INFO[attrs['name']] = {
                'color': attrs['colour'],
                'color_active': attrs.get('colour_active', attrs['colour']),
                'sprite': attrs.get('sprite', attrs['name']),
                'layer': attrs['layer'],
                'unittype': attrs['unittype'],
            }

    with open(CUSTOM_VALUES_FILE_PATH, 'r') as f:
        custom_colors = json.loads(f.read())
        OBJECT_INFO |= custom_colors

    OBJECT_INFO = {
        key: OBJECT_INFO[key]
        for key in sorted(OBJECT_INFO.keys())
    }

    palette = all_palettes()['default']

    return OBJECT_INFO, {
        name: {
            'color_active': palette[tuple(color_indexes['color_active'])],
            'color': palette[tuple(color_indexes['color'])],
        }
        for name, color_indexes in OBJECT_INFO.items()
    }


if __name__ == '__main__':
    print(json.dumps(load_information(), indent=4))
