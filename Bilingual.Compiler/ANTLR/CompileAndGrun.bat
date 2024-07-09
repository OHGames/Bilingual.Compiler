CALL antlr4 BilingualLexer.g4 -o "%~dp0\Java"
CALL antlr4 BilingualParser.g4 -o "%~dp0\Java"
cd .\Java\
CALL javac Bilingual*.java
CALL grun Bilingual file -gui -tree "%~dp0\test.bi"
PAUSE