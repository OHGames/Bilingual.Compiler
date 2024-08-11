//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.12.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from BilingualLexer.g4 by ANTLR 4.12.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.12.0")]
[System.CLSCompliant(false)]
public partial class BilingualLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		CurlyOpen=1, CurlyClosed=2, SquareOpen=3, SquareClosed=4, ParenOpen=5, 
		ParenClosed=6, Semicolon=7, Colon=8, Equal=9, NotEqual=10, Bang=11, EqualTo=12, 
		PlusEqual=13, MinusEqual=14, MulEqual=15, DivEqual=16, Mod=17, Pow=18, 
		Mul=19, Div=20, Add=21, Sub=22, GreaterThan=23, LessThan=24, GreaterThanEqual=25, 
		LessThanEqual=26, Dot=27, Comma=28, Hash=29, True=30, False=31, If=32, 
		ElseIf=33, Else=34, While=35, Do=36, For=37, Foreach=38, Continue=39, 
		Break=40, Return=41, Await=42, Var=43, Global=44, In=45, Choose=46, Run=47, 
		Inject=48, MemberName=49, Number=50, LineId=51, LineIdComment=52, DoubleQuote=53, 
		DollarDouble=54, WS=55, Comment=56, String=57, EscapeSequence=58, StringCurly=59, 
		Text=60;
	public const int
		IN_STRING=1;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "IN_STRING"
	};

	public static readonly string[] ruleNames = {
		"CurlyOpen", "CurlyClosed", "SquareOpen", "SquareClosed", "ParenOpen", 
		"ParenClosed", "Semicolon", "Colon", "Equal", "NotEqual", "Bang", "EqualTo", 
		"PlusEqual", "MinusEqual", "MulEqual", "DivEqual", "Mod", "Pow", "Mul", 
		"Div", "Add", "Sub", "GreaterThan", "LessThan", "GreaterThanEqual", "LessThanEqual", 
		"Dot", "Comma", "Hash", "True", "False", "If", "ElseIf", "Else", "While", 
		"Do", "For", "Foreach", "Continue", "Break", "Return", "Await", "Var", 
		"Global", "In", "Choose", "Run", "Inject", "MemberName", "Number", "LineId", 
		"LineIdComment", "DoubleQuote", "DollarDouble", "Letter", "Digit", "WS", 
		"Comment", "String", "EscapeSequence", "StringCurly", "DoubleQuoteInString", 
		"Text"
	};


	public BilingualLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public BilingualLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, "'}'", "'['", "']'", "'('", "')'", "';'", "':'", "'='", "'!='", 
		"'!'", "'=='", "'+='", "'-='", "'*='", "'/='", "'%'", "'^'", "'*'", "'/'", 
		"'+'", "'-'", "'>'", "'<'", "'<='", "'>='", "'.'", "','", "'#'", "'true'", 
		"'false'", "'if'", "'else if'", "'else'", "'while'", "'do'", "'for'", 
		"'foreach'", "'continue'", "'break'", "'return'", "'await'", "'var'", 
		"'global'", "'in'", "'choose'", "'run'", "'inject'", null, null, null, 
		null, null, "'$\"'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "CurlyOpen", "CurlyClosed", "SquareOpen", "SquareClosed", "ParenOpen", 
		"ParenClosed", "Semicolon", "Colon", "Equal", "NotEqual", "Bang", "EqualTo", 
		"PlusEqual", "MinusEqual", "MulEqual", "DivEqual", "Mod", "Pow", "Mul", 
		"Div", "Add", "Sub", "GreaterThan", "LessThan", "GreaterThanEqual", "LessThanEqual", 
		"Dot", "Comma", "Hash", "True", "False", "If", "ElseIf", "Else", "While", 
		"Do", "For", "Foreach", "Continue", "Break", "Return", "Await", "Var", 
		"Global", "In", "Choose", "Run", "Inject", "MemberName", "Number", "LineId", 
		"LineIdComment", "DoubleQuote", "DollarDouble", "WS", "Comment", "String", 
		"EscapeSequence", "StringCurly", "Text"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "BilingualLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static BilingualLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,60,405,6,-1,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,
		6,7,6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,
		2,14,7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,
		2,21,7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,
		2,28,7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,
		2,35,7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,
		2,42,7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,
		2,49,7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,
		2,56,7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,2,62,7,62,
		1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,5,1,5,1,6,1,
		6,1,7,1,7,1,8,1,8,1,9,1,9,1,9,1,10,1,10,1,11,1,11,1,11,1,12,1,12,1,12,
		1,13,1,13,1,13,1,14,1,14,1,14,1,15,1,15,1,15,1,16,1,16,1,17,1,17,1,18,
		1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,22,1,22,1,23,1,23,1,24,1,24,1,24,
		1,25,1,25,1,25,1,26,1,26,1,27,1,27,1,28,1,28,1,29,1,29,1,29,1,29,1,29,
		1,30,1,30,1,30,1,30,1,30,1,30,1,31,1,31,1,31,1,32,1,32,1,32,1,32,1,32,
		1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,1,34,1,34,
		1,35,1,35,1,35,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,37,1,37,1,37,
		1,37,1,38,1,38,1,38,1,38,1,38,1,38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,
		1,39,1,39,1,40,1,40,1,40,1,40,1,40,1,40,1,40,1,41,1,41,1,41,1,41,1,41,
		1,41,1,42,1,42,1,42,1,42,1,43,1,43,1,43,1,43,1,43,1,43,1,43,1,44,1,44,
		1,44,1,45,1,45,1,45,1,45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,47,1,47,
		1,47,1,47,1,47,1,47,1,47,1,48,1,48,1,48,5,48,310,8,48,10,48,12,48,313,
		9,48,1,49,3,49,316,8,49,1,49,4,49,319,8,49,11,49,12,49,320,1,49,1,49,4,
		49,325,8,49,11,49,12,49,326,3,49,329,8,49,1,50,1,50,1,50,1,50,1,50,1,50,
		1,50,1,50,1,50,1,50,3,50,341,8,50,1,51,1,51,1,51,1,52,1,52,1,53,1,53,1,
		53,1,53,1,53,1,54,1,54,1,55,1,55,1,56,1,56,1,56,1,56,1,57,1,57,1,57,1,
		57,5,57,365,8,57,10,57,12,57,368,9,57,1,57,1,57,1,58,1,58,1,58,1,58,1,
		58,3,58,377,8,58,5,58,379,8,58,10,58,12,58,382,9,58,1,58,1,58,1,59,1,59,
		1,59,1,60,1,60,1,60,1,60,1,61,1,61,1,61,1,61,1,61,1,62,1,62,1,62,1,62,
		4,62,402,8,62,11,62,12,62,403,0,0,63,2,1,4,2,6,3,8,4,10,5,12,6,14,7,16,
		8,18,9,20,10,22,11,24,12,26,13,28,14,30,15,32,16,34,17,36,18,38,19,40,
		20,42,21,44,22,46,23,48,24,50,25,52,26,54,27,56,28,58,29,60,30,62,31,64,
		32,66,33,68,34,70,35,72,36,74,37,76,38,78,39,80,40,82,41,84,42,86,43,88,
		44,90,45,92,46,94,47,96,48,98,49,100,50,102,51,104,52,106,53,108,54,110,
		0,112,0,114,55,116,56,118,57,120,58,122,59,124,0,126,60,2,0,1,7,3,0,65,
		90,95,95,97,122,1,0,48,57,3,0,9,10,13,13,32,32,2,0,10,10,13,13,4,0,10,
		10,13,13,34,34,92,92,2,0,34,34,123,123,1,0,92,92,414,0,2,1,0,0,0,0,4,1,
		0,0,0,0,6,1,0,0,0,0,8,1,0,0,0,0,10,1,0,0,0,0,12,1,0,0,0,0,14,1,0,0,0,0,
		16,1,0,0,0,0,18,1,0,0,0,0,20,1,0,0,0,0,22,1,0,0,0,0,24,1,0,0,0,0,26,1,
		0,0,0,0,28,1,0,0,0,0,30,1,0,0,0,0,32,1,0,0,0,0,34,1,0,0,0,0,36,1,0,0,0,
		0,38,1,0,0,0,0,40,1,0,0,0,0,42,1,0,0,0,0,44,1,0,0,0,0,46,1,0,0,0,0,48,
		1,0,0,0,0,50,1,0,0,0,0,52,1,0,0,0,0,54,1,0,0,0,0,56,1,0,0,0,0,58,1,0,0,
		0,0,60,1,0,0,0,0,62,1,0,0,0,0,64,1,0,0,0,0,66,1,0,0,0,0,68,1,0,0,0,0,70,
		1,0,0,0,0,72,1,0,0,0,0,74,1,0,0,0,0,76,1,0,0,0,0,78,1,0,0,0,0,80,1,0,0,
		0,0,82,1,0,0,0,0,84,1,0,0,0,0,86,1,0,0,0,0,88,1,0,0,0,0,90,1,0,0,0,0,92,
		1,0,0,0,0,94,1,0,0,0,0,96,1,0,0,0,0,98,1,0,0,0,0,100,1,0,0,0,0,102,1,0,
		0,0,0,104,1,0,0,0,0,106,1,0,0,0,0,108,1,0,0,0,0,114,1,0,0,0,0,116,1,0,
		0,0,0,118,1,0,0,0,1,120,1,0,0,0,1,122,1,0,0,0,1,124,1,0,0,0,1,126,1,0,
		0,0,2,128,1,0,0,0,4,132,1,0,0,0,6,136,1,0,0,0,8,138,1,0,0,0,10,140,1,0,
		0,0,12,142,1,0,0,0,14,144,1,0,0,0,16,146,1,0,0,0,18,148,1,0,0,0,20,150,
		1,0,0,0,22,153,1,0,0,0,24,155,1,0,0,0,26,158,1,0,0,0,28,161,1,0,0,0,30,
		164,1,0,0,0,32,167,1,0,0,0,34,170,1,0,0,0,36,172,1,0,0,0,38,174,1,0,0,
		0,40,176,1,0,0,0,42,178,1,0,0,0,44,180,1,0,0,0,46,182,1,0,0,0,48,184,1,
		0,0,0,50,186,1,0,0,0,52,189,1,0,0,0,54,192,1,0,0,0,56,194,1,0,0,0,58,196,
		1,0,0,0,60,198,1,0,0,0,62,203,1,0,0,0,64,209,1,0,0,0,66,212,1,0,0,0,68,
		220,1,0,0,0,70,225,1,0,0,0,72,231,1,0,0,0,74,234,1,0,0,0,76,238,1,0,0,
		0,78,246,1,0,0,0,80,255,1,0,0,0,82,261,1,0,0,0,84,268,1,0,0,0,86,274,1,
		0,0,0,88,278,1,0,0,0,90,285,1,0,0,0,92,288,1,0,0,0,94,295,1,0,0,0,96,299,
		1,0,0,0,98,306,1,0,0,0,100,315,1,0,0,0,102,330,1,0,0,0,104,342,1,0,0,0,
		106,345,1,0,0,0,108,347,1,0,0,0,110,352,1,0,0,0,112,354,1,0,0,0,114,356,
		1,0,0,0,116,360,1,0,0,0,118,371,1,0,0,0,120,385,1,0,0,0,122,388,1,0,0,
		0,124,392,1,0,0,0,126,401,1,0,0,0,128,129,5,123,0,0,129,130,1,0,0,0,130,
		131,6,0,0,0,131,3,1,0,0,0,132,133,5,125,0,0,133,134,1,0,0,0,134,135,6,
		1,1,0,135,5,1,0,0,0,136,137,5,91,0,0,137,7,1,0,0,0,138,139,5,93,0,0,139,
		9,1,0,0,0,140,141,5,40,0,0,141,11,1,0,0,0,142,143,5,41,0,0,143,13,1,0,
		0,0,144,145,5,59,0,0,145,15,1,0,0,0,146,147,5,58,0,0,147,17,1,0,0,0,148,
		149,5,61,0,0,149,19,1,0,0,0,150,151,5,33,0,0,151,152,5,61,0,0,152,21,1,
		0,0,0,153,154,5,33,0,0,154,23,1,0,0,0,155,156,5,61,0,0,156,157,5,61,0,
		0,157,25,1,0,0,0,158,159,5,43,0,0,159,160,5,61,0,0,160,27,1,0,0,0,161,
		162,5,45,0,0,162,163,5,61,0,0,163,29,1,0,0,0,164,165,5,42,0,0,165,166,
		5,61,0,0,166,31,1,0,0,0,167,168,5,47,0,0,168,169,5,61,0,0,169,33,1,0,0,
		0,170,171,5,37,0,0,171,35,1,0,0,0,172,173,5,94,0,0,173,37,1,0,0,0,174,
		175,5,42,0,0,175,39,1,0,0,0,176,177,5,47,0,0,177,41,1,0,0,0,178,179,5,
		43,0,0,179,43,1,0,0,0,180,181,5,45,0,0,181,45,1,0,0,0,182,183,5,62,0,0,
		183,47,1,0,0,0,184,185,5,60,0,0,185,49,1,0,0,0,186,187,5,60,0,0,187,188,
		5,61,0,0,188,51,1,0,0,0,189,190,5,62,0,0,190,191,5,61,0,0,191,53,1,0,0,
		0,192,193,5,46,0,0,193,55,1,0,0,0,194,195,5,44,0,0,195,57,1,0,0,0,196,
		197,5,35,0,0,197,59,1,0,0,0,198,199,5,116,0,0,199,200,5,114,0,0,200,201,
		5,117,0,0,201,202,5,101,0,0,202,61,1,0,0,0,203,204,5,102,0,0,204,205,5,
		97,0,0,205,206,5,108,0,0,206,207,5,115,0,0,207,208,5,101,0,0,208,63,1,
		0,0,0,209,210,5,105,0,0,210,211,5,102,0,0,211,65,1,0,0,0,212,213,5,101,
		0,0,213,214,5,108,0,0,214,215,5,115,0,0,215,216,5,101,0,0,216,217,5,32,
		0,0,217,218,5,105,0,0,218,219,5,102,0,0,219,67,1,0,0,0,220,221,5,101,0,
		0,221,222,5,108,0,0,222,223,5,115,0,0,223,224,5,101,0,0,224,69,1,0,0,0,
		225,226,5,119,0,0,226,227,5,104,0,0,227,228,5,105,0,0,228,229,5,108,0,
		0,229,230,5,101,0,0,230,71,1,0,0,0,231,232,5,100,0,0,232,233,5,111,0,0,
		233,73,1,0,0,0,234,235,5,102,0,0,235,236,5,111,0,0,236,237,5,114,0,0,237,
		75,1,0,0,0,238,239,5,102,0,0,239,240,5,111,0,0,240,241,5,114,0,0,241,242,
		5,101,0,0,242,243,5,97,0,0,243,244,5,99,0,0,244,245,5,104,0,0,245,77,1,
		0,0,0,246,247,5,99,0,0,247,248,5,111,0,0,248,249,5,110,0,0,249,250,5,116,
		0,0,250,251,5,105,0,0,251,252,5,110,0,0,252,253,5,117,0,0,253,254,5,101,
		0,0,254,79,1,0,0,0,255,256,5,98,0,0,256,257,5,114,0,0,257,258,5,101,0,
		0,258,259,5,97,0,0,259,260,5,107,0,0,260,81,1,0,0,0,261,262,5,114,0,0,
		262,263,5,101,0,0,263,264,5,116,0,0,264,265,5,117,0,0,265,266,5,114,0,
		0,266,267,5,110,0,0,267,83,1,0,0,0,268,269,5,97,0,0,269,270,5,119,0,0,
		270,271,5,97,0,0,271,272,5,105,0,0,272,273,5,116,0,0,273,85,1,0,0,0,274,
		275,5,118,0,0,275,276,5,97,0,0,276,277,5,114,0,0,277,87,1,0,0,0,278,279,
		5,103,0,0,279,280,5,108,0,0,280,281,5,111,0,0,281,282,5,98,0,0,282,283,
		5,97,0,0,283,284,5,108,0,0,284,89,1,0,0,0,285,286,5,105,0,0,286,287,5,
		110,0,0,287,91,1,0,0,0,288,289,5,99,0,0,289,290,5,104,0,0,290,291,5,111,
		0,0,291,292,5,111,0,0,292,293,5,115,0,0,293,294,5,101,0,0,294,93,1,0,0,
		0,295,296,5,114,0,0,296,297,5,117,0,0,297,298,5,110,0,0,298,95,1,0,0,0,
		299,300,5,105,0,0,300,301,5,110,0,0,301,302,5,106,0,0,302,303,5,101,0,
		0,303,304,5,99,0,0,304,305,5,116,0,0,305,97,1,0,0,0,306,311,3,110,54,0,
		307,310,3,112,55,0,308,310,3,110,54,0,309,307,1,0,0,0,309,308,1,0,0,0,
		310,313,1,0,0,0,311,309,1,0,0,0,311,312,1,0,0,0,312,99,1,0,0,0,313,311,
		1,0,0,0,314,316,3,44,21,0,315,314,1,0,0,0,315,316,1,0,0,0,316,318,1,0,
		0,0,317,319,3,112,55,0,318,317,1,0,0,0,319,320,1,0,0,0,320,318,1,0,0,0,
		320,321,1,0,0,0,321,328,1,0,0,0,322,324,3,54,26,0,323,325,3,112,55,0,324,
		323,1,0,0,0,325,326,1,0,0,0,326,324,1,0,0,0,326,327,1,0,0,0,327,329,1,
		0,0,0,328,322,1,0,0,0,328,329,1,0,0,0,329,101,1,0,0,0,330,331,3,58,28,
		0,331,332,3,100,49,0,332,333,3,100,49,0,333,334,3,100,49,0,334,335,3,100,
		49,0,335,336,3,100,49,0,336,337,3,100,49,0,337,338,3,100,49,0,338,340,
		3,100,49,0,339,341,3,104,51,0,340,339,1,0,0,0,340,341,1,0,0,0,341,103,
		1,0,0,0,342,343,3,16,7,0,343,344,3,118,58,0,344,105,1,0,0,0,345,346,5,
		34,0,0,346,107,1,0,0,0,347,348,5,36,0,0,348,349,5,34,0,0,349,350,1,0,0,
		0,350,351,6,53,2,0,351,109,1,0,0,0,352,353,7,0,0,0,353,111,1,0,0,0,354,
		355,7,1,0,0,355,113,1,0,0,0,356,357,7,2,0,0,357,358,1,0,0,0,358,359,6,
		56,3,0,359,115,1,0,0,0,360,361,5,47,0,0,361,362,5,47,0,0,362,366,1,0,0,
		0,363,365,8,3,0,0,364,363,1,0,0,0,365,368,1,0,0,0,366,364,1,0,0,0,366,
		367,1,0,0,0,367,369,1,0,0,0,368,366,1,0,0,0,369,370,6,57,3,0,370,117,1,
		0,0,0,371,380,3,106,52,0,372,379,8,4,0,0,373,376,5,92,0,0,374,377,9,0,
		0,0,375,377,5,0,0,1,376,374,1,0,0,0,376,375,1,0,0,0,377,379,1,0,0,0,378,
		372,1,0,0,0,378,373,1,0,0,0,379,382,1,0,0,0,380,378,1,0,0,0,380,381,1,
		0,0,0,381,383,1,0,0,0,382,380,1,0,0,0,383,384,3,106,52,0,384,119,1,0,0,
		0,385,386,5,92,0,0,386,387,9,0,0,0,387,121,1,0,0,0,388,389,5,123,0,0,389,
		390,1,0,0,0,390,391,6,60,0,0,391,123,1,0,0,0,392,393,5,34,0,0,393,394,
		1,0,0,0,394,395,6,61,4,0,395,396,6,61,1,0,396,125,1,0,0,0,397,402,8,5,
		0,0,398,399,8,6,0,0,399,400,5,92,0,0,400,402,5,123,0,0,401,397,1,0,0,0,
		401,398,1,0,0,0,402,403,1,0,0,0,403,401,1,0,0,0,403,404,1,0,0,0,404,127,
		1,0,0,0,15,0,1,309,311,315,320,326,328,340,366,376,378,380,401,403,5,5,
		0,0,4,0,0,5,1,0,6,0,0,7,53,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
