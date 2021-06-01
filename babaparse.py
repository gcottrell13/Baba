from PIL import Image
import glob
import re
import pathlib
from collections import defaultdict
import logging
import json

logger = logging.getLogger()
logger.setLevel(logging.INFO)
logger.addHandler(logging.StreamHandler())


info_re = re.compile(r"(?P<name>[\w_]+?)_(?P<phase>\d{1,2})_(?P<wobble>\d)\.png")

FILES_PATH = r"E:\SteamLibrary\steamapps\common\Baba Is You\Data\Sprites"

OUTPUT_DIRECTORY = r'baba_analysis/'

THEME = 'default'

THEMES = {
    'default': 9,
}
THEME_DATA = OUTPUT_DIRECTORY + f'Themes/{THEMES[THEME]}theme.txt'


PALETTE_FILE_NAME = OUTPUT_DIRECTORY + f'Palettes/{THEME}.png'
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



COLORS = {}

with open(OUTPUT_DIRECTORY + f'json/COLORS.json', 'r') as f:
    j = f.read()
    color_data = json.loads(j)

    for data in color_data:
        try:
            color_x, color_y = data.get('colour_active', data['colour'])
            color = PALETTE[color_y][color_x]
            COLORS[data['name']] = color
        except IndexError as e:
            print(data['colour'],data['name'], e)
        except KeyError as e:
            print(objid, e)

        
            

ALL_FILES = glob.glob(FILES_PATH + r'\*.png')

WIDTH = 24
HEIGHT = 24
WPAD = 2
HPAD = 2

DIRECTIONS = {
    0: 'right',
    8: 'up',
    16: 'left',
    24: 'down',
}

SLEEP = {
    7: 'sleep_up',
    15: 'sleep_left',
    23: 'sleep_down',
    31: 'sleep_right',
}

JOINABLE_FLAGS = {
    1: 'right',
    2: 'up',
    4: 'left',
    8: 'down',
}

WORD_MAP = {
    'seastar': 'star',
    'tree2': 'tree',
}

IMAGE_MODE = 'RGBA'


up = r'text_up'
left = r'text_left'
right = r'text_right'
down = r'text_down'

class RecursiveDefaultDict(defaultdict):
    def __init__(self, default):
        self.default = default
        super().__init__(lambda: RecursiveDefaultDict(default))

def load_image(name: str, file: str):
    img = Image.open(file).convert(IMAGE_MODE)
    r, g, b, a = img.split()

    if name in WORD_MAP:
        name = WORD_MAP[name]
    
    p = COLORS.get(name, (255, 255, 255))
    pr, pg, pb = p[0]/255, p[1]/255, p[2]/255
    r = r.point(lambda i: i * pr)
    g = g.point(lambda i: i * pg)
    b = b.point(lambda i: i * pb)
    return Image.merge(IMAGE_MODE, (r, g, b, a))
    
    

def load_sprites(name: str):
    matches = glob.glob(FILES_PATH + '\\' + name + '_*.png')
    return [
        load_image(name, file) for file in matches
    ]

def copy_animation_across(animation: list[Image], destination: list[Image], coord):
    for i, dest in enumerate(destination):
        anim = animation[i % len(animation)]
        dest.paste(anim, coord)

def get_output_coord(block_x: int, block_y: int):
    return WPAD + (WIDTH + WPAD) * block_x, HPAD + (HEIGHT + HPAD) * block_y

def get_new_gif(num_frames: int, width_in_blocks: int, height_in_blocks: int):
    new_width = width_in_blocks * (WIDTH + WPAD) + WPAD
    new_height = height_in_blocks * (HEIGHT + HPAD) + HPAD
    return [
        Image.new(IMAGE_MODE, (new_width, new_height), (0, 0, 0, 0))
        for i in range(num_frames)
    ]

def save_gif(name: str, frames: list[Image]):
    frames[0].save(OUTPUT_DIRECTORY + f'{name}.gif',
                          save_all=True,
                          append_images=frames[1:],
                          duration=300,
                           optimize=True,
                          loop=0)

def get_text_name(name: str):
    if name in WORD_MAP:
        return get_text_name(WORD_MAP[name])
    return f'text_{name}'

