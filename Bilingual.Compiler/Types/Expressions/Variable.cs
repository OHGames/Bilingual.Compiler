﻿namespace Bilingual.Compiler.Types.Expressions
{
    public record class Variable(string Name, List<Accessor> Accessors) : Expression;
}
