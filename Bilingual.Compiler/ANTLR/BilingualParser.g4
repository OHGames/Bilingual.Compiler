/* ========================================================================== */
/*                              Bilingual Parser                              */
/* ========================================================================== */

parser grammar BilingualParser;

options {
    tokenVocab = BilingualLexer;
}

/* =============================== Containers =============================== */
file: container+ EOF?;
container: member CurlyOpen script* CurlyClosed;

script: scriptAttributes* MemberName ParenOpen ParenClosed block;
scriptAttributes: SquareOpen MemberName ParenOpen expression ParenClosed SquareClosed;

/* ======================= Expressions and Statements ======================= */
expression
    : ParenOpen expression ParenClosed                                          #ParenthesesExpression
    | left=expression Pow right=expression                                      #PowExpr
    | left=expression ( Mul | Div | Mod ) right=expression                      #MulDivMod
    | left=expression ( Add | Sub) right=expression                             #AddSub
    | left=expression EqualTo right=expression                                  #EqualToExpr
    | left=expression NotEqual right=expression                                 #NotEqualToExpr
    | Bang right=expression                                                     #BangExpression
    | Sub right=expression                                                      #NegateExpression
    | Add right=expression                                                      #AbsoluteValueExpression
    | plusMinusMulDivEqual                                                      #PlusMinusMulDivEqualExpression       
    | left=expression ( GreaterThan | LessThan ) right=expression               #GreaterLessThan
    | left=expression ( GreaterThanEqual | LessThanEqual) right=expression      #GreaterThanLessThanEqual                       
    | incrementsAndDecrements                                                   #IncrementAndDecrementExpr
    | functionCall                                                              #FunctionCallExpr
    | member                                                                    #MemberExpression
    | arrayAccess                                                               #ArrayAccessExpression
    | literal                                                                   #LiteralExpr
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
    | runStatement Semicolon                                        #RunStmt
    | injectStatement Semicolon                                     #InjectStmt
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
member: ( MemberName Dot )* MemberName;

ifStatement: If ParenOpen expression ParenClosed block ifElseStatement* elseStatement?;
ifElseStatement: ElseIf ParenOpen expression ParenClosed block;
elseStatement: Else block;

whileStatement: While ParenOpen expression ParenClosed block;
doWhileStatement: Do block While ParenOpen expression ParenClosed;

forStatement: For ParenOpen variableDeclaration loopCondition=expression Semicolon alterIndex=expression ParenClosed block;
forEachStatement: Foreach ParenOpen item=MemberName In collection=expression ParenClosed block;

dialogueStatement: MemberName dialogueEmotion? Colon (String | interpolationString);
dialogueEmotion: ParenOpen MemberName ParenClosed;

chooseStatement: chooseBlock chooseBlock chooseBlock*;
chooseBlock: Choose expression block;

functionCall: Await? member ParenOpen param* ParenClosed;
param: expression Comma?;

arrayIndexer: SquareOpen expression SquareClosed;

arrayObject: SquareOpen ( expression Comma )* expression? SquareClosed;
//accessor: MemberName ( ParenOpen param* ParenClosed )? arrayIndexer?;

arrayAccess: ( String | member | arrayObject ) arrayIndexer;

runStatement: Run member;
injectStatement: Inject member;

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

// https://github.com/sepp2k/antlr4-string-interpolation-examples
stringContents : Text                                               #TextStringContent
               | StringCurly expression CurlyClosed                 #ExpressionStringContent
               | StringCurly pluralizedQuantity CurlyClosed         #PluralizedStringContent
               ;

interpolationString: DollarDouble stringContents* DoubleQuote;

pluralizedQuantity: Plural ParenOpen expression Comma pluralCountParam (Comma pluralCountParam)*? ParenClosed;
pluralCountParam: (Zero | One | Two | Other | Few | Many) Equal String;