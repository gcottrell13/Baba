
from modules.vars import WORD_MAP


def get_text_name(name: str):
    if name in WORD_MAP:
        return get_text_name(WORD_MAP[name])
    return f"text_{name}"

