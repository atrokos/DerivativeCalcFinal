using System;
using System.Collections.Generic;

namespace CSharpMathCore
{
    abstract class OPNode : ActionNode
    {
        public INode rightchild;

        public OPNode(INode leftchild, INode rightchild) : base(leftchild)
        {
            this.rightchild = rightchild;
            this.rightchild.SetParent(this);
        }

        public override void SwapChildren(INode oldchild, INode newchild)
        {
            if (leftchild == oldchild)
            {
                leftchild = newchild;
                leftchild.SetParent(this);
            }
            else if (rightchild == oldchild)
            {
                rightchild = newchild;
                rightchild.SetParent(this);
            }
        }
    }

    sealed class Plus : OPNode
    {
        public Plus(INode leftchild, INode rightchild) : base(leftchild, rightchild)
        {

        }
        public override void SelfCheck()
        {
            if (leftchild is Constant left && rightchild is Constant right) //All constants
            {
                Constant sum = new(left.Value + right.Value);

                SwapChildren(this, sum);
            }
            else if (leftchild is Constant left1 && left1.Value == 0) //Left is 0
            {
                SwapChildren(this, rightchild);
            }
            else if (rightchild is Constant right1 && right1.Value == 0) //Right is 0
            {
                SwapChildren(this, leftchild);
            }
        }
        public override void Differentiate()
        {
            leftchild.Differentiate();
            leftchild.SelfCheck();
            rightchild.Differentiate();
            rightchild.SelfCheck();
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone(), rightchild.Clone() };
            Storage.Steps.Add(list);
            leftchild.DifferentiateSteps();
            rightchild.DifferentiateSteps();
            list.Add(leftchild.Clone());
            list.Add(rightchild.Clone());
        }
        public override INode Clone()
        {
            /// <summary> This method returns a clone of the called node with a null parent. </summary>
            Plus plus = new(leftchild.Clone(), rightchild.Clone());

            return plus;
        }
    }
    sealed class Minus : OPNode
    {
        public Minus(INode leftchild, INode rightchild) : base(leftchild, rightchild)
        {

        }
        public override void SelfCheck()
        {
            if (leftchild is Constant left && rightchild is Constant right) //All constants
            {
                Constant sum = new(left.Value - right.Value);

                SwapChildren(this, sum);
            }
            else if (leftchild is Constant left1 && left1.Value == 0) //Left is 0, so it multiplies by -1
            {
                Constant constant = new(-1);
                Multi multi = new(constant, rightchild);

                SwapChildren(this, multi);
            }
            else if (rightchild is Constant right1 && right1.Value == 0) //Right is 0
            {
                SwapChildren(this, leftchild);
            }
        }
        public override void Differentiate()
        {
            leftchild.Differentiate();
            leftchild.SelfCheck();
            rightchild.Differentiate();
            rightchild.SelfCheck();
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone(), rightchild.Clone() };
            Storage.Steps.Add(list);
            leftchild.DifferentiateSteps();
            rightchild.DifferentiateSteps();
            list.Add(leftchild.Clone());
            list.Add(rightchild.Clone());
        }
        public override INode Clone()
        {
            /// <summary> This method returns a clone of the called node with a null parent. </summary>
            Minus minus = new(leftchild.Clone(), rightchild.Clone());
            return minus;
        }
    }
    sealed class Multi : OPNode
    {
        public Multi(INode leftchild, INode rightchild) : base(leftchild, rightchild)
        {

        }

        public override void SelfCheck()
        {
            //!!! ONLY for 1, because -1 wouldn't make a difference
        }
        public override void Differentiate()
        {
            Multi first = new(leftchild.Clone(), rightchild.Clone());
            first.leftchild.Differentiate();

            Multi second = new(leftchild.Clone(), rightchild.Clone());
            second.rightchild.Differentiate();

            Plus plus = new(first, second);

            GetParent().SwapChildren(this, plus);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone(), rightchild.Clone() };
            Storage.Steps.Add(list);

            INode left1 = leftchild.Clone();
            INode right1 = rightchild.Clone();

            Multi first = new(left1, right1);
            first.leftchild.DifferentiateSteps();

            Multi second = new(leftchild, rightchild);
            second.rightchild.DifferentiateSteps();

            Plus plus = new(first, second);

            GetParent().SwapChildren(this, plus);

            list.Add(plus);
        }
        public override INode Clone()
        {
            /// <summary> This method returns a clone of the called node with a null parent. </summary>
            Multi multi = new(leftchild.Clone(), rightchild.Clone());
            return multi;
        }
    }
    sealed class Divi : OPNode
    {
        public Divi(INode leftchild, INode rightchild) : base(leftchild, rightchild)
        {

        }
        public override void SelfCheck()
        {

        }
        public override void Differentiate()
        {
            INode PowerG = rightchild.Clone();
            Constant two = new(2);
            Power power = new(PowerG, two);

            Multi multi1 = new(leftchild.Clone(), rightchild.Clone());
            multi1.leftchild.Differentiate();

            Multi multi2 = new(leftchild.Clone(), rightchild.Clone());
            multi1.rightchild.Differentiate();
            Minus minus = new(multi1, multi2);

            Divi newdivi = new(minus, power);

            GetParent().SwapChildren(this, newdivi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone(), rightchild.Clone() };
            Storage.Steps.Add(list);

            INode PowerG = rightchild.Clone();
            Constant two = new(2);
            Power power = new(PowerG, two);

            Multi multi1 = new(leftchild.Clone(), rightchild.Clone());
            multi1.leftchild.DifferentiateSteps();

            Multi multi2 = new(leftchild.Clone(), rightchild.Clone());
            multi1.rightchild.DifferentiateSteps();
            Minus minus = new(multi1, multi2);

            Divi newdivi = new(minus, power);

            GetParent().SwapChildren(this, newdivi);
            list.Add(newdivi.Clone());
        }
        public override INode Clone()
        {
            /// <summary> This method returns a clone of the called node with a null parent. </summary>
            Divi divi = new(leftchild.Clone(), rightchild.Clone());
            return divi;
        }
    }
    sealed class Power : OPNode
    {
        public Power(INode leftchild, INode rightchild) : base(leftchild, rightchild)
        {

        }
        public override void SelfCheck()
        {

        }
        public override void Differentiate()// [f^g]' = f^g * [ln(f)*g]'
        {
            INode power = this.Clone();

            Log log = new(leftchild.Clone());
            INode right = rightchild.Clone();

            Multi multi2 = new(log, right);
            Multi multi1 = new(power, multi2);

            multi2.Differentiate();

            GetParent().SwapChildren(this, multi1);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone(), rightchild.Clone() };
            Storage.Steps.Add(list);

            INode power = this.Clone();

            Log log = new(leftchild.Clone());
            INode right = rightchild.Clone();

            Multi multi2 = new(log, right);
            Multi multi1 = new(power, multi2);

            multi2.DifferentiateSteps();

            GetParent().SwapChildren(this, multi1);

            list.Add(multi1.Clone());
        }
        public override INode Clone()
        {
            /// <summary> This method returns a clone of the called node with a null parent. </summary>
            Power power = new(leftchild.Clone(), rightchild.Clone());
            return power;
        }
    }
}
