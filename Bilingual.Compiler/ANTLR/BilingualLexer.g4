/* ========================================================================== */
/*                               Bilingual Lexer                              */
/* ========================================================================== */

lexer grammar BilingualLexer;

/* ================================= Symbols ================================ */
CurlyOpen: '{' -> pushMode(DEFAULT_MODE);
CurlyClosed: '}' -> popMode;
SquareOpen: '[';
SquareClosed: ']';
ParenOpen: '(';
ParenClosed: ')';

Semicolon: ';';
Colon: ':';

Equal: '=';
NotEqual: '!=';
Bang: '!';
EqualTo: '==';
PlusEqual: '+=';
MinusEqual: '-=';
MulEqual: '*=';
DivEqual: '/=';
Mod: '%';
Pow: '^';
Mul: '*';
Div: '/';
Add: '+';
Sub: '-';

GreaterThan: '>';
LessThan: '<';
GreaterThanEqual: '<=';
LessThanEqual: '>=';

Dot: '.';
Comma: ',';

Hash: '#';

/* ================================ Keywords ================================ */
True: 'true';
False: 'false';

If: 'if';
ElseIf: 'else if';
Else: 'else';

While: 'while';
Do: 'do';

For: 'for';
Foreach: 'foreach';

Continue: 'continue';
Break: 'break';
Return: 'return';

Await: 'await';

Var: 'var';
Global: 'global';
In: 'in';

Choose: 'choose';

Run: 'run';
Inject: 'inject';

Zero: 'zero';
One: 'one';
Two: 'two';
Other: 'other';
Few: 'few';
Many: 'many';
Plural: 'Plural' | 'Pl';
Ordinal: 'Ordinal' | 'Ord';

/* ========================= Literals and Fragments ========================= */

MemberName: Letter (Digit | Letter)*;
Number: Sub? Digit+ ( Dot Digit+ )?;

//ANTLR cannot match fixed amount of lexer rules, so it must be written out manually.
LineId: Hash Number Number Number Number Number Number Number Number LineIdComment?; 
LineIdComment: Colon String;

DoubleQuote: '"';
DollarDouble: '$"' -> pushMode(IN_STRING);

fragment Letter: [A-Za-z_];
fragment Digit: [0-9];

WS: [\t\n\r ] -> skip;

Comment: '//' ~[\r\n]* -> skip;

// https://stackoverflow.com/q/24557953
// https://stackoverflow.com/a/24559773
String: DoubleQuote (~["\n\r\\] | '\\' (. | EOF) )*  DoubleQuote;


/* =============================== String Mode ============================== */

mode IN_STRING;

// https://stackoverflow.com/a/69747874
EscapedLeftBrace: '\\{' -> type(Grapheme);
EscapedQuote: '\\"' -> type(Grapheme);
EscapedBackslash: '\\\\' -> type(Grapheme);
ExprStart: '{' -> type(CurlyOpen), pushMode(DEFAULT_MODE);
EndDouble: '"' -> type(DoubleQuote), popMode;
Grapheme: ~('{' | '"' | '\n' | '\r');