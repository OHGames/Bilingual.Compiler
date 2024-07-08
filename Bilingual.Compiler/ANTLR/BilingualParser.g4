/* ========================================================================== */
/*                              Bilingual Parser                              */
/* ========================================================================== */

parser grammar BilingualParser;

options {
    tokenVocab = BilingualLexer;
}

/* =============================== Containers =============================== */
file: container+ EOF?;
container: MemberName CurlyOpen script* CurlyClosed;
script: scriptAttributes* MemberName ParenOpen ParenClosed block;
scriptAttributes: SquareOpen MemberName ParenOpen expression ParenClosed SquareClosed;

/* ======================= Expressions and Statements ======================= */
expression
    : ParenOpen expression ParenClosed                              #ParenthesesExpression
    | expression Pow expression                                     #PowExpr
    | expression ( Mul | Div | Mod ) expression                     #MulDivMod
    | expression ( Add | Sub) expression                            #AddSub
    | expression EqualTo expression                                 #EqualToExpr
    | expression NotEqual expression                                #NotEqualToExpr
    | Bang expression                                               #BangExpression
    | Sub expression                                                #NegateExpression
    | Add expression                                                #AbsoluteValueExpression
    | plusMinusMulDivEqual                                          #PlusMinusMulDivEqualExpression       
    | expression ( GreaterThan | LessThan ) expression              #GreaterLessThan
    | expression ( GreaterThanEqual | LessThanEqual) expression     #GreaterThanLessThanEqual                       
    | incrementsAndDecrements                                       #IncrementAndDecrementExpr
    | functionCall                                                  #FunctionCallExpr
    | member                                                        #MemberExpression
    | arrayAccess                                                   #ArrayAccessExpression
    | literal                                                       #LiteralExpr
    ;

literal
    : arrayObject                                                   #ArrayLiteral
    | ( True | False )                                              #TrueFalseLiteral
    | Number                                                        #NumberLiteral
    | String                                                        #StringLiteral
    ;
    
block: ( statement | CurlyOpen statement* CurlyClosed );

statement
    : doWhileStatement Semicolon                                    #DoWhileStmt
    | dialogueStatement Semicolon LineId?                           #DialogueStmt
    | memberAssignment Semicolon                                    #MemberAssignmentStmt
    | functionCall Semicolon                                        #FunctionCallStmt
    | incrementsAndDecrements Semicolon                             #IncrementDecrementStmt
    | plusMinusMulDivEqual Semicolon                                #PlusMinusMulDivStmt
    | Continue Semicolon                                            #ContinueStmt
    | Break Semicolon                                               #BreakStmt
    | Return Semicolon                                              #ReturnStmt
    | ifStatement                                                   #IfStmt
    | variableDeclaration                                           #VariableDeclarationStmt
    | whileStatement                                                #WhileStmt
    | forStatement                                                  #ForStmt
    | forEachStatement                                              #ForEachStmt
    | chooseStatement                                               #ChooseStmt
    ;


/* ============================= Statement Rules ============================ */
variableDeclaration: Global? Var MemberName Equal expression Semicolon;
memberAssignment: member Equal expression;
member: ( accessor Dot )* MemberName;

ifStatement: If ParenOpen expression ParenClosed block ifElseStatement* elseStatement?;
ifElseStatement: ElseIf ParenOpen expression ParenClosed block;
elseStatement: Else block;

whileStatement: While ParenOpen expression ParenClosed block;
doWhileStatement: Do block While ParenOpen expression ParenClosed;

forStatement: For ParenOpen variableDeclaration expression Semicolon expression ParenClosed block;
forEachStatement: Foreach ParenOpen expression In expression ParenClosed block;

dialogueStatement: MemberName dialogueEmotion? Colon expression;
dialogueEmotion: ParenOpen MemberName ParenClosed;

chooseStatement: chooseBlock chooseBlock chooseBlock*;
chooseBlock: Choose expression block;

functionCall: ( accessor Dot )* MemberName ParenOpen param* ParenClosed;
param: expression Comma?;

arrayIndexer: SquareOpen expression SquareClosed;

arrayObject: SquareOpen ( expression Comma )* expression? SquareClosed;
accessor: MemberName ( ParenOpen param* ParenClosed )? arrayIndexer?;

arrayAccess: ( functionCall | member ) arrayIndexer;

unaryIncrementLeft: Add Add member;
unaryIncrementRight: member Add Add;
unaryDecrementLeft: Sub Sub member;
unaryDecrementRight: member Sub Sub;

plusMinusMulDivEqual
    : member ( MulEqual | DivEqual ) expression                     #MulDivEqualTo 
    | member ( PlusEqual | MinusEqual ) expression                  #PlusMinusEqualTo 
    ;

incrementsAndDecrements
    : unaryIncrementLeft
    | unaryIncrementRight
    | unaryDecrementLeft
    | unaryDecrementRight
    ;