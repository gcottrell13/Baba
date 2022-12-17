from typing import Iterable

from PIL.Image import Image
import math

__all__ = [
    'AnalysisResult',
    'ImageCollection',
    'Vector2',
]

Vector2 = tuple[int, int]

AnalysisResultValue = list[list[Vector2]]

# c[objectName][stateName]
AnalysisResult = dict[str, dict[str, AnalysisResultValue]]

# c[objectName][stateId][wobbleNumber]
ImageCollection = dict[str, dict[int, dict[int, Image]]]


class ObjectSprites:
    def __init__(self, name: str):
        name, *rest = name.split('.')
        self.name = name
        self.rest = rest

    def __iter__(self) -> Iterable[Image]:
        raise NotImplementedError()

    def __len__(self):
        return math.lcm(*map(len, self))

    def __repr__(self):
        return f'{self.__class__.__name__}<{".".join([self.name, *self.rest])}>'

    def largest_dimensions(self) -> tuple[int, int]:
        xs = []
        ys = []
        for w in self:
            x, y = w.size
            xs.append(x)
            ys.append(y)
        return max(xs), max(ys)


class Wobbler(ObjectSprites):
    def __init__(self, name: str, wobbles: list[Image]):
        super().__init__(name)
        self.wobbles = wobbles

    def __iter__(self):
        return iter(self.wobbles)

    def __len__(self):
        return len(self.wobbles)


class AnimateOnMove(ObjectSprites):
    def __init__(self, name: str, *frames: Wobbler):
        super().__init__(name)
        self.frames = frames

    def __iter__(self):
        for frame in self.frames:
            yield from iter(frame)

    def __len__(self):
        return sum(map(len, self.frames))



class FacingOnMove(ObjectSprites):
    def __init__(self, name: str,
                 *,
                 up: AnimateOnMove,
                 left: AnimateOnMove,
                 right: AnimateOnMove,
                 down: AnimateOnMove,
                 sleep_up: AnimateOnMove = None,
                 sleep_left: AnimateOnMove = None,
                 sleep_right: AnimateOnMove = None,
                 sleep_down: AnimateOnMove = None,
                 ):
        super().__init__(name)
        self.up = up
        self.left = left
        self.right = right
        self.down = down
        self.sleep_up = sleep_up
        self.sleep_left = sleep_left
        self.sleep_right = sleep_right
        self.sleep_down = sleep_down

    def __iter__(self):
        yield from self.up
        yield from self.left
        yield from self.right
        yield from self.down
        yield from self.sleep_up
        yield from self.sleep_left
        yield from self.sleep_right
        yield from self.sleep_down


class Joinable(ObjectSprites):
    def __init__(self, name: str,
                 u: Wobbler,
                 d: Wobbler,
                 l: Wobbler,
                 r: Wobbler,
                 ul: Wobbler,
                 ud: Wobbler,
                 ur: Wobbler,
                 dl: Wobbler,
                 dr: Wobbler,
                 lr: Wobbler,
                 udl: Wobbler,
                 udr: Wobbler,
                 url: Wobbler,
                 drl: Wobbler,
                 udlr: Wobbler,
                 ):
        super().__init__(name)
        self.u = u
        self.d = d
        self.l = l
        self.r = r
        self.ul = ul
        self.ud = ud
        self.ur = ur
        self.dl = dl
        self.dr = dr
        self.lr = lr
        self.udl = udl
        self.udr = udr
        self.url = url
        self.drl = drl
        self.udlr = udlr

    def __iter__(self):
        yield from self.u
        yield from self.d
        yield from self.l
        yield from self.r
        yield from self.ul
        yield from self.ud
        yield from self.ur
        yield from self.dl
        yield from self.dr
        yield from self.lr
        yield from self.udl
        yield from self.udr
        yield from self.url
        yield from self.drl
        yield from self.udlr
