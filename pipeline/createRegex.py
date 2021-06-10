def OR(*alist: str):
    return F'({"|".join(alist)})'

VERBS = OR('is ', 'has ', 'eat ', 'grab ', 'follow ', 'make ', 'mimic ', 'fear ')

AND = 'and '

OBJ_MODIFIERS = OR('not ', 'lonely ', 'idle ')
SUB_MODIFIERS = OR('not ', 'seldom ', 'often ')

COMPOUNDERS = OR('on ', 'feeling ', 'near ')

WORD = '(\w{2,})'

OBJECT = F'{OBJ_MODIFIERS}?{WORD}'
SUBJECT = F'{SUB_MODIFIERS}?{WORD}'

def AND(n1: str, n2: str, string: str):
    return f'(?<{n1}>{string})(?<{n2}>(?: and {string})*)'

OBJ_COMPOUNDERS = F'{OBJECT} (?:not )?{COMPOUNDERS}{WORD}'
SUB_COMPOUNDERS = F'{SUBJECT} (?:not )?{COMPOUNDERS}{WORD}'
OBJECT = OR(OBJ_COMPOUNDERS, OBJECT)
SUBJECT = OR(SUB_COMPOUNDERS, SUBJECT)
ALL_OBJECT = AND('obj1', 'objRest', OBJECT)
ALL_SUBJECT = AND('sub1', 'subRest', SUBJECT)


OVERALL_STRUCTURE = F'^{ALL_OBJECT} (?<verb>{VERBS}){ALL_SUBJECT}$'

print(f'''
OVERALL:
{OVERALL_STRUCTURE}

OBJECT:
{OBJECT}

SUBJECT:
{SUBJECT}

''')

