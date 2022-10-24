from PIL import Image

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
ImageCollection = dict[str, dict[int, dict[int, Image.Image]]]
