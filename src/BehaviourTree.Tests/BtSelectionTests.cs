﻿using BehaviourTree.Composites;
using BehaviourTree.Tests.Utils;
using NUnit.Framework;

namespace BehaviourTree.Tests
{
    [TestFixture]
    public sealed class BtSelectionTests
    {
        [TestFixture]
        public sealed class GivenChildrenReturnsSuccess
        {
            private IBtBehaviour _sut;
            private WatchCollectionMock _childrenWatcher;

            [SetUp]
            public void Setup()
            {
                _childrenWatcher = new WatchCollectionMock(
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Succeeded),
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Succeeded),
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Succeeded));

                _sut = new BtSelection(_childrenWatcher.Behaviours);
            }

            [Test]
            public void WhenRunningToEnd_ShouldReturnSuccess()
            {
                _sut.Tick(ElaspedTicks.From(0));
                _sut.Tick(ElaspedTicks.From(0));
                var behaviourStatus = _sut.Tick(ElaspedTicks.From(0));

                Assert.That(_sut.Status, Is.EqualTo(BehaviourStatus.Succeeded));
                Assert.That(behaviourStatus, Is.EqualTo(BehaviourStatus.Succeeded));
                Assert.That(_childrenWatcher.NbOfChildrenCalled, Is.EqualTo(1));
            }
        }

        [TestFixture]
        public sealed class GivenChildrenReturnsFailure
        {
            private IBtBehaviour _sut;
            private WatchCollectionMock _childrenWatcher;

            [SetUp]
            public void Setup()
            {
                _childrenWatcher = new WatchCollectionMock(
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Failed),
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Failed),
                    new MockBtBehaviour(BehaviourStatus.Ready, BehaviourStatus.Failed));

                _sut = new BtSelection(_childrenWatcher.Behaviours);
            }

            [Test]
            public void WhenRunningToEnd_ShouldReturnFailure()
            {
                _sut.Tick(ElaspedTicks.From(0));
                _sut.Tick(ElaspedTicks.From(0));
                var behaviourStatus = _sut.Tick(ElaspedTicks.From(0));

                Assert.That(_sut.Status, Is.EqualTo(BehaviourStatus.Failed));
                Assert.That(behaviourStatus, Is.EqualTo(BehaviourStatus.Failed));
                Assert.That(_childrenWatcher.AllChildrenCalled);
            }
        }
    }
}