def output_facing_and_animation(name: str, images: dict[int, dict[int, Image]]):
    directions: dict[str, list[list[Image]]] = defaultdict(list)
    current = None

    output_data: dict[str, list[list[str]]] = defaultdict(list)

    #print(sorted(images.keys()))
    
    for phase in sorted(images.keys()):
        if phase in SLEEP:
            direc = SLEEP[phase]
        elif phase < 8:
            direc = 'right'
        elif phase < 16:
            direc = 'up'
        elif phase < 24:
            direc = 'left'
        else:
            direc = 'down'
        
        directions[direc].append([
            wobble_sprite
            for wobble_sprite in images[phase].values()
        ])
        output_data[direc].append([
            f'{name}_{phase}_{wobble}'
            for wobble in images[phase].keys()
        ])
            
        #print(name, phase, len(images[phase]))
    
    animation_lengths = {
        len(anim)
        for dir, anim in directions.items()
    } | {3}
    
    layout = [
        ['name', up, down, right, left],
        ['', 'up', 'down', 'right', 'left'],
    ]

    if any(key in directions for key in SLEEP.values()):
        layout.append(['text_sleep', 'sleep_up', 'sleep_down', 'sleep_right', 'sleep_left'])

    animation_length = 1
    for l in animation_lengths:
        animation_length *= l
    
    output_images = get_new_gif(animation_length, max([len(row) for row in layout]), len(layout))

    for row_id, row in enumerate(layout):
        for col_id, item in enumerate(row):
            if not item:
                continue
            
            sprites = []
            if item in [up, left, down, right, 'text_sleep']:
                sprites = load_sprites(item)
            elif item in ['up', 'left', 'down', 'right']:
                sprites = [i for j in directions[item] for i in j]
            elif item in ['sleep_up', 'sleep_left', 'sleep_down', 'sleep_right']:
                sprites = [i for j in directions[item] for i in j]
            elif item == 'name':
                sprites = load_sprites(get_text_name(name))
            #print(item, len(sprites))
            if sprites:
                copy_animation_across(sprites, output_images, get_output_coord(col_id, row_id))
                
    save_gif(name, output_images)

    return output_data


def output_simple(name: str, images: dict[int, dict[int, Image]]):
    output_data = {
        'all': [
            f'{name}_{phase}_{wobble}'
            for phase, p in images.items()
            for wobble in p.keys()
        ],
    }
    if name.startswith('text'):
        layout = [['sprite', 'is', 'text']]
    else:
        layout = [
            ['sprite', 'is', 'name'],
        ]

    animation_length = len(images) * 3
    
    output_images = get_new_gif(animation_length, max([len(row) for row in layout]), len(layout))
    for row_id, row in enumerate(layout):
        for col_id, item in enumerate(row):
            sprites = []
            if item == 'name':
                sprites = load_sprites(get_text_name(name))
            elif item == 'is':
                sprites = load_sprites(f'text_is')
            elif item == 'text':
                sprites = load_sprites(f'text_text')
            elif item == 'sprite':
                sprites = [image for phase in images.values() for image in phase.values()]
                
            if sprites:
                copy_animation_across(sprites, output_images, get_output_coord(col_id, row_id))

    save_gif(name, output_images)
    return output_data

def output_joinable(name: str, images: dict[int, dict[int, Image]]):
    output_images = get_new_gif(3, 12, 12)

    above = list(images[8].values())
    below = list(images[2].values())
    beside_right = list(images[4].values())
    beside_left = list(images[1].values())
    
    for mask in range(16):
        col = (mask % 4) * 3 + 1
        row = (mask // 4) * 3 + 1
        if mask & 2:
            copy_animation_across(above, output_images, get_output_coord(col, row-1))
        if mask & 8:
            copy_animation_across(below, output_images, get_output_coord(col, row+1))
        if mask & 4:
            copy_animation_across(beside_left, output_images, get_output_coord(col-1, row))
        if mask & 1:
            copy_animation_across(beside_right, output_images, get_output_coord(col+1, row))

        copy_animation_across(list(images[mask].values()), output_images, get_output_coord(col, row))
            
    copy_animation_across(load_sprites(get_text_name(name)), output_images, get_output_coord(0, 0))
    
    save_gif(name, output_images)
        


def open_all_images() -> dict[str, dict[int, dict[int, Image]]]:
    objects = RecursiveDefaultDict(int)
    for file in ALL_FILES:
        try:
            name, phase, wobble = info_re.match(pathlib.Path(file).name).groups()
            phase, wobble = int(phase), int(wobble)
            img = load_image(name, file)
            objects[name][phase][wobble] = img
        except:
            print(file)
    return objects



OUTPUT = {}
for name, values in open_all_images().items():
    output = None
    
    has_facing = all(direction in values for direction in DIRECTIONS)
    if has_facing and len(values) == 4:
        # this object has separate sprites for each direction
        logger.info(f'{name} has facing sprites')
        output = output_facing_and_animation(name, values)
    elif has_facing and len(values) > 4:
        # this object has separate sprites for each direction, AND has animations
        logger.info(f'{name} has facing sprites AND animations')
        output = output_facing_and_animation(name, values)
    elif len(values) == 16:
        # this object can 'join' with others nearby, like WALL or WATER
        logger.info(f'{name} can join with others')
        output_joinable(name, values)
        output = {
            'type': 'joinable',
        }
        
    elif len(values) > 1:
        # this object has no direction, but does have animation sprites
        logger.info(f'{name} has an animation, but no facing sprites')
        output = output_simple(name, values)
    else:
        # this object is simple
        logger.info(f'{name} is simple')
        output = output_simple(name, values)

    if output:
        OUTPUT[name] = output
        
with open(OUTPUT_DIRECTORY + f'json/OUTPUT.json', 'w') as f:
    f.write(json.dumps(OUTPUT, indent=4))














