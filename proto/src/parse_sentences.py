from typing import TypedDict, Literal

Grid = list[list[str | None]]


class Vocabulary(TypedDict):
    verbs: set[str]
    adjectives: set[str]
    nouns: set[str]
    modifiers: set[str]
    conjunctions: set[str]
    relations: set[str]


VerbsLiteral = Literal['verbs']
AdjectivesLiteral = Literal['adjectives']
NounsLiteral = Literal['nouns']
ModifiersLiteral = Literal['modifiers']
ConjunctionsLiteral = Literal['conjunctions']
RelationsLiteral = Literal['relations']

Noun = tuple[ModifiersLiteral, NounsLiteral] | tuple[NounsLiteral]
Adjective = tuple[ModifiersLiteral, AdjectivesLiteral] | tuple[AdjectivesLiteral]
NA = Noun | Adjective
Specifier = tuple[NA] | tuple[NA, RelationsLiteral, NA]
Clause = tuple[Specifier] | tuple[Specifier, ConjunctionsLiteral, Specifier]
Sentence = tuple[Clause, VerbsLiteral, Clause]


def get_word_chains(grid: Grid, words: set[str]) -> list[list[str]]:
    """grid: a list of columns grid[x][y] where x increases to the right and y increases down"""
    starts = []
    for x, col in enumerate(grid):
        for y, word in enumerate(col):
            if word not in words:
                continue

            if x < len(grid) - 2 and grid[x + 1][y] in words:
                starts.append((x, y, 'right'))
            if y < len(grid[x]) - 2 and grid[x][y + 1] in words:
                starts.append((x, y, 'down'))

    consumed = set()
    chains = []
    for x, y, dir in starts:
        chain = []
        while x < len(grid) and y < len(grid[x]) and grid[x][y] in words:
            m = (x, y, dir)
            if m in consumed:
                break
            consumed.add(m)
            chain.append(grid[x][y])
            if dir == 'down':
                y += 1
            elif dir == 'right':
                x += 1
        else:
            chains.append(chain)
    return chains


def get_sentences(grid: Grid, vocab: Vocabulary):
    """grid: a list of columns grid[x][y] where x increases to the right and y increases down"""
    chains = get_word_chains(grid, {word for words in vocab.values() for word in words})

    # verbs = grammar['verbs']
    # adjectives = grammar['adjectives']
    # nouns = grammar['nouns']
    # modifiers = grammar['modifiers']
    # conjunctions = grammar['conjunctions']
    # relations = grammar['relations']

    def match_specifier(alist: list[str], exclude: set[str]) -> Specifier | None:
        can_be = {'modifiers', 'nouns', 'adjectives'}
        p = []
        for word in alist:
            part_of_speech = [
                c
                for c in can_be
                if word in vocab[c] and word not in exclude
            ]
            if not part_of_speech:
                return None
            pos = part_of_speech[0]
            if pos == 'modifiers':
                can_be = {'nouns', 'adjectives', 'relations'}
            elif pos == 'adjectives':
                can_be = {'modifiers', 'relations'}
            elif pos == 'relations':
                can_be = {'modifiers', 'nouns', 'adjectives'}
            elif pos == 'nouns':
                can_be = {'modifiers', 'relations'}

            p.append(pos)
        return tuple(p)

    def match_clause(alist: list[str], *, exclude: set[str] = None) -> Clause | None:
        """split by conjunctions, there can be many conjunctions"""
        exclude = exclude or set()
        specifiers: list[Specifier] = []
        last_i = 0
        for i, word in enumerate(alist):
            if word in vocab['conjunctions']:
                spec = match_specifier(alist[last_i:i], exclude)
                if not spec:
                    return None
                specifiers.append(spec)
                last_i = i + 1
        if l := alist[last_i:]:
            spec = match_specifier(l, exclude)
            if not spec:
                return None
            specifiers.append(spec)
        else:
            return None

        return tuple([
                         item
                         for pair in zip(specifiers, ['conjunctions'] * len(specifiers))
                         for item in pair
                     ][:-1])

    def match_sentence(alist: list[str]) -> Sentence:
        """split by verb. there should only be one verb in a sentence"""
        if v := [word for word in alist if word in vocab['verbs']]:
            first_verb = alist.index(v[0])
            p1, p2 = alist[:first_verb], alist[first_verb + 1:]
            if (m1 := match_clause(p1)) and (m2 := match_clause(p2, exclude=vocab['relations'])):
                return m1, 'verbs', m2

    sentences: list[tuple[list[str], Sentence]] = []
    while chains:
        orig = chains.pop()
        i = 1
        current = orig
        while current:
            if m := match_sentence(current):
                sentences.append((current, m))
                break
            elif len(current) > 3:
                current = current[:-1]
            else:
                current = orig[i:]
                i += 1
    return sentences
