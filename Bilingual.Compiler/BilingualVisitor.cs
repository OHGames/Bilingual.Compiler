using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Bilingual.Compiler.Exceptions;
using Bilingual.Compiler.Types;
using Bilingual.Compiler.Types.Containers;
using Bilingual.Compiler.Types.Expressions;
using Bilingual.Compiler.Types.Statements;
using Bilingual.Compiler.Types.Statements.ControlFlow;

namespace Bilingual.Compiler
{
    /// <summary>
    /// Use the generated ANTLR parser to visit the files and create C# representations 
    /// of the syntax tree in memory to be serialized/compiled into JSON.
    /// </summary>
    public partial class BilingualVisitor : BilingualParserBaseVisitor<BilingualObject>
    {
        public BilingualVisitor()
        {
            VisitorHelpers.Visitor = this;
        }

        public override BilingualFile VisitFile([NotNull] BilingualParser.FileContext context)
        {
            var containerContexts = context.container();

            List<ScriptContainer> containers = [];
            for (int i = 0; i < containerContexts.Length; i++)
            {
                containers.Add(VisitContainer(containerContexts[i]));
            }

            return new BilingualFile(containers);
        }

        public override ScriptContainer VisitContainer([NotNull] BilingualParser.ContainerContext context)
        {
            var scriptContexts = context.script();
            var containerNameContext = context.containerName();

            List<Script> scripts = [];
            if (scriptContexts is not null && scriptContexts.Length != 0)
            {
                for (int i = 0; i < scriptContexts.Length; i++)
                {
                    scripts.Add(VisitScript(scriptContexts[i]));
                }
            }

            var name = containerNameContext.GetText();

            return new ScriptContainer(name, scripts);
        }

        public override VariableAssignment VisitMemberAssignment([NotNull] BilingualParser.MemberAssignmentContext context)
        {
            var expressionContext = context.expression();
            var memberContext = context.member();

            var expression = VisitExpression(expressionContext);
            var variable = VisitMember(memberContext);

            return new VariableAssignment(variable, expression);
        }

        public override DialogueStatement VisitDialogueStmt([NotNull] BilingualParser.DialogueStmtContext context)
        {
            var statementContext = context.dialogueStatement();
            var lineIdContext = context.LineId();

            // no line id
            if (lineIdContext is null) return VisitDialogueStatement(statementContext);

            var statement = VisitDialogueStatement(statementContext);
            var lineId = lineIdContext.ToString()!.Remove(0, 1); // remove starting #
            string? comment = null;

            if (lineId!.Contains(':'))
            {
                // get the comment. Remove the colon and starting quote
                comment = lineId.Substring(lineId.IndexOf(':') + 2);
                // remove ending quote
                comment = comment.Remove(comment.Length - 1, 1);

                // remove colon and comment
                lineId = lineId[..8];
            }

            var success = uint.TryParse(lineId, out uint id);
            if (!success) throw new InvalidCastException("Cannot convert line id to uint. " +
                    $"See line {context.Start.Line}.");

            return statement with { LineId = id, TranslationComment = comment };
        }

        // This one gets parsed in any statement, but in Stmt above, dialogue ids are possibly added.
        public override DialogueStatement VisitDialogueStatement([NotNull] BilingualParser.DialogueStatementContext context)
        {
            var memberContext = context.MemberName();
            var emotionContext = context.dialogueEmotion();
            var stringContext = context.String();
            var interpContext = context.interpolationString();

            var member = memberContext.GetText();
            var emotion = Visit(emotionContext) as Literal;
            var dialogue = stringContext != null 
                ? new Literal(stringContext.GetText().StripStartingQuotes()) 
                : VisitExpression(interpContext);

            string? emotionString = null;
            if (emotion is not null)
            {
                emotionString = (string)emotion.Value;
            }

            return new DialogueStatement(member, emotionString, dialogue, null, null) { FileLine = context.Start.Line };
        }

        public override InterpolatedString VisitInterpolationString([NotNull] BilingualParser.InterpolationStringContext context)
        {
            var stringContexts = context.stringContents();
            List<Expression> expressions = [];

            for (int i = 0; i < stringContexts.Length; i++)
            {
                expressions.Add(VisitExpression(stringContexts[i]));
            }

            return new InterpolatedString(expressions);
        }

