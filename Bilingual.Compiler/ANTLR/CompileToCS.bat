:: https://stackoverflow.com/a/14942944
CALL antlr4 -Dlanguage=CSharp -o "%~dp0\CSharp" BilingualLexer.g4 -no-listener -visitor
CALL antlr4 -Dlanguage=CSharp -o "%~dp0\CSharp" BilingualParser.g4 -no-listener -visitor