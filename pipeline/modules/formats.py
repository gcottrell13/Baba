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

    def frames_count(self):
        return math.lcm(*map(lambda x: x.frames_count(), self))

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

    def frames_count(self):
        return len(self.wobbles)


class AnimateOnMove(ObjectSprites):
    def __init__(self, name: str, *frames: Wobbler):
        super().__init__(name)
        self.frames = frames

    def __iter__(self):
        for frame in self.frames:
            yield from iter(frame)

    def frames_count(self):
        return sum(map(lambda x: x.frames_count(), self.frames))


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
        if self.sleep_up:
            yield from self.sleep_up
        yield from self.left
        if self.sleep_left:
            yield from self.sleep_left
        yield from self.down
        if self.sleep_down:
            yield from self.sleep_down
        yield from self.right
        if self.sleep_right:
            yield from self.sleep_right

    def frames_count(self):
        return sum(i.frames_count() for i in [
            self.up,
            self.left,
            self.down,
            self.right,
            self.sleep_up,
            self.sleep_left,
            self.sleep_down,
            self.sleep_right,
        ] if i)


class Joinable(ObjectSprites):
    up = 0b10
    down = 0b1000
    left = 0b100
    right = 0b1

    def __init__(self, name: str,
                 *args: Wobbler
                 ):
        super().__init__(name)
        self.frames = args

    def __iter__(self):
        for item in self.frames:
            yield from item

    def frames_count(self):
        return sum(i.frames_count() for i in self.frames if i)
