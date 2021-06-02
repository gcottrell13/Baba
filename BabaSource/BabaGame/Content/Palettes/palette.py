from PIL import Image
import glob
import re
import pathlib
from collections import defaultdict
import logging
import json
import math

palettes = glob.glob("*.png")

print(palettes)

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

    file = file.replace('.png', '.json')

    with open(f'jsonfiles/{file}', 'w') as f:
        f.write(json.dumps(PALETTE))






