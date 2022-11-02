using AngouriMath;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpMath.Core;
using CSharpMath.Parsing;

namespace CSharpMath
{
    namespace Expressions
    {
        public class MathExpression
        {
            private Head? tree;
            private readonly Parser parser;
            private bool isDifferentiable = true;
            List<string> expressions = new();

            public MathExpression(string newexpr, string VAR)
            {
                parser = new(VAR);
                if (!Regex.IsMatch(newexpr, @"^[0-9A-Za-z()+/*-^ ]+$") || !CorrectSyntax(newexpr.ToLower()))
                {
                    isDifferentiable = false;
                    return;
                }

                expressions.Add(newexpr.ToLower());
            }
            public int Diff_Count()
            {
                return expressions.Count - 1;
            }
            public string ToStringSpec(int index, bool latex) //Returns an expression in LaTeX on the given index
            {
                Entity result = expressions[index];
                if (latex)
                    return result.Latexise();

                return result.ToString();
            }
            public override string ToString() //Returns the last expression as a LaTeX string
            {
                Entity result = expressions[^1];
                return result.Latexise();
            }
            public void Simplify()
            {
                Entity simple = expressions[^1];
                expressions[^1] = simple.Simplify().ToString();
            }
            public void Differentiate(bool withSteps)
            {
                if (withSteps)
                {
                    Entity entity = expressions[^1];
                    expressions[^1] = entity.Simplify().ToString();
                    tree = parser.ConvertToTree(expressions[^1]);

                    Storage.Steps = new();
                    tree.DifferentiateSteps();
                    expressions.Add(parser.ConvertToInfix(tree));
                }
                else
                {
                    tree = parser.ConvertToTree(expressions[^1]);

                    tree.Differentiate();
                    expressions.Add(parser.ConvertToInfix(tree));
                }
            }
            public bool IsDifferentiable()
            {
                return isDifferentiable;
            }
            bool CorrectSyntax(string expression)
            {
                try
                {
                    parser.ConvertToTree(expression);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}
