using System;
using System.Collections.Generic;
using CSharpMath.Core;
using AngouriMath;
using System.Globalization;

namespace CSharpMath.Parsing
{
    class Parser
    {
        private static readonly HashSet<string> functions = new HashSet<string> { "sin", "cos", "tg", "tan", "cotg", "cotan", "abs", "arcsin", "arccos", "arctg", "arccotg", "ln", "sqrt", "cbrt" };
        private static readonly HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/", "^" };
        private static readonly HashSet<string> variables = new HashSet<string> { "e", "pi" };
        private List<string> expr = new();
        private readonly string VAR;
        private int pos = -1;

        public Parser(string vAR)
        {
            VAR = vAR;
        }
        public Head ConvertToTree(string expression)
        {
            Head tree = PrefixToTree(ListToPrefix(ExprToList(expression)));
            pos = -1;
            return tree;
        }
        public string ConvertToInfix(Head tree)
        {
            string expression = PrefixToInfix(TreeToPrefix(tree));
            //pos = 0;
            return expression;
        }
        public string ConvertToInfixHead(INode node)
        {
            Head head = new(node);

            string expression = PrefixToInfix(TreeToPrefix(head));
            Entity entity = expression;
            pos = -1;
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
                    if (str_current.Equals("-") && (i == 0 || simplified[i - 1].ToString().Equals("(")))
                    {
                        result.Add("0");
                    }
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
            this.expr = expr;
            Head node = new(BuildTree());
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
                switch (node)
                {
                    case Constant constant:
                        if (constant.Value == Math.E)
                            result.Add("e");
                        else if (constant.Value == Math.PI)
                            result.Add("pi");
                        else
                        {
                            double val = constant.Value;
                            result.Add(val.ToString(CultureInfo.InvariantCulture));
                        }
                        break;
                    case DiffVariable:
                        result.Add(VAR);
                        break;
                    case LetterConstant letterConstant:
                        result.Add(letterConstant.Letter);
                        break;
                    case OPNode opnode:
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
                            default:
                                throw new Exception($"TreeToPrefix: UNKNOWN OPTOKEN: {node}");
                        }
                        break;
                    case Function function:
                        switch (function)
                        {
                            case Sin:
                                result.Add("sin");
                                stack.Push(function.leftchild);
                                break;
                            case Cos:
                                result.Add("cos");
                                stack.Push(function.leftchild);
                                break;
                            case Tg:
                                result.Add("tan");
                                stack.Push(function.leftchild);
                                break;
                            case Cotg:
                                result.Add("cotan");
                                stack.Push(function.leftchild);
                                break;
                            case Arcsin:
                                result.Add("arcsin");
                                stack.Push(function.leftchild);
                                break;
                            case Arccos:
                                result.Add("arccos");
                                stack.Push(function.leftchild);
                                break;
                            case Arctg:
                                result.Add("arctan");
                                stack.Push(function.leftchild);
                                break;
                            case Arccotg:
                                result.Add("arccotan");
                                stack.Push(function.leftchild);
                                break;
                            case Abs:
                                result.Add("abs");
                                stack.Push(function.leftchild);
                                break;
                            case Log:
                                result.Add("ln");
                                stack.Push(function.leftchild);
                                break;
                        }
                        break;
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
        INode BuildTree()
        {
            pos++;

            if (double.TryParse(expr[pos], out double n)) //It is a number
            {
                Constant constant = new(n);
                return constant;
            }
            else if (expr[pos] == VAR) //It is a derivative variable
            {
                DiffVariable variable = new();
                return variable;
            }
            else //It is an operator or a function
            {
                switch (expr[pos])
                {
                    case "e":
                        Constant euler = new(Math.E);

                        return euler;
                    case "pi":
                        Constant pi = new(Math.PI);

                        return pi;
                    case "+":
                        Plus plus = new(BuildTree(), BuildTree());

                        return plus;
                    case "-":
                        Minus minus = new(BuildTree(), BuildTree());

                        return minus;
                    case "*":
                        Multi multi = new(BuildTree(), BuildTree());

                        return multi;
                    case "/":
                        Divi divi = new(BuildTree(), BuildTree());

                        return divi;
                    case "^":
                        Power power = new(BuildTree(), BuildTree());

                        return power;
                    case "sin":
                        Sin sin = new(BuildTree());

                        return sin;
                    case "cos":
                        Cos cos = new(BuildTree());

                        return cos;
                    case "tan":
                    case "tg":
                        Tg tg = new(BuildTree());

                        return tg;
                    case "cotan":
                    case "cotg":
                        Cotg cotg = new(BuildTree());

                        return cotg;
                    case "arcsin":
                        Arcsin arcsin = new(BuildTree());

                        return arcsin;
                    case "arccos":
                        Arccos arccos = new(BuildTree());

                        return arccos;
                    case "arctg":
                        Arctg arctg = new(BuildTree());

                        return arctg;
                    case "arccotg":
                        Arccotg arccotg = new(BuildTree());

                        return arccotg;
                    case "abs":
                        Abs abs = new(BuildTree());

                        return abs;
                    case "ln":
                        Log log = new(BuildTree());

                        return log;
                    case "sqrt":
                        Constant one = new(1), two = new(2);
                        Divi sqrtdivi = new(one, two);
                        Power power1 = new(BuildTree(), sqrtdivi);

                        return power1;
                    case "cbrt":
                        Constant one2 = new(1), three = new(3);
                        Divi cbrtdivi = new(one2, three);
                        Power power2 = new(BuildTree(), cbrtdivi);

                        return power2;
                    default:
                        if (expr[pos].Length == 1)
                        {
                            LetterConstant letter = new(expr[pos]);
                            return letter;
                        }
                        throw new Exception("UNKNOWN OPTOKEN");
                }
            }
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
    class StepGenerator
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
                case Abs:
                    GeneratedSteps.Add(DerivativeCalcFinal.Rules.abs);
                    GeneratedSteps.Add($"/math \\frac{{ {nodes[1]} }}{{ |{nodes[1]}| }} * [{nodes[1]}]'");
                    GenerateFurther(margin, color, 1);
                    GeneratedSteps.Add($"/math {nodes[2]}");
                    break;
            }
        }

        public List<string> StartGenerating()
        {
            GeneratedSteps = new List<string>();
            pos = -1;
            Generate(0, 0);

            return GeneratedSteps;
        }
    }
}