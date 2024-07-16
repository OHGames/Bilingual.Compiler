using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Expressions;

namespace Bilingual.Compiler
{
    /// <summary>
    /// Misc helper functions.
    /// </summary>
    public static class VisitorHelpers
    {
        public static BilingualVisitor Visitor;

        /// <summary>
        /// Constant folding is when we simplify math expressions at compile time
        /// because they are constant values (ex: 6 * 4).
        /// </summary>
        /// <returns>If foldable</returns>
        public static bool CanFold(Expression left, Expression right, 
            out double leftValue, out double rightValue)
        {
            if (left is Literal leftLit && right is Literal rightLit)
            {
                if (leftLit.IsDouble() && rightLit.IsDouble())
                {
                    leftValue = (double)leftLit.Value;
                    rightValue = (double)rightLit.Value;
                    return true;
                }
            }

            leftValue = rightValue = double.NaN;
            return false;
        }

        /// <summary>
        /// Fold the values by performing the operations.
        /// </summary>
        /// <returns>A new literal with the folded value</returns>
        /// <exception cref="Exception">If a non-math or non-conditional operator is used.</exception>
        public static Literal Fold(double l, Operator opr, double r)
        {
            return opr switch
            {
                // returns a double
                Operator.Pow => new Literal(Math.Pow(l, r)),
                Operator.Mul => new Literal(l * r),
                Operator.Div => new Literal(l / r),
                Operator.Mod => new Literal(l % r),
                Operator.Add => new Literal(l + r),
                Operator.Sub => new Literal(l - r),

                // returns a bool
                Operator.GreaterThanEqualTo => new Literal(l >= r),
                Operator.LessThanEqualTo => new Literal(l <= r),
                Operator.GreaterThan => new Literal(l > r),
                Operator.LessThan => new Literal(l < r),

                // returns a bool
                Operator.NotEqualTo => new Literal(l != r),
                Operator.EqualTo => new Literal(l == r),

                _ => throw new Exception("You can only fold math expressions.")
            };
        }

        /// <summary>If the expression has the possibility of being a conditional.</summary>
        public static bool IsConditionalExpression(Expression expression)
        {
            if (expression is Literal literal)
            {
                return literal.IsBool();
            }
            else if (expression is OprExpression oprExpression)
            {
                return oprExpression.Operator.IsConditionalOperator();
            }
            else if (expression is Variable)
            {
                // variables have potential to be a bool.
                return true;
            }

            return false;
        }

        /// <summary>If the condition has the possibility of being a math expression.</summary>
        public static bool IsMathExpression(Expression expression)
        {
            if (expression is Literal literal)
            {
                return literal.IsDouble();
            }
            else if (expression is OprExpression oprExpression)
            {
                return oprExpression.Operator.IsMathOperator();
            }
            else if (expression is Variable)
            {
                // variables have potential to be a double.
                return true;
            }

            return false;
        }

        /// <summary>Get the accessors from a list of accessor contexts.</summary>
        public static List<Accessor> GetAccessors(BilingualParser.AccessorContext[]? accessorContexts)
        {
            if (accessorContexts is null || accessorContexts.Length == 0) 
                return [];

            List<Accessor> accessors = [];
            for (int i = 0; i < accessorContexts.Length; i++)
            {
                accessors.Add((Accessor)Visitor.Visit(accessorContexts[i]));
            }

            return accessors;
        }
        
        /// <summary>Return a <see cref="Params"/> from a list of param contexts.</summary>
        public static Params GetParams(BilingualParser.ParamContext[]? paramContexts)
        {
            if (paramContexts is null || paramContexts.Length == 0)
                return new Params([]);

            List<Expression> expressions = [];
            for (int i = 0; i < paramContexts.Length; i++)
            {
                expressions.Add((Expression)Visitor.Visit(paramContexts[i]));
            }

            return new Params(expressions);
        }

        /// <summary>If the operator is a conditional operator (returns true/false).</summary>
        public static bool IsConditionalOperator(this Operator opr)
        {
            return opr == Operator.EqualTo || opr == Operator.NotEqualTo
                || opr == Operator.GreaterThanEqualTo || opr == Operator.LessThanEqualTo
                || opr == Operator.GreaterThan || opr == Operator.LessThan;
        }

        /// <summary>If the operator is a math operator (returns a number).</summary>
        public static bool IsMathOperator(this Operator opr)
        {
            return opr == Operator.Pow || opr == Operator.Mul
                || opr == Operator.Div || opr == Operator.Mod
                || opr == Operator.Add || opr == Operator.Sub
                || opr == Operator.PlusPlus || opr == Operator.MinusEqual
                || opr == Operator.MulEqual || opr == Operator.DivEqual
                || opr == Operator.PlusEqual || opr == Operator.MinusEqual;
        }

        /// <summary>The String lexer rule returns double qoutes, get rid of the outer ones.</summary>
        public static string StripStartingQuotes(this string str)
        {
            str = str[1..^1];
            return str;
        }
    }
}
