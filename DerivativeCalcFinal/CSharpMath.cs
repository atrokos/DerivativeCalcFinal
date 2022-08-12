﻿using AngouriMath;
using ExprTree;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CSharpMath
{
    namespace Differentiation
    {
        public class MathExpression
        {
            private Head tree;
            private readonly Parser parser;
            private string expr_string;
            private bool isDifferentiable;

            public MathExpression(string newexpr, string VAR, bool withSteps)
            {
                parser = new(VAR);
                
                if (withSteps)
                {
                    Entity entity = newexpr;
                    expr_string = entity.Simplify().ToString();
                }
                else
                {
                    expr_string = newexpr.ToLower();
                }

                try
                {
                    tree = parser.ConvertToTree(expr_string);
                    isDifferentiable = true;
                }
                catch
                {
                    isDifferentiable = false;
                }
            }
            public override string ToString()
            {
                Entity result = expr_string;
                return result.Latexise();
            }
            public void Simplify()
            {
                Entity simple = expr_string;
                expr_string = simple.Simplify().ToString();
            }
            public void Differentiate() // Thought: Further derivations can be stored in a List, so List[0] is OG func., List[1] is 1st der. etc.
            {
                tree.Differentiate();
                expr_string = parser.ConvertToInfix(tree);
            }
            public void DifferentiateSteps()
            {
                Storage.Steps = new();
                tree.DifferentiateSteps();
                expr_string = parser.ConvertToInfix(tree);
            }
            public bool IsDifferentiable()
            {
                return isDifferentiable;
            }

        }
        class Parser
        {
            private static readonly HashSet<string> functions = new HashSet<string> { "sin", "cos", "tg", "tan", "cotg", "cotan", "abs", "arcsin", "arccos", "arctg", "arccotg", "ln", "sqrt", "cbrt" };
            private static readonly HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/", "^" };
            private static readonly HashSet<string> variables = new HashSet<string> { "e", "pi" };
            private readonly string VAR;
            private int pos;

            public Parser(string vAR)
            {
                VAR = vAR;
            }
            public Head ConvertToTree(string expression)
            {
                Head tree = PrefixToTree(ListToPrefix(ExprToList(expression)));
                return tree;
            }
            public string ConvertToInfix(Head tree)
            {
                string expression = PrefixToInfix(TreeToPrefix(tree));
                pos = 0;
                return expression;
            }
            public string ConvertToInfixHead(INode node)
            {
                Head head = new();
                head.Add(node);

                string expression = PrefixToInfix(TreeToPrefix(head));
                Entity entity = expression;
                pos = 0;
                return entity.ToString();
            }

            List<string> ExprToList(string simplified)
            {
                List<string> result = new();
                int i = 0;
                bool oper_needed = false;
                while (i < simplified.Length)
                {
                    string str_current = simplified[i].ToString();
                    if (simplified[i] == ' ')
                    {
                        i++;
                        continue;
                    }
                    if (char.IsDigit(simplified[i]))
                    {
                        if (oper_needed)
                            result.Add("*");
                        string numb = "";
                        do
                        {
                            numb += simplified[i];
                            i++;

                        } while (i < simplified.Length && char.IsDigit(simplified[i]));

                        result.Add(numb);
                        oper_needed = true;
                        continue;
                    }
                    else if (operators.Contains(str_current))
                    {
                        result.Add(str_current);
                        i++;
                        oper_needed = false;
                        continue;
                    }
                    else if (str_current == VAR)
                    {
                        if (oper_needed)
                            result.Add("*");
                        result.Add(str_current);
                        i++;
                        oper_needed = true;
                        continue;
                    }
                    else if (simplified[i] == ')')
                    {
                        result.Add(str_current);
                        i++;
                        oper_needed = true;
                        continue;
                    }
                    else if (simplified[i] == '(')
                    {
                        if (oper_needed)
                            result.Add("*");
                        result.Add(str_current);
                        i++;
                        oper_needed = false;
                        continue;
                    }
                    else
                    {
                        string func = "";
                        do
                        {
                            func += simplified[i];
                            i++;
                            if (variables.Contains(func))
                            {
                                break;
                            }

                        } while (i < simplified.Length && simplified[i] != '(' && Char.IsLetter(simplified[i]));

                        if (functions.Contains(func))
                        {
                            if (oper_needed)
                                result.Add("*");
                            oper_needed = false;
                            result.Add(func);
                            i++;
                            continue;
                        }
                        else if (variables.Contains(func) || func.Length == 1)
                        {
                            if (oper_needed)
                                result.Add("*");
                            oper_needed = true;
                            result.Add(func);
                            continue;
                        }
                        throw new Exception("Unknown Token: Unknown Function or Variable");
                    }
                }
                return result;
            }
            List<string> ListToPrefix(List<string> expr)
            {
                List<string> result = new();
                Stack<string> operatorstack = new();
                for (int i = expr.Count - 1; i >= 0; i--)
                {
                    string current = expr[i];
                    if (operators.Contains(current))
                    {
                        if (operatorstack.Count > 0)
                        {
                            if (GetPriority(current) > GetPriority(operatorstack.Peek()))
                            {
                                operatorstack.Push(current);
                            }
                            else if (GetPriority(current) < GetPriority(operatorstack.Peek()))
                            {
                                do
                                {
                                    result.Add(operatorstack.Pop());
                                }
                                while (operatorstack.Count > 0 && GetPriority(current) < GetPriority(operatorstack.Peek()));
                                operatorstack.Push(current);
                            }
                            else if (operatorstack.Peek() == "^")
                            {
                                do
                                {
                                    result.Add(operatorstack.Pop());
                                }
                                while (operatorstack.Count > 0 && GetPriority(current) <= GetPriority(operatorstack.Peek()) && operatorstack.Peek() != "^");
                                operatorstack.Push(current);
                            }
                            else
                            {
                                operatorstack.Push(current);
                            }
                        }
                        else
                        {
                            operatorstack.Push(current);
                        }
                    }
                    else if (functions.Contains(current))
                    {
                        while (operatorstack.Peek() != ")")
                        {
                            result.Add(operatorstack.Pop());
                        }
                        operatorstack.Pop();
                        result.Add(current);
                    }
                    else if (current == ")")
                    {
                        operatorstack.Push(current);
                    }
                    else if (current == "(")
                    {
                        while (operatorstack.Peek() != ")")
                        {
                            result.Add(operatorstack.Pop());
                        }
                        operatorstack.Pop();
                    }
                    else
                    {
                        result.Add(current);
                    }
                }

                while (operatorstack.Count != 0)
                {
                    result.Add(operatorstack.Pop());
                }

                result.Reverse();
                return result;
            }
            Head PrefixToTree(List<string> expr)
            {
                Head node = new();
                node.Add(BuildTree(expr));
                return node;
            }
            List<string> TreeToPrefix(Head tree)
            {
                Stack<INode> stack = new();
                List<string> result = new();

                stack.Push(tree.leftchild);

                while (stack.Count > 0)
                {
                    INode node = stack.Pop();
                    if (node is Constant constant)
                    {
                        if (constant.Value == Math.E)
                            result.Add("e");
                        else if (constant.Value == Math.PI)
                            result.Add("pi");
                        else
                        {
                            double val = constant.Value;
                            result.Add(val.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    else if (node is DiffVariable)
                    {
                        result.Add(VAR);
                    }
                    else if (node is LetterConstant letterConstant)
                    {
                        result.Add(letterConstant.Letter);
                    }
                    else if (node is OPNode opnode)
                    {
                        switch (opnode)
                        {
                            case Plus:
                                result.Add("+");
                                stack.Push(opnode.rightchild);
                                stack.Push(opnode.leftchild);
                                break;
                            case Minus:
                                result.Add("-");
                                stack.Push(opnode.rightchild);
                                stack.Push(opnode.leftchild);
                                break;
                            case Multi:
                                result.Add("*");
                                stack.Push(opnode.rightchild);
                                stack.Push(opnode.leftchild);
                                break;
                            case Divi:
                                result.Add("/");
                                stack.Push(opnode.rightchild);
                                stack.Push(opnode.leftchild);
                                break;
                            case Power:
                                result.Add("^");
                                stack.Push(opnode.rightchild);
                                stack.Push(opnode.leftchild);
                                break;
                            case Sin:
                                result.Add("sin");
                                stack.Push(opnode.leftchild);
                                break;
                            case Cos:
                                result.Add("cos");
                                stack.Push(opnode.leftchild);
                                break;
                            case Tg:
                                result.Add("tan");
                                stack.Push(opnode.leftchild);
                                break;
                            case Cotg:
                                result.Add("cotan");
                                stack.Push(opnode.leftchild);
                                break;
                            case Arcsin:
                                result.Add("arcsin");
                                stack.Push(opnode.leftchild);
                                break;
                            case Arccos:
                                result.Add("arccos");
                                stack.Push(opnode.leftchild);
                                break;
                            case Arctg:
                                result.Add("arctan");
                                stack.Push(opnode.leftchild);
                                break;
                            case Arccotg:
                                result.Add("arccotan");
                                stack.Push(opnode.leftchild);
                                break;
                            case Abs:
                                result.Add("abs");
                                stack.Push(opnode.leftchild);
                                break;
                            case Log:
                                result.Add("ln");
                                stack.Push(opnode.leftchild);
                                break;
                            default:
                                throw new Exception($"TreeToPrefix: UNKNOWN OPTOKEN: {node}");
                        }
                    }
                }
                return result;
            }
            string PrefixToInfix(List<string> expr)
            {
                Stack<string> stack = new();

                for (int i = expr.Count - 1; i >= 0; i--)
                {
                    string curr = expr[i];
                    if (operators.Contains(curr))
                    {
                        string op1 = stack.Pop();
                        string op2 = stack.Pop();

                        string temp = "(" + op1 + curr + op2 + ")";
                        stack.Push(temp);
                    }
                    else if (functions.Contains(curr))
                    {
                        string op1 = stack.Pop();

                        string temp = curr + "(" + op1 + ")";
                        stack.Push(temp);
                    }
                    else
                    {
                        stack.Push(curr);
                    }
                }
                return stack.Pop();
            }
            INode BuildTree(List<string> expr)
            {
                INode node;
                double n;

                if (double.TryParse(expr[pos], out n)) //It is a number
                {
                    Constant constant = new(n);
                    node = constant;
                }
                else if (expr[pos] == VAR) //It is a derivative variable
                {
                    DiffVariable variable = new();
                    node = variable;
                }
                else //It is an operator or a function
                {
                    switch (expr[pos])
                    {
                        case "e":
                            Constant euler = new(Math.E);
                            node = euler;
                            break;
                        case "pi":
                            Constant pi = new(Math.PI);
                            node = pi;
                            break;
                        case "+":
                            Plus plus = new();
                            pos++;
                            plus.Add(BuildTree(expr));

                            pos++;
                            plus.Add(BuildTree(expr));
                            node = plus;
                            break;
                        case "-":
                            Minus minus = new();
                            pos++;
                            minus.Add(BuildTree(expr));

                            pos++;
                            minus.Add(BuildTree(expr));
                            node = minus;
                            break;
                        case "*":
                            Multi multi = new();
                            pos++;
                            multi.Add(BuildTree(expr));

                            pos++;
                            multi.Add(BuildTree(expr));
                            node = multi;
                            break;
                        case "/":
                            Divi divi = new();
                            pos++;
                            divi.Add(BuildTree(expr));

                            pos++;
                            divi.Add(BuildTree(expr));
                            node = divi;
                            break;
                        case "^":
                            Power power = new();
                            pos++;
                            power.Add(BuildTree(expr));

                            pos++;
                            power.Add(BuildTree(expr));
                            node = power;
                            break;
                        case "sin":
                            Sin sin = new();
                            pos++;
                            sin.Add(BuildTree(expr));
                            node = sin;
                            break;
                        case "cos":
                            Cos cos = new();
                            pos++;
                            cos.Add(BuildTree(expr));
                            node = cos;
                            break;
                        case "tan":
                        case "tg":
                            Tg tg = new();
                            pos++;
                            tg.Add(BuildTree(expr));
                            node = tg;
                            break;
                        case "cotan":
                        case "cotg":
                            Cotg cotg = new();
                            pos++;
                            cotg.Add(BuildTree(expr));
                            node = cotg;
                            break;
                        case "arcsin":
                            Arcsin arcsin = new();
                            pos++;
                            arcsin.Add(BuildTree(expr));
                            node = arcsin;
                            break;
                        case "arccos":
                            Arccos arccos = new();
                            pos++;
                            arccos.Add(BuildTree(expr));
                            node = arccos;
                            break;
                        case "arctg":
                            Arctg arctg = new();
                            pos++;
                            arctg.Add(BuildTree(expr));
                            node = arctg;
                            break;
                        case "arccotg":
                            Arccotg arccotg = new();
                            pos++;
                            arccotg.Add(BuildTree(expr));
                            node = arccotg;
                            break;
                        case "abs":
                            Abs abs = new();
                            pos++;
                            abs.Add(BuildTree(expr));
                            node = abs;
                            break;
                        case "ln":
                            Log log = new();
                            pos++;
                            log.Add(BuildTree(expr));
                            node = log;
                            break;
                        case "sqrt":
                            Power power1 = new();
                            pos++;
                            power1.Add(BuildTree(expr));

                            Divi sqrtdivi = new();
                            Constant one = new(1), two = new(2);
                            sqrtdivi.SetChildren(one, two);
                            power1.Add(sqrtdivi);

                            node = power1;
                            break;
                        case "cbrt":
                            Power power2 = new();
                            pos++;
                            power2.Add(BuildTree(expr));

                            Divi cbrtdivi = new();
                            Constant one2 = new(1), three = new(3);
                            cbrtdivi.SetChildren(one2, three);
                            power2.Add(cbrtdivi);

                            node = power2;
                            break;
                        default:
                            if (expr[pos].Length == 1)
                            {
                                LetterConstant letter = new(expr[pos]);
                                node = letter;
                                break;
                            }
                            throw new Exception("UNKNOWN OPTOKEN");
                    }
                }
                return node;
            }
            int GetPriority(string oper)
            {
                switch (oper)
                {
                    case "+":
                        return 1;
                    case "-":
                        return 1;
                    case "*":
                        return 2;
                    case "/":
                        return 2;
                    case "^":
                        return 3;
                    default:
                        if (operators.Contains(oper))
                            return 2;
                        return 0;
                }
            }
        }

        public class StepGenerator
        {
            List<string> GeneratedSteps = new();
            private int pos = -1;
            private Parser parser;
            private const int marginIncrease = 8;

            public StepGenerator(string VAR)
            {
                parser = new(VAR);
            }

            void GenerateFurther(int margin, int color, int NoOfChildren)
            {
                if (NoOfChildren == 2)
                {
                    Generate(margin + marginIncrease, color + 1);
                    Generate(margin + marginIncrease, color + 1);
                    GeneratedSteps.Add($"/margin {margin}");
                    GeneratedSteps.Add($"/background {color}");
                    GeneratedSteps.Add("Po úpravě:");
                }
                else
                {
                    Generate(margin + marginIncrease, color + 1);
                    GeneratedSteps.Add($"/margin {margin}");
                    GeneratedSteps.Add($"/background {color}");
                    GeneratedSteps.Add("Po úpravě:");
                }
            }
            void Generate(int margin, int color)
            {
                pos++;

                List<string> nodes = new();
                foreach (INode node in Storage.Steps[pos])
                {
                    Entity entity = parser.ConvertToInfixHead(node);
                    nodes.Add(entity.Simplify().Latexise());
                }

                GeneratedSteps.Add($"/margin {margin}");
                GeneratedSteps.Add($"/background {color}");
                GeneratedSteps.Add($"/math [{nodes[0]}]'");
                switch (Storage.Steps[pos][0])
                {
                    case Constant:
                    case LetterConstant:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.constant);
                        break;
                    case DiffVariable:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.diffvariable);
                        break;
                    case Plus:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.addition);
                        GeneratedSteps.Add($"/math [{nodes[1]}]' + [{nodes[2]}]'");
                        GenerateFurther(margin, color, 2);
                        GeneratedSteps.Add($"/math {nodes[3]} + {nodes[4]}");
                        break;
                    case Minus:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.substraction);
                        GeneratedSteps.Add($"/math [{nodes[1]}]' - [{nodes[2]}]'");
                        GenerateFurther(margin, color, 2);
                        GeneratedSteps.Add($"/math {nodes[3]} - {nodes[4]}");
                        break;
                    case Multi:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.multiplication);
                        GeneratedSteps.Add($"/math [{nodes[1]}]'*{nodes[2]} + {nodes[1]}*[{nodes[2]}]'");
                        GenerateFurther(margin, color, 2);
                        GeneratedSteps.Add($"/math {nodes[3]}");
                        break;
                    case Divi:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.division);
                        GeneratedSteps.Add($"/math \\frac{{ [{nodes[1]}]'*({nodes[2]}) - ({nodes[1]})*[{nodes[2]}]' }}{{ {nodes[2]}^2 }}");
                        GenerateFurther(margin, color, 2);
                        GeneratedSteps.Add($"/math {nodes[3]}");
                        break;
                    case Power:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.power);
                        GeneratedSteps.Add($"/math {nodes[0]} * [ln({nodes[1]}) * {nodes[2]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[3]}");
                        break;
                    case Sin:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.sin);
                        GeneratedSteps.Add($"/math cos({nodes[1]})*[{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Cos:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.cos);
                        GeneratedSteps.Add($"/math -sin({nodes[1]})*[{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Tg:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.tg);
                        GeneratedSteps.Add($"/math \\frac{{1}}{{cos({nodes[1]})^2}} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Cotg:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.cotg);
                        GeneratedSteps.Add($"/math \\frac{{-1}}{{sin({nodes[1]})^2}} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Arcsin:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.arcsin);
                        GeneratedSteps.Add($"/math \\frac{{1}}{{\\sqrt{{ 1-({nodes[1]})^2 }} }} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Arccos:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.arccos);
                        GeneratedSteps.Add($"/math \\frac{{-1}}{{\\sqrt{{ 1-{nodes[1]}^2 }} }} * [{nodes[1]}]'");
                        GenerateFurther(margin + marginIncrease, color + 1, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Arctg:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.arctg);
                        GeneratedSteps.Add($"/math \\frac{{1}}{{ 1 + {nodes[1]}^2 }} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Arccotg:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.arccotg);
                        GeneratedSteps.Add($"/math \\frac{{-1}}{{ 1 + {nodes[1]}^2 }} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                    case Log:
                        GeneratedSteps.Add(DerivativeCalcFinal.Rules.ln);
                        GeneratedSteps.Add($"/math \\frac{{1}}{{ {nodes[1]} }} * [{nodes[1]}]'");
                        GenerateFurther(margin, color, 1);
                        GeneratedSteps.Add($"/math {nodes[2]}");
                        break;
                }
            }

            public List<string> StartGenerating()
            {
                GeneratedSteps = new List<string>();
                pos = -1;
                Generate(0,0);

                return GeneratedSteps;
            }
        }
    }
}
