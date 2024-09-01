using Bilingual.Compiler.Types.Expressions;
using ReswPlusLib;

namespace Bilingual.Compiler.Types
{
    public record class LocalizedQuanity(Expression Value, Dictionary<PluralTypeEnum, string> Plurals) : Expression;
}

// Copyright (c) Rudy Huyn. All rights reserved.
// Licensed under the MIT License.
// Source: https://github.com/DotNetPlus/ReswPlus
namespace ReswPlusLib
{
    public enum PluralTypeEnum
    {
        ZERO,
        ONE,
        TWO,
        OTHER,
        FEW,
        MANY
    };
}