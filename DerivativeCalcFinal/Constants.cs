using System;
using System.Collections.Generic;

namespace CSharpMath.Core
{
    sealed class DiffVariable : ConstNode
    {
        public override void SelfCheck()
        {
            return;
        }
        public override void Differentiate()
        {
            Constant one = new(1);

            GetParent().SwapChildren(this, one);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone() };
            Storage.Steps.Add(list);

            Constant one = new(1);
            GetParent().SwapChildren(this, one);
        }
        public override INode Clone()
        {
            DiffVariable variable = new();
            return variable;
        }

    }
    sealed class Constant : ConstNode
    {
        private double value;
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public Constant(double value)
        {
            Value = value;
        }
        public override void SelfCheck()
        {
            return;
        }
        public override void Differentiate()
        {
            Value = 0;
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone() };
            Storage.Steps.Add(list);

            Value = 0;
        }
        public override INode Clone()
        {
            Constant constant = new(Value);
            return constant;
        }
    }
    sealed class LetterConstant : ConstNode
    {
        string? letter;

        public string Letter
        {
            get
            {
                if (letter == null)
                    return "";
                return letter;
            }
            set { this.letter = value; }
        }
        public LetterConstant(string? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Letter = value;
        }
        public override void SelfCheck()
        {
            return;
        }
        public override void Differentiate()
        {
            Constant zero = new(0);

            GetParent().SwapChildren(this, zero);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone() };
            Storage.Steps.Add(list);

            Constant zero = new(0);

            GetParent().SwapChildren(this, zero);
        }
        public override INode Clone()
        {
            LetterConstant constant = new(Letter);
            return constant;
        }
    }
}
