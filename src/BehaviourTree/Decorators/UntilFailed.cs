﻿namespace BehaviourTree.Decorators
{
    public sealed class UntilFailed<TContext> : DecoratorBehaviour<TContext>
    {
        public UntilFailed(IBehaviour<TContext> child) : this("UntilFailed", child)
        {
        }

        public UntilFailed(string name, IBehaviour<TContext> child) : base(name, child)
        {
        }

        protected override BehaviourStatus Update(TContext context)
        {
            var childStatus = Child.Tick(context);

            if (childStatus == BehaviourStatus.Failed)
            {
                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<UntilFailed<TContext>> typedVisitor)
            {
                typedVisitor.Visit(this);
                return;
            }

            base.Accept(visitor);
        }
    }
}
