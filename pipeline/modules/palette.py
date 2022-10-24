from PIL import Image
import glob
import json
from .vars import FILES_PATH

palettes = glob.glob(str(FILES_PATH / 'Palettes' / "*.png"))

# print(palettes)

output = {}

for file in palettes:

    palette_file = Image.open(file)
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

    name = file.replace('.png', '')
    output[name] = PALETTE

with open(f'PALETTES.json', 'w') as f:
    f.write(json.dumps(output))






