import base64
from io import BytesIO
from typing import Iterable

from modules.formats import ObjectSprites, Joinable, FacingOnMove, AnimateOnMove, Wobbler
from modules.imagesToSpritesheet import create_sheet
from modules.utils import output_directory_structure

from modules.vars import OUTPUT_DIRECTORY, VISUAL_OUTPUT_DIRECTORY
from modules.load_object_information import load_information, all_palettes

NAMESPACE = "Core.Content"

csharp_keywords = [
    "lock", "object", "async", "float", "is"
]


def save_object_info():
    info, colors = load_information()

    def mapitem(name, item):
        colorx, colory = item['color']
        coloractx, coloracty = item['color_active']

        fields = [
            f'color = ({colorx} << shift) + {colory}',
            f'color_active = ({coloractx} << shift) + {coloracty}',
            f'sprite = "{name}"',
            f'layer = {item["layer"]}',
            f'unittype = "{item["unittype"]}"',
        ]

        return f"""\t\t\t{{"{name}", new ObjectInfoItem() {{ {', '.join(fields)} }} }}"""

    items = ',\n'.join([
        mapitem(name, item)
        for name, item in info.items()
    ])

    def transform_name(name: str):
        name = name.removeprefix('text_')
        if ord('0') <= ord(name[0]) <= ord('9'):
            name = "_" + name
        if name in csharp_keywords:
            name = "@" + name
        return name

    keyIndex = [
        k
        for k in info.keys()
        if not k.startswith('text_') or k[5:] not in info
    ]

    enum_values = ",\n".join(
        f"\t{transform_name(key)} = {index}"
        for index, key in enumerate(keyIndex)
    )

    id_to_name = ",\n".join(
        f"\t\t{{ ObjectTypeId.{transform_name(key)}, \"{key}\" }}"
        for key in keyIndex
    )
    name_to_id = ",\n".join(
        f"\t\t{{ \"{key}\", ObjectTypeId.{transform_name(key)} }}"
        for key in keyIndex
    )

    output_directory_structure(OUTPUT_DIRECTORY, {
        'Content/ObjectInfo.cs': f"""
using System;
using System.Collections.Generic;
using System.Linq;

namespace {NAMESPACE};

public class ObjectInfoItem {{
    public int color;
    public int color_active;
    public string sprite = string.Empty;
    public int layer;
    public string unittype = string.Empty;
}}

public static class ObjectInfo {{
    private const int shift = {(7).bit_length()};
    public static readonly Dictionary<string, ObjectInfoItem> Info = new Dictionary<string, ObjectInfoItem>() {{
{items}
    }};
    
    public static readonly Dictionary<ObjectTypeId, string> IdToName = new() {{
{id_to_name}
    }};
    
    public static readonly Dictionary<string, ObjectTypeId> NameToId = new() {{
{name_to_id}
    }};
    
}}

public enum ObjectTypeId {{
{enum_values}
}}
        """,
    })


def save_palette_info():
    palettes = all_palettes()

    def coord_to_intrep(coord: tuple[int, int]):
        return f'({coord[0]} << shift) + {coord[1]}'

    def map_palette(name: str, data: dict[tuple[int, int], tuple[int, int, int]]):
        colors = ','.join([
            f'\n\t\t\t{{ {coord_to_intrep(coord)}, new Color({", ".join(map(str, color))}) }}'
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

    color_name_map_items = {
        'red': coord_to_intrep((2, 1)),
        'blue': coord_to_intrep((3, 3)),
        'yellow': coord_to_intrep((2, 4)),
        'orange': coord_to_intrep((2, 2)),
        'green': coord_to_intrep((5, 2)),
        'cyan': coord_to_intrep((1, 4)),
        'lime': coord_to_intrep((5, 3)),
        'purple': coord_to_intrep((3, 0)),
        'pink': coord_to_intrep((4, 1)),
        'rosy': coord_to_intrep((4, 2)),
        'grey': coord_to_intrep((0, 1)),
        'black': coord_to_intrep((0, 0)),
        'silver': coord_to_intrep((0, 2)),
        'white': coord_to_intrep((0, 3)),
        'brown': coord_to_intrep((6, 1)),
    }
    color_name_map = ',\n\t\t\t'.join(
        f'{{ "{name}", {v} }}'
        for name, v in color_name_map_items.items()
    )

    output_directory_structure(OUTPUT_DIRECTORY, {
        'Content/PaletteInfo.cs': f"""
using Microsoft.Xna.Framework; 
using System.Collections.Generic;
using System.Linq;

namespace {NAMESPACE} {{
    public static class PaletteInfo {{
        public const int shift = {(7).bit_length()};
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
        return f'new Point({x}, {y})'

    def wobbler(wobbler: Wobbler, positions: list[tuple[int, int]]):
        w, h = wobbler.largest_dimensions()
        return f'new Wobbler("{wobbler}", new[] {{ {joinmap(vec2, positions)} }}, new Point({w}, {h}), sheets["{wobbler.name}"])'

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

    longest_name_size = max(map(len, data.keys()))

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
            n = f"\"{name}\"".ljust(16 + 2)
            sheets.append(f'\t\t\t{{ {n}, Texture2D.FromStream(graphics, '
                          f'new MemoryStream(System.Convert.FromBase64String("{b64}"))) }}')

    sheet_text = ',\n'.join(sheets)
    lines_text = ',\n'.join(lines)
    output_directory_structure(OUTPUT_DIRECTORY, {
        'Content/Sheets.cs': f"""
using Microsoft.Xna.Framework.Graphics; 
using System.Collections.Generic;
using System.IO;

namespace {NAMESPACE} {{
    public static class Sheets {{
        public static Dictionary<string, Texture2D> GetSheets(GraphicsDevice graphics) => new Dictionary<string, Texture2D>() {{
{sheet_text}
        }};
    }}
}}
    """,
        'Content/SheetMap.cs': f"""
using Core.Utils; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace {NAMESPACE} {{
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
