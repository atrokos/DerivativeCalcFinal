using System;
using System.Collections.Generic;

namespace CSharpMathCore
{
    public abstract class Function : ActionNode
    {
        public Function(INode leftchild) : base(leftchild)
        {

        }
        public override void SelfCheck()
        {
            return;
        }
    }
    class Sin : Function
    {
        public Sin(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Cos cos = new(leftchild.Clone());
            Multi multi = new(leftchild.Clone(), cos);
            multi.leftchild.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Cos cos = new(leftchild.Clone());
            Multi multi = new(leftchild.Clone(), cos);
            multi.leftchild.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            if (leftchild == null)
                throw new NullReferenceException();

            Sin sin = new(leftchild.Clone());
            return sin;
        }
    }
    class Cos : Function
    {
        public Cos(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant constant = new(-1);

            Sin sin = new(leftchild.Clone());
            INode left = leftchild.Clone();

            Multi multi2 = new(left, sin);
            left.Differentiate();
            left.SelfCheck();

            Multi multi1 = new(constant, multi2);

            GetParent().SwapChildren(this, multi1);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant constant = new(-1);

            Sin sin = new(leftchild.Clone());
            INode left = leftchild.Clone();

            Multi multi2 = new(left, sin);
            left.DifferentiateSteps();
            left.SelfCheck();

            Multi multi1 = new(constant, multi2);

            GetParent().SwapChildren(this, multi1);
            list.Add(multi1);
        }
        public override INode Clone()
        {
            if (leftchild == null)
                throw new NullReferenceException();

            Cos cos = new(leftchild.Clone());
            return cos;
        }
    }
    class Tg : Function
    {
        public Tg(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant one = new(1);
            Constant two = new(2);
            Cos cos = new(leftchild.Clone());

            Power power = new(cos, two);
            Divi divi = new(one, power);

            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant one = new(1);
            Constant two = new(2);
            Cos cos = new(leftchild.Clone());

            Power power = new(cos, two);
            Divi divi = new(one, power);

            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            if (leftchild == null)
                    throw new NullReferenceException();

            Tg tg = new(leftchild.Clone());

            return tg;
        }
    }
    class Cotg : Function
    {
        public Cotg(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant minus = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Sin sin = new(leftchild.Clone());

            Power power = new(sin, two);
            Divi divi = new(one, power);

            Multi multi2 = new(divi, leftchild.Clone());
            multi2.rightchild.Differentiate();

            Multi multi1 = new(minus, multi2);

            GetParent().SwapChildren(this, multi1);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant minus = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Sin sin = new(leftchild.Clone());

            Power power = new(sin, two);
            Divi divi = new(one, power);

            Multi multi2 = new(divi, leftchild.Clone());
            multi2.rightchild.DifferentiateSteps();

            Multi multi1 = new(minus, multi2);

            GetParent().SwapChildren(this, multi1);
            list.Add(multi1);
        }
        public override INode Clone()
        {
            Cotg cotg = new(leftchild.Clone());

            return cotg;
        }
    }
    class Arcsin : Function
    {
        public Arcsin(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant one = new(1);
            Constant two = new(2);

            Power power2 = new(leftchild.Clone(), two.Clone());
            Minus minus = new(one.Clone(), power2);

            Divi divi2 = new(one.Clone(), two.Clone());
            Power power1 = new(minus, divi2);

            Divi divi1 = new(one.Clone(), power1);
            Multi multi = new(divi1, leftchild.Clone());
            multi.rightchild.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant one = new(1);
            Constant two = new(2);

            Power power2 = new(leftchild.Clone(), two.Clone());
            Minus minus = new(one.Clone(), power2);

            Divi divi2 = new(one.Clone(), two.Clone());
            Power power1 = new(minus, divi2);

            Divi divi1 = new(one.Clone(), power1);
            Multi multi = new(divi1, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            Arcsin arcsin = new(leftchild.Clone());
            return arcsin;
        }
    }
    class Arccos : Function
    {
        public Arccos(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant minusone = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Power power2 = new(leftchild.Clone(), two.Clone());
            Minus minus = new(one.Clone(), power2);

            Divi divi2 = new(one.Clone(), two.Clone());
            Power power1 = new(minus, divi2);

            Divi divi1 = new(one.Clone(), power1);
            Multi multi = new(divi1, leftchild.Clone());
            multi.rightchild.Differentiate();

            Multi multi1 = new(minusone, multi);

            GetParent().SwapChildren(this, multi1);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant minusone = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Power power2 = new(leftchild.Clone(), two.Clone());
            Minus minus = new(one.Clone(), power2);

            Divi divi2 = new(one.Clone(), two.Clone());
            Power power1 = new(minus, divi2);

            Divi divi1 = new(one.Clone(), power1);
            Multi multi = new(divi1, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            Multi multi1 = new(minusone, multi);

            GetParent().SwapChildren(this, multi);
            list.Add(multi1);
        }
        public override INode Clone()
        {
            if (leftchild == null)
                throw new NullReferenceException();
            Arccos arccos = new(leftchild.Clone());
            return arccos;
        }
    }
    class Arctg : Function
    {
        public Arctg(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant one = new(1);
            Constant two = new(2);

            Power power = new(leftchild.Clone(), two);
            Plus plus = new(one.Clone(), power);

            Divi divi = new(one.Clone(), plus);
            
            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant one = new(1);
            Constant two = new(2);

            Power power = new(leftchild.Clone(), two);
            Plus plus = new(one.Clone(), power);

            Divi divi = new(one.Clone(), plus);

            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            Arctg arctg = new(leftchild.Clone());
            return arctg;
        }
    }
    class Arccotg : Function
    {
        public Arccotg(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Constant minusone = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Power power = new(leftchild.Clone(), two);
            Plus plus = new(one.Clone(), power);

            Divi divi = new(one.Clone(), plus);

            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.Differentiate();

            Multi multi1 = new(minusone, multi);
            GetParent().SwapChildren(this, multi1);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant minusone = new(-1);
            Constant one = new(1);
            Constant two = new(2);

            Power power = new(leftchild.Clone(), two);
            Plus plus = new(one.Clone(), power);

            Divi divi = new(one.Clone(), plus);

            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            Multi multi1 = new(minusone, multi);
            GetParent().SwapChildren(this, multi1);
            list.Add(multi1);
        }
        public override INode Clone()
        {
            Arccotg arccotg = new(leftchild.Clone());
            return arccotg;
        }
    }
    class Log : Function // Only the natural log
    {
        public Log(INode leftchild) : base(leftchild)
        {

        }
        public override void SelfCheck()
        {
            if (leftchild is Constant e && e.Value == Math.E)
            {
                Constant constant = new(1);

                SwapChildren(this, constant);
            }
        }
        public override void Differentiate()
        {
            Constant Dleft = new(1);
            INode Dright = leftchild.Clone();
            Divi divi = new(Dleft, Dright);

            INode Lright = leftchild.Clone();
            Multi multi = new(divi, Lright);
            Lright.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Constant Dleft = new(1);
            INode Dright = leftchild.Clone();
            Divi divi = new(Dleft, Dright);

            INode Lright = leftchild.Clone();
            Multi multi = new(divi, Lright);
            Lright.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            Log newlog = new(leftchild.Clone());
            return newlog;
        }
    }
    class Abs : Function
    {
        public Abs(INode leftchild) : base(leftchild)
        {

        }
        public override void Differentiate()
        {
            Abs abs = new(leftchild.Clone());
            Divi divi = new(leftchild.Clone(), abs);
            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.Differentiate();

            GetParent().SwapChildren(this, multi);
        }
        public override void DifferentiateSteps()
        {
            List<INode> list = new() { this.Clone(), leftchild.Clone() };
            Storage.Steps.Add(list);

            Abs abs = new(leftchild.Clone());
            Divi divi = new(leftchild.Clone(), abs);
            Multi multi = new(divi, leftchild.Clone());
            multi.rightchild.DifferentiateSteps();

            GetParent().SwapChildren(this, multi);
            list.Add(multi);
        }
        public override INode Clone()
        {
            Abs abs = new(leftchild.Clone());

            return abs;
        }
    }
}
