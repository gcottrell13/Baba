#! py -3.11

from modules.analysis import analyze_images
from modules.make_proofs import make_all_proofs
from modules.utils import output_directory_structure

from modules.vars import OUTPUT_DIRECTORY
from modules.load_object_information import load_information, all_palettes
from modules.imageUtils import *


def save_object_info():
    info, colors = load_information()

    def mapitem(name, item):
        colorx, colory = item['color']
        coloractx, coloracty = item['color_active']

        fields = [
            f'color = ({colorx} << shift) + {colory}',
            f'color_active = ({coloractx} << shift) + {coloracty}',
            f'sprite = "{item["sprite"]}"',
            f'layer = {item["layer"]}',
            f'unittype = "{item["unittype"]}"',
        ]

        return f"""\t\t\t{{"{name}", new ObjectInfoItem() {{ {', '.join(fields)} }} }}"""

    items = ',\n'.join([
        mapitem(name, item)
        for name, item in info.items()
    ])

    output_directory_structure(OUTPUT_DIRECTORY, {
        'ObjectInfo.cs': f"""
using System;
using System.Collections.Generic;
using System.Linq;

namespace BabaGame.Content {{
    public struct ObjectInfoItem {{
        public int color;
        public int color_active;
        public string sprite;
        public int layer;
        public string unittype;
    }}
    
    public static class ObjectInfo {{
        private const int shift = {(7).bit_length()};
        public static readonly Dictionary<string, ObjectInfoItem> Info = new Dictionary<string, ObjectInfoItem>() {{
{items}
        }};
    }}
}}
        """,
    })


def save_palette_info():
    palettes = all_palettes()

    def map_palette(name: str, data: dict[tuple[int, int], tuple[int, int, int]]):
        colors = ','.join([
            f'\n\t\t\t{{ ({coord[0]} << shift) + {coord[1]}, new Color({", ".join(map(str, color))}) }}'
            for coord, color in data.items()
        ])
        return f'\t\tprivate static readonly Dictionary<int, Color> palette_{name} = new Dictionary<int, Color>() {{ {colors}\n\t\t}};'

    items = '\n'.join([
        map_palette(name, item)
        for name, item in palettes.items()
    ])

    palette_dir = '\n'.join([
        f'\t\t\t{{ "{name}", palette_{name} }},'
        for name in palettes
    ])

    output_directory_structure(OUTPUT_DIRECTORY, {
        'PaletteInfo.cs': f"""
using Microsoft.Xna.Framework; 
using System.Collections.Generic;

namespace BabaGame.Content {{
    public static class PaletteInfo {{
        private const int shift = {(7).bit_length()};
{items}
        public static Dictionary<string, Dictionary<int, Color>> Palettes = new Dictionary<string, Dictionary<int, Color>>() {{
{palette_dir}        
        }};
    }}
}}
""",
    })


if __name__ == "__main__":
    images = open_all_images()
    analysis_result = analyze_images(images, load_information()[0])
    save_object_info()
    save_palette_info()
    # make_all_proofs(analysis_result)
