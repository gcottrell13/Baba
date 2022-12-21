from pathlib import Path
from modules.vars import WORD_MAP


def get_text_name(name: str):
    if name in WORD_MAP:
        return get_text_name(WORD_MAP[name])
    return f"text_{name}"


def output_directory_structure(root: Path, dirs: dict):
    for k, v in dirs.items():
        if isinstance(v, dict):
            output_directory_structure(root / k, v)
        elif isinstance(v, str):
            with (root / k).open('w') as f:
                f.write(v)