        public override Literal VisitTextStringContent([NotNull] BilingualParser.TextStringContentContext context)
        {
            return new Literal(context.GetText());
        }

        public override Expression VisitExpressionStringContent(
            [NotNull] BilingualParser.ExpressionStringContentContext context)
        {
            return VisitExpression(context.expression());
        }

        public override Literal VisitDialogueEmotion([NotNull] BilingualParser.DialogueEmotionContext context)
        {
            return new Literal(context.MemberName().ToString()!);
        }

        public override BreakStatement VisitBreakStmt([NotNull] BilingualParser.BreakStmtContext context)
        {
            return new BreakStatement();
        }

        public override ContinueStatement VisitContinueStmt([NotNull] BilingualParser.ContinueStmtContext context)
        {
            return new ContinueStatement();
        }

        public override ReturnStatement VisitReturnStmt([NotNull] BilingualParser.ReturnStmtContext context)
        {
            return new ReturnStatement();
        }

        public override ChooseStatement VisitChooseStatement([NotNull] BilingualParser.ChooseStatementContext context)
        {
            var chooseBlockContexts = context.chooseBlock();

            // no need to check if null, all choose statements have at least 2 blocks
            List<ChooseBlock> blocks = [];
            for (int i = 0; i < chooseBlockContexts.Length; i++)
            {
                blocks.Add(VisitChooseBlock(chooseBlockContexts[i]));
            }

            return new ChooseStatement(blocks);
        }

        public override ChooseBlock VisitChooseBlock([NotNull] BilingualParser.ChooseBlockContext context)
        {
            var expressionContext = context.expression();
            var blockContext = context.block();

            var expression = VisitExpression(expressionContext);
            var block = VisitBlock(blockContext);

            return new ChooseBlock(expression, block);
        }

        public override ForEachStatement VisitForEachStatement([NotNull] BilingualParser.ForEachStatementContext context)
        {
            var blockContext = context.block();

            var itemName = context.item.Text;
            var collectionExpression = VisitExpression(context.collection);
            var block = VisitBlock(blockContext);

            return new ForEachStatement(itemName, collectionExpression, block);
        }

        public override ForStatement VisitForStatement([NotNull] BilingualParser.ForStatementContext context)
        {
            var declarationContext = context.variableDeclaration();
            var blockExpression = context.block();

            var declaration = VisitVariableDeclaration(declarationContext);
            var loopExpression = VisitExpression(context.loopCondition);
            var alterIndex = VisitExpression(context.alterIndex);
            var block = VisitBlock(blockExpression);

            return new ForStatement(declaration, loopExpression, 
                alterIndex, block);
        }

        public override BilingualObject VisitWhileStatement([NotNull] BilingualParser.WhileStatementContext context)
        {
            var expressionContext = context.expression();
            var blockContext = context.block();

            var expression = VisitExpression(expressionContext);
            var block = VisitBlock(blockContext);

            if (!VisitorHelpers.IsConditionalExpression(expression))
            {
                throw new BilingualParsingException("Expressions in while statements must be an expression " +
                    "that may return true/false. " +
                    $"See line {context.Start.Line}.");
            }

            return new WhileStatement(expression, block);
        }

        public override BilingualObject VisitDoWhileStatement([NotNull] BilingualParser.DoWhileStatementContext context)
        {
            var expressionContext = context.expression();
            var blockContext = context.block();

            var expression = VisitExpression(expressionContext);
            var block = VisitBlock(blockContext);

            if (!VisitorHelpers.IsConditionalExpression(expression))
            {
                throw new BilingualParsingException("Expressions in do while statements must be an " +
                    "expression that may return true/false. " +
                    $"See line {context.Start.Line}, column {context.Start.Column}.");
            }

            return new DoWhileStatement(expression, block);
        }

        public override IfStatement VisitIfStatement([NotNull] BilingualParser.IfStatementContext context)
        {
            var expressionContext = context.expression();
            var elseIfContexts = context.ifElseStatement();
            var elseContext = context.elseStatement();
            var blockContext = context.block();

            var expression = VisitExpression(expressionContext);
            var block = VisitBlock(blockContext);

            if (!VisitorHelpers.IsConditionalExpression(expression))
            {
                throw new BilingualParsingException("Expressions in if statements must be an expression " +
                    "that may return true/false. " +
                    $"See line {context.Start.Line}.");
            }

            List<ElseIfStatement> elseIfStatements = [];
            if (elseIfContexts != null && elseIfContexts.Length != 0)
            {
                for (int i = 0; i < elseIfContexts.Length; i++)
                {
                    elseIfStatements.Add(VisitIfElseStatement(elseIfContexts[i]));
                }
            }

            // The shadowed Visit function returns null if elseContext is null
            // so we can just use the visit function because null means no statement
            // and because 'as' wont throw and exception if null.
            ElseStatement? elseStatement = Visit(elseContext) as ElseStatement;

            return new IfStatement(expression, block, elseIfStatements, elseStatement);
        }

        public override ElseIfStatement VisitIfElseStatement([NotNull] BilingualParser.IfElseStatementContext context)
        {
            var expressionContext = context.expression();
            var blockContext = context.block();

            var expression = VisitExpression(expressionContext);
            var block = VisitBlock(blockContext);

            if (!VisitorHelpers.IsConditionalExpression(expression))
            {
                throw new BilingualParsingException("Expressions in if statements must be an" +
                    " expression that may return true/false." +
                    $" See line {context.Start.Line}.");
            }

            return new ElseIfStatement(expression, block);
        }

        public override ElseStatement VisitElseStatement([NotNull] BilingualParser.ElseStatementContext context)
        {
            return new ElseStatement(VisitBlock(context.block()));
        }

        public override VariableDeclaration VisitVariableDeclaration([NotNull] BilingualParser.VariableDeclarationContext context)
        {
            var nameContext = context.MemberName();
            var expressionContext = context.expression();

            var expression = VisitExpression(expressionContext);
            var isGlobal = context.Global() is not null;
            var name = nameContext.GetText();

            return new VariableDeclaration(name, isGlobal, expression);
        }

        // The statement is just a functionCall but with an added semicolon, so just return
        // the expression housed in a statement object.
        public override BilingualObject VisitFunctionCallStmt([NotNull] BilingualParser.FunctionCallStmtContext context)
        {
            var functionContext = context.functionCall();
            var functionCall = VisitFunctionCall(functionContext);

            return new FunctionCallStatement(functionCall);
        }

        public override Block VisitBlock([NotNull] BilingualParser.BlockContext context)
        {
            var statementContexts = context.statement();

            if (statementContexts is null || statementContexts.Length == 0)
                return new Block([]);

            List<Statement> statements = [];
            for (int i = 0; i < statementContexts.Length; i++)
            {
                statements.Add((Statement)Visit(statementContexts[i]));
            }

            return new Block(statements);
        }

        public override OprExpression VisitPlusMinusEqualTo([NotNull] BilingualParser.PlusMinusEqualToContext context)
        {
            var oprContext = context.PlusEqual() ?? context.MinusEqual();
            var expressionContext = context.expression();
            var memberContext = context.member();

            var operation = oprContext.GetText() switch
            {
                "+=" => Operator.PlusEqual,
                "-=" => Operator.MinusEqual,
                _ => throw new InvalidOperationException("Invalid operator for PlusMinusEqualTo." +
                    $" See line {context.Start.Line}.")
            };

            var left = VisitMember(memberContext);
            var right = VisitExpression(expressionContext);

            return new OprExpression(left, operation, right);
        }

        // the statement just holds the expression so just visit the expression.
        public override BilingualObject VisitPlusMinusMulDivStmt([NotNull] BilingualParser.PlusMinusMulDivStmtContext context)
        {
            return new PlusMinusMulDivEqualStatement((OprExpression)Visit(context.plusMinusMulDivEqual()));
        }

