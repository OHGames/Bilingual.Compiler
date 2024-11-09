# Bilingual.Compiler
A dialogue-oriented portable scripting language inspired by C# and [YarnSpinner](https://www.yarnspinner.dev/).
This project will compile Bilingual files to be used with a Bilingual Runtime. This compiler is a runtime app that you can use to compile and localize.

[Click here for documentation.](Documentation/Index.md)

## Features of Bilingual and the Compiler
- Character names
- Compiles into JSON for portability across engines.
- Interpolated dialogue (inline content)
- Custom commands
- Async or sync support for commands
- Basic API allows for flexibility
- Control over wait command
- C# inspired syntax
- Compiled scripts can be run on engines with a supported runtime
- Localization support
    - Plural/Ordinal support across languages
    - Accounts for possibility of grammatical gender and abbreviation in pluralized/ordinalized words
- Generate localization files in csv format based from Bilingual Files
- MIT License and open source