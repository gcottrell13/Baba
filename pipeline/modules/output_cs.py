import base64
from io import BytesIO
from typing import Iterable

from modules.formats import ObjectSprites, Joinable, FacingOnMove, AnimateOnMove, Wobbler
from modules.imagesToSpritesheet import create_sheet
from modules.utils import output_directory_structure

from modules.vars import OUTPUT_DIRECTORY, VISUAL_OUTPUT_DIRECTORY
from modules.load_object_information import load_information, all_palettes


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

namespace Content {{
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

namespace Content {{
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


def output_spritesheets(data: dict[str, ObjectSprites]):
    def vec2(x, y):
        return f'new Vector2({x}, {y})'

    def wobbler(w: Wobbler, positions: list[tuple[int, int]]):
        return f'new Wobbler("{w}", new[] {{ {joinmap(vec2, positions)} }}, sheets["{w.name}"])'

    def joinmap(fn, alist: Iterable, indent=0):
        i = '\t' * indent
        return (', ' + ('\n' if indent else '')).join(map(lambda x: i + fn(*x), alist))

    def map_wobbles(j: Joinable | AnimateOnMove, positions: list[tuple[int, int]]):
        p = iter(positions)
        d = [
            [
                next(p)
                for j in f.wobbles
            ]
            for f in j.frames
        ]
        return f'new Wobbler[] {{\n{joinmap(wobbler, zip(j.frames, d), 1)} }}'

    def map_facing(f: FacingOnMove, positions: list[tuple[int, int]]):
        d = [
            'up',
            'sleep_up',
            'left',
            'sleep_left',
            'down',
            'sleep_down',
            'right',
            'sleep_right',
        ]
        p = iter(positions)
        out = {}
        for name in d:
            anim = getattr(f, name, None)
            if not anim:
                out[name] = 'null'
            else:
                anim_positions = [next(p) for _ in anim]
                out[name] = animate_on_move(anim, anim_positions)
        return out

    def facing_line(dirstr: str, t: str):
        return f'{dirstr}: {t}'

    def joinable(j: Joinable, positions: list[tuple[int, int]]):
        if not j:
            return 'null'
        wobbles = map_wobbles(j, positions)
        return f'new Joinable("{name}", {wobbles})'

    def animate_on_move(j: AnimateOnMove, positions: list[tuple[int, int]]):
        if not j:
            return 'null'
        wobbles = map_wobbles(j, positions)
        return f'new AnimateOnMove("{name}", {wobbles})'

    lines: list[str] = []
    sheets: list[str] = []

    for name, info in data.items():
        sheet, mapping = create_sheet(list(info))

        match info:
            case Joinable() as j:
                text = joinable(j, mapping)
            case Wobbler() as w:
                text = wobbler(w, mapping)
            case AnimateOnMove() as a:
                text = animate_on_move(a, mapping)
            case FacingOnMove() as f:
                animations = map_facing(f, mapping)
                text = f"""
new FacingOnMove(
                name: "{name}", 
{joinmap(facing_line, animations.items(), indent=4)}
)""".strip()
            case _:
                text = 'null'

        lines.append(f'\t\t\t{{ "{name}", {text} }}')
        sheet[0].save(VISUAL_OUTPUT_DIRECTORY / f'{name}.sheet.png')
        with open(str(VISUAL_OUTPUT_DIRECTORY / f'{name}.sheet.png'), 'rb') as f:
            b64 = base64.b64encode(f.read()).decode('utf-8')
            sheets.append(f'\t\t\t{{ "{name}", Texture2D.FromStream(graphics, '
                          f'new MemoryStream(System.Convert.FromBase64String("{b64}"))) }}')

    sheet_text = ',\n'.join(sheets)
    lines_text = ',\n'.join(lines)
    output_directory_structure(OUTPUT_DIRECTORY, {
            'Sheets.cs': f"""
using Microsoft.Xna.Framework.Graphics; 
using System.Collections.Generic;
using System.IO;

namespace Content {{
    public static class Sheets {{
        public static Dictionary<string, Texture2D> GetSheets(GraphicsDevice graphics) => new Dictionary<string, Texture2D>() {{
{sheet_text}
        }};
    }}
}}
    """,
            'SheetMap.cs': f"""
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework;

namespace Content {{
    public static class SheetMap {{
        public static Dictionary<string, SpriteValues> GetSpriteInfo(Dictionary<string, Texture2D> sheets) {{
            return new Dictionary<string, SpriteValues>() {{
{lines_text}
            }};
        }}
    }}
}}
""",
        })