        public override OprExpression VisitMulDivEqualTo([NotNull] BilingualParser.MulDivEqualToContext context)
        {
            var oprContext = context.MulEqual() ?? context.DivEqual();
            var expressionContext = context.expression();
            var memberContext = context.member();

            var operation = oprContext.GetText() switch
            {
                "*=" => Operator.MulEqual,
                "/=" => Operator.DivEqual,
                _ => throw new InvalidOperationException("Invalid operator for MulDivEqualTo." +
                    $" See line {context.Start.Line}.")
            };

            var left = VisitMember(memberContext);
            var right = VisitExpression(expressionContext);

            return new OprExpression(left, operation, right);
        }

        public override Expression VisitGreaterLessThan([NotNull] BilingualParser.GreaterLessThanContext context)
        {
            var oprContext = context.GreaterThan() ?? context.LessThan();

            var operation = oprContext.GetText() switch
            {
                ">" => Operator.GreaterThan,
                "<" => Operator.LessThan,
                _ => throw new InvalidOperationException("Invalid operator for GreaterLessThan." +
                    $" See line {context.Start.Line}.")
            };

            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, operation, r);
            }

            return new OprExpression(left, operation, right);
        }

        public override Expression VisitGreaterThanLessThanEqual
            ([NotNull] BilingualParser.GreaterThanLessThanEqualContext context)
        {
            var oprContext = context.GreaterThanEqual() ?? context.LessThanEqual();

            var operation = oprContext.GetText() switch
            {
                ">=" => Operator.GreaterThanEqualTo,
                "<=" => Operator.LessThanEqualTo,
                _ => throw new InvalidOperationException("Invalid operator for GreaterThanLessThanEqual." +
                    $" See line {context.Start.Line}.")
            };

            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, operation, r);
            }

            return new OprExpression(left, operation, right);
        }

        public override FunctionCallExpression VisitFunctionCall([NotNull] BilingualParser.FunctionCallContext context)
        {
            var accessorContexts = context.accessor();
            var memberContext = context.MemberName();
            var paramsContexts = context.param();

            var accessors = VisitorHelpers.GetAccessors(accessorContexts);
            var memberName = memberContext.GetText();
            var param = VisitorHelpers.GetParams(paramsContexts);

            return new FunctionCallExpression(memberName, accessors, param);
        }

        public override Accessor VisitAccessor([NotNull] BilingualParser.AccessorContext context)
        {
            var memberNameContext = context.MemberName();
            var arrayIndexerContext = context.arrayIndexer();
            var paramsContexts = context.param();

            if (arrayIndexerContext is not null && paramsContexts?.Length != 0)
            {
                throw new BilingualParsingException("Either one indexer OR parameter allowed." +
                    $" See line {context.Start.Line}.");
            }

            var memberName = memberNameContext.GetText();
            var arrayIndexer = Visit(arrayIndexerContext);
            var param = VisitorHelpers.GetParams(paramsContexts);

            // 'as' will ensure that no exception is thrown if null
            return new Accessor(memberName, arrayIndexer as Expression, param);
        }

        public override BilingualObject VisitParam([NotNull] BilingualParser.ParamContext context)
        {
            return Visit(context.expression());
        }

        public override ArrayAccess VisitArrayAccess([NotNull] BilingualParser.ArrayAccessContext context)
        {
            ParserRuleContext baseObjectContext = context.functionCall();
            baseObjectContext ??= context.member();
            var indexerContext = context.arrayIndexer();

            var baseObject = Visit(baseObjectContext);
            var indexer = VisitExpression(indexerContext);

            return new ArrayAccess(baseObject, indexer);
        }

        public override BilingualObject VisitArrayIndexer([NotNull] BilingualParser.ArrayIndexerContext context)
        {
            return Visit(context.expression());
        }

        public override OprExpression VisitUnaryIncrementLeft([NotNull] BilingualParser.UnaryIncrementLeftContext context)
        {
            var memberContext = context.member();
            var memberExpression = VisitExpression(memberContext);

            return new OprExpression(null!, Operator.PlusPlus, memberExpression);
        }

        public override OprExpression VisitUnaryIncrementRight([NotNull] BilingualParser.UnaryIncrementRightContext context)
        {
            var memberContext = context.member();
            var memberExpression = VisitExpression(memberContext);

            return new OprExpression(memberExpression, Operator.PlusPlus, null!);
        }

        public override OprExpression VisitUnaryDecrementLeft([NotNull] BilingualParser.UnaryDecrementLeftContext context)
        {
            var memberContext = context.member();
            var memberExpression = VisitMember(memberContext);

            return new OprExpression(null!, Operator.MinusMinus, memberExpression);
        }

        public override OprExpression VisitUnaryDecrementRight([NotNull] BilingualParser.UnaryDecrementRightContext context)
        {
            var memberContext = context.member();
            var memberExpression = VisitMember(memberContext);

            return new OprExpression(null!, Operator.MinusMinus, memberExpression);
        }

        public override Variable VisitMember([NotNull] BilingualParser.MemberContext context)
        {
            var memberNameContext = context.MemberName();
            var accessorContexts = context.accessor();

            var memberName = memberNameContext.GetText();

            return new Variable(memberName, VisitorHelpers.GetAccessors(accessorContexts));
        }

        public override BilingualObject VisitParenthesesExpression(
            [NotNull] BilingualParser.ParenthesesExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override Expression VisitMulDivMod([NotNull] BilingualParser.MulDivModContext context)
        {
            var operatorContext = context.Mul() ?? context.Div() ?? context.Mod();
            var opr = operatorContext.GetText() switch
            {
                "*" => Operator.Mul,
                "/" => Operator.Div,
                "%" => Operator.Mod,
                _ => throw new InvalidOperationException("MulDivMod has invalid operator." +
                    $" See line {context.Start.Line}.")
            };

            var expressionContexts = context.expression();
            var left = VisitExpression(expressionContexts[0]);
            var right = VisitExpression(expressionContexts[1]);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, opr, r);
            }

            return new OprExpression(left, opr, right);
        }

        public override BilingualObject VisitPowExpr([NotNull] BilingualParser.PowExprContext context)
        {
            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, Operator.Pow, r);
            }

            return new OprExpression(left, Operator.Pow, right);
        }

        public override Expression VisitAddSub([NotNull] BilingualParser.AddSubContext context)
        {
            var operatorContext = context.Add() ?? context.Sub();
            var opr = operatorContext.GetText() == "+" ? Operator.Add : Operator.Sub;

            if (operatorContext.GetText() != "+" && operatorContext.GetText() != "-")
                throw new InvalidOperationException("AddSub has invalid operator" +
                    $" See line {context.Start.Line}, column {context.Start.Column}.");

            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            // simplify and fold
            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, opr, r);
            }

            return new OprExpression(left, opr, right);
        }

        public override Expression VisitEqualToExpr([NotNull] BilingualParser.EqualToExprContext context)
        {
            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, Operator.EqualTo, r);
            }

            if (left is Literal leftLit && right is Literal rightLit)
            {
                return new Literal(leftLit.Value == rightLit.Value);
            }

            return new OprExpression(left, Operator.EqualTo, right);
        }

        public override Expression VisitNotEqualToExpr([NotNull] BilingualParser.NotEqualToExprContext context)
        {
            var left = VisitExpression(context.left);
            var right = VisitExpression(context.right);

            if (VisitorHelpers.CanFold(left, right, out double l, out double r))
            {
                return VisitorHelpers.Fold(l, Operator.NotEqualTo, r);
            }

            if (left is Literal leftLit && right is Literal rightLit)
            {
                return new Literal(leftLit.Value != rightLit.Value);
            }

            return new OprExpression(left, Operator.NotEqualTo, right);
        }

        public override Literal VisitStringLiteral([NotNull] BilingualParser.StringLiteralContext context)
        {
            var stringContext = context.String();
            var stringText = stringContext.GetText();

            // remove the " marks
            stringText = stringText.StripStartingQuotes();

            return new Literal(stringText);
        }

        public override Literal VisitTrueFalseLiteral([NotNull] BilingualParser.TrueFalseLiteralContext context)
        {
            var valueContext = context.True() ?? context.False();
            var valueText = valueContext.GetText();

            return valueText switch
            {
                "true" => new Literal(true),
                "false" => new Literal(false),
                _ => throw new InvalidOperationException("TrueFalse is not 'true' or 'false'." +
                    $" See line {context.Start.Line}.")
            };
        }

        public override Expression VisitBangExpression([NotNull] BilingualParser.BangExpressionContext context)
        {
            var expression = VisitExpression(context.expression());

            if (!VisitorHelpers.IsConditionalExpression(expression))
            {
                throw new BilingualParsingException("Only expressions that return true/false can have " +
                    "the not operator applied." +
                    $" See line {context.Start.Line}.");
            }

            if (expression is Literal literal)
            {
                if (literal.IsBool())
                {
                    return new Literal(!(bool)literal.Value);
                }
            }

            return new OprExpression(null, Operator.Bang, expression);
        }

        public override Expression VisitNegateExpression([NotNull] BilingualParser.NegateExpressionContext context)
        {
            var expression = VisitExpression(context.expression());

            if (!VisitorHelpers.IsMathExpression(expression))
            {
                throw new BilingualParsingException("Only math or number related expressions " +
                    "can be negated, no bools." +
                    $" See line {context.Start.Line}.");
            }

            if (expression is Literal literal)
            {
                if (literal.IsDouble())
                {
                    return new Literal(-(double)literal.Value);
                }
            }

            return new OprExpression(null, Operator.Sub, expression);
        }

        public override BilingualObject VisitAbsoluteValueExpression
            ([NotNull] BilingualParser.AbsoluteValueExpressionContext context)
        {
            var expression = VisitExpression(context.expression());

            if (!VisitorHelpers.IsMathExpression(expression))
            {
                throw new BilingualParsingException("Only math or number related expressions " +
                    "can get an absolute value, no bools."+
                    $" See line {context.Start.Line}.");
            }

            if (expression is Literal literal)
            {
                if (literal.IsDouble())
                {
                    return new Literal(Math.Abs((double)literal.Value));
                }
            }

            return new OprExpression(null, Operator.Add, expression);
        }

        public override Literal VisitNumberLiteral([NotNull] BilingualParser.NumberLiteralContext context)
        {
            var numberText = context.Number().GetText();
            var numberSuccess = double.TryParse(numberText, out double numberValue);

            if (!numberSuccess)
            {
                throw new InvalidCastException($"Number is not a valid double. " +
                    $"\nSee line {context.Start.Line}.");
            }

            return new Literal(numberValue);
        }

        public override Literal VisitArrayObject([NotNull] BilingualParser.ArrayObjectContext context)
        {
            var expressions = context.expression();

            if (expressions is null || expressions.Length == 0)
                return new Literal(new List<Expression>());

            List<Expression> vistedExpressions = [];

            for (int i = 0; i < expressions.Length; i++)
            {
                var expr = expressions[i];
                vistedExpressions.Add(VisitExpression(expr));
            }

            return new Literal(vistedExpressions);
        }

        public override ScriptAttribute VisitScriptAttributes([NotNull] BilingualParser.ScriptAttributesContext context)
        {
            var memberName = context.MemberName().GetText();
            var exprContext = context.expression();
            var expr = VisitExpression(exprContext);

            return new ScriptAttribute(memberName, expr);
        }

        public override Script VisitScript([NotNull] BilingualParser.ScriptContext context)
        {
            var attributeContexts = context.scriptAttributes();
            var memberContext = context.MemberName();
            var blockContext = context.block();

            var block = VisitBlock(blockContext);
            var name = memberContext.GetText();

            List<ScriptAttribute> attributes = [];
            if (attributeContexts is not null && attributeContexts.Length != 0)
            {
                for (int i = 0; i < attributeContexts.Length; i++)
                {
                    attributes.Add(VisitScriptAttributes(attributeContexts[i]));
                }
            }

            return new Script(name, block, attributes);
        }

        /// <summary>
        /// If a parse context is null, return null dont visit.
        /// </summary>
        public new BilingualObject Visit(IParseTree? tree)
        {
            if (tree is null) return null!;
            return base.Visit(tree);
        }

        /// <summary>Shorthand to visit an expression.</summary>
        public Expression VisitExpression(IParseTree tree)
        {
            return (Expression)Visit(tree);
        }
    }
}
