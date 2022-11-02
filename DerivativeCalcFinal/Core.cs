using System;
using System.Collections.Generic;

namespace CSharpMath.Core
{
    public static class Storage
    {
        public static List<List<INode>> Steps = new();
    }
    interface IParent
    {
        void SwapChildren(INode oldchild, INode newchild); // child -> P  newchild   ==> child  P <- newchild
    }
    public interface INode
    {
        void SetParent(ActionNode? node);
        void SelfCheck();
        void Differentiate();
        void DifferentiateSteps();
        INode Clone();
    }

    public abstract class ActionNode : INode, IParent
    {
        public INode leftchild;
        private ActionNode? parent = null;

        public ActionNode(INode leftchild)
        {
            this.leftchild = leftchild;
            this.leftchild.SetParent(this);
        }
        abstract public void SelfCheck();
        abstract public void Differentiate();
        abstract public void DifferentiateSteps();
        abstract public INode Clone();

        public ActionNode GetParent()
        {
            if (parent == null)
                throw new ArgumentNullException($"{this} was requested to return its parent, but it doesn't have one.");

            return parent;
        }
        public void SetParent(ActionNode? node)
        {
            parent = node;
        }
        public virtual void SwapChildren(INode oldchild, INode newchild)
        {
            leftchild = newchild;
            leftchild.SetParent(this);
        }
    }
    class Head : ActionNode
    {
        public Head(INode leftchild) : base(leftchild)
        {

        }
        public override void SelfCheck()
        {
            if (leftchild != null)
                leftchild.SelfCheck();
            else
                throw new NullReferenceException();
        }
        public override void Differentiate()
        {
            if (leftchild != null)
                leftchild.Differentiate();
            else
                throw new NullReferenceException();
        }
        public override void DifferentiateSteps()
        {
            if (leftchild != null)
                leftchild.DifferentiateSteps();
            else
                throw new NullReferenceException();
        }
        public override INode Clone()
        {
            Head head = new(leftchild.Clone());
            
            return head;
        }
    }
    public abstract class ConstNode : INode
    {
        public ActionNode? parent;

        public ActionNode GetParent()
        {
            if (parent == null)
                throw new NullReferenceException();
            return parent;
        }
        public void SetParent(ActionNode? node)
        {
            parent = node;
        }
        abstract public void SelfCheck();
        abstract public void Differentiate();
        abstract public void DifferentiateSteps();
        abstract public INode Clone();
    }
}
