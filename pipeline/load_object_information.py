import json
from typing import Iterator, TypedDict
from PIL import Image
from vars import VALUES_FILE_PATH, CUSTOM_VALUES_FILE_PATH, FILES_PATH

ColorsFormat = dict[str, dict[str, tuple[int, int, int]]]


class InfoItem(TypedDict):
    color_inactive: tuple[int, int]
    color: tuple[int, int]
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

palette_file = Image.open(PALETTE_FILE_NAME)
PALETTE_WIDTH = 7
PALETTE_HEIGHT = 5
PALETTE_FLAT_DATA = list(palette_file.getdata())
PALETTE = {
    (j, i): PALETTE_FLAT_DATA[i * PALETTE_WIDTH + j]
    for j in range(PALETTE_WIDTH)
    for i in range(PALETTE_HEIGHT)
}

_cache: tuple[ObjectInformationCollection, ColorsFormat] = ()

def load_information() -> tuple[ObjectInformationCollection, ColorsFormat]:

    global _cache
    if _cache:
        return _cache

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
                'color_inactive': attrs['colour'],
                'color': attrs.get('colour_active', attrs['colour']),
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

    _cache = OBJECT_INFO, {
        name: {
            'color': PALETTE[tuple(color_indexes['color'])],
            'color_inactive': PALETTE[tuple(color_indexes['color_inactive'])],
        }
        for name, color_indexes in OBJECT_INFO.items()
    }
    return _cache

if __name__ == '__main__':
    print(json.dumps(load_information(), indent=4))