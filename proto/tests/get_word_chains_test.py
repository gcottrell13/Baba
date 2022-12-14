import pytest as pytest
import json

from proto.src.parse_sentences import get_word_chains, Grid, get_sentences


class PrettyPrint:
    def __init__(self, a):
        self.a = a

    def __repr__(self):
        return json.dumps(self.a, indent=4)

    def __eq__(self, other):
        if isinstance(other, PrettyPrint):
            return json.dumps(other.a) == json.dumps(self.a)
        return json.dumps(self.a) == json.dumps(other)


@pytest.mark.parametrize(
    'grid, expected',
    [
        (
                [
                    [None, None, 'a'],
                    [None, None, 'a'],
                    [None, None, 'a'],
                ],
                [
                    ['a', 'a', 'a'],
                ]
        ),
        (
                [
                    ['b', None, 'b'],
                    ['a', None, 'a'],
                    ['b', None, 'a'],
                ],
                [
                    ['b', 'a', 'b'],
                    ['b', 'a', 'a'],
                ]
        ),
        (
                [
                    [None, 'b', None],
                    ['a', 'b', 'a'],
                    [None, 'a', None],
                ],
                [
                    ['b', 'b', 'a'],
                    ['a', 'b', 'a'],
                ]
        ),
        (
                [
                    ['c', 'b', 'a'],
                    ['c', 'b', 'a'],
                    ['c', 'b', 'a'],
                    ['c', 'b', 'a'],
                ],
                [
                    ['c', 'c', 'c', 'c'],
                    ['c', 'b', 'a'],
                    ['b', 'b', 'b', 'b'],
                    ['a', 'a', 'a', 'a'],
                    ['c', 'b', 'a'],
                    ['c', 'b', 'a'],
                    ['c', 'b', 'a'],
                ]
        ),
    ]
)
def test_chains(grid: Grid, expected: list[list[str]]):
    result = get_word_chains(grid, {'a', 'b', 'c'})
    assert result == expected


@pytest.mark.parametrize(
    'grid, expected',
    [
        (
                [
                    [None, 'baba', None],
                    ['a', 'is', 'a'],
                    [None, 'you', None],
                ],
                [
                    [
                        ['baba', 'is', 'you'],
                        [[['nouns']], 'verbs', [['adjectives']]]
                    ],
                ]
        ),
        (
                [
                    ['rock', 'baba', 'baba'],
                    ['rock', 'has', 'flag'],
                    ['rock', 'rock', 'is'],
                    ['rock', 'rock', 'you'],
                ],
                [
                    [
                        ['rock', 'has', 'flag'],
                        [[['nouns']], 'verbs', [['nouns']]]
                    ],
                    [
                        ['flag', 'is', 'you'],
                        [[['nouns']], 'verbs', [['adjectives']]]
                    ],
                    [
                        ['baba', 'has', 'rock'],
                        [[['nouns']], 'verbs', [['nouns']]]
                    ],
                ]
        ),
        (
                [
                    ['baba', 'on', 'rock', 'is', 'not', 'you', 'you'],
                ],
                [
                    [
                        ['baba', 'on', 'rock', 'is', 'not', 'you'],
                        [[['nouns', 'relations', 'nouns']], 'verbs', [['modifiers', 'adjectives']]]
                    ]
                ],
        ),
        (
                [
                    ['baba', 'on', 'rock', 'is', 'not', 'you', 'and', 'rock', 'on', 'you'],
                ],
                [
                    [
                        ['baba', 'on', 'rock', 'is', 'not', 'you', 'and', 'rock'],
                        [
                            [['nouns', 'relations', 'nouns']],
                            'verbs',
                            [['modifiers', 'adjectives'], 'conjunctions', ['nouns']],
                        ]
                    ]
                ],
        ),
        (
                [
                    line.split(',')
                    for line in open('baba-test.csv', 'r').read().splitlines()
                ],
                [
                    [
                        ["baba", "is", "you", "and", "win"],
                        [[["nouns"]], "verbs", [["adjectives"], "conjunctions", ["adjectives"]]]
                    ],
                    [
                        ["baba", "has", "box"],
                        [[["nouns"]], "verbs", [["nouns"]]]
                    ],
                    [
                        ["rock", "and", "win", "has", "box"],
                        [[["nouns"], "conjunctions", ["adjectives"]], "verbs", [["nouns"]]]
                    ],
                    [
                        ["baba", "is", "baba"],
                        [[["nouns"]], "verbs", [["nouns"]]]
                    ]
                ],
        )
    ],
    ids=[
        'cross',
        'full',
        'long',
        'conjunction',
        'csv-test',
    ]
)
def test_get_sentences(grid: Grid, expected: list[list[str]]):
    result = get_sentences(grid, {
        'nouns': {'baba', 'rock', 'flag', 'box'},
        'verbs': {'is', 'has'},
        'modifiers': {'not'},
        'adjectives': {'you', 'win'},
        'conjunctions': {'and'},
        'relations': {'on', 'near'},
    })
    assert PrettyPrint(result) == PrettyPrint(expected)
