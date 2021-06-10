import json
from typing import Iterator
from PIL import Image
from vars import VALUES_FILE_PATH, CUSTOM_VALUES_FILE_PATH, FILES_PATH


def parse_block(lines: Iterator[str]) -> dict:
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

def load_colors() -> dict[str, dict[str, tuple[int, int, int]]]:
    colors_by_name: dict[str, dict[str, tuple[int, int]]] = {}

    with open(VALUES_FILE_PATH, 'r') as f:
        lines = iter(f.readlines())

        while not next(lines).startswith('editor_objlist = '):
            pass
        
        d = parse_block(lines)

        for obj, attrs in d.items():
            if 'name' not in attrs:
                continue
            colors_by_name[attrs['name']] = {
                'color_inactive': attrs['colour'],
                'color': attrs.get('colour_active', attrs['colour']),
            }

    return {
        name: {
            'color': PALETTE[tuple(color_indexes['color'])],
            'color_inactive': PALETTE[tuple(color_indexes['color_inactive'])],
        }
        for name, color_indexes in colors_by_name.items()
    }

if __name__ == '__main__':
    print(json.dumps(load_colors(), indent=4))