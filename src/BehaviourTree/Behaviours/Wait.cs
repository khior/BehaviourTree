﻿using System;

namespace BehaviourTree.Behaviours
{
    public sealed class Wait<TContext> : BaseBehaviour<TContext> where TContext : IClock
    {
        public readonly long WaitTimeInMilliseconds;
        private long? _initialTimestamp;

        public Wait(int waitTimeInMilliseconds) : this("Wait", waitTimeInMilliseconds)
        {

        }

        public Wait(string name, int waitTimeInMilliseconds) : base(name)
        {
            WaitTimeInMilliseconds = waitTimeInMilliseconds;
        }

        protected override BehaviourStatus Update(TContext context)
        {
            var currentTimeStamp = context.GetTimeStampInMilliseconds();

            if (_initialTimestamp == null)
            {
                _initialTimestamp = currentTimeStamp;
            }

            var elapsedMilliseconds = currentTimeStamp - _initialTimestamp;

            if (elapsedMilliseconds >= WaitTimeInMilliseconds)
            {
                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        }

        protected override void OnTerminate(BehaviourStatus status)
        {
            _initialTimestamp = null;
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<Wait<TContext>> typedVisitor)
            {
                typedVisitor.Visit(this);
                return;
            }

            base.Accept(visitor);
        }
    }
}
