from typing import Iterator
from PIL import Image
from .vars import VALUES_FILE_PATH, CUSTOM_VALUES_FILE_PATH, FILES_PATH


def parse_block(lines: Iterator[str]) -> dict:
    if next(lines).strip() != '{':
        raise Exception()
    
    props = {}

    parts = next(lines).split(' = ')
    if len(parts) == 1:
        d = parse_block(lines)
        props[parts[0].strip()] = d
    elif len(parts) == 2:
        try:
            props[parts[0].strip()] = eval(parts[1].replace('{', '').replace('}', ''))
        except:
            pass
    return props



PALETTE_FILE_NAME = FILES_PATH / f'Palettes/default.png'

palette_file = Image.open(PALETTE_FILE_NAME)
PALETTE_WIDTH = 7
PALETTE_HEIGHT = 5
PALETTE_FLAT_DATA = list(palette_file.getdata())
PALETTE = [
    [
        PALETTE_FLAT_DATA[i * PALETTE_WIDTH + j]
        for j in range(PALETTE_WIDTH)
    ]
    for i in range(PALETTE_HEIGHT)
]

def load_colors() -> dict[str, dict]:
    colors_by_name: dict[str, dict] = {}

    with open(VALUES_FILE_PATH, 'r') as f:
        lines = iter(f.readlines())

        while not next(lines).startswith('tileslist'):
            pass
        
        d = parse_block(lines)

        colors_by_name[d['name']] = {
            'color': d['colour'],
            'color': d.get('active', None),
        }

    return colors_by_name

