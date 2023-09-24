using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace lotecsoftware.goals.tests {
    public class TestGoalSystem {
        protected GoalController _goalController;
        protected ILinkableItem _itemA;
        protected ILinkableItem _itemExtra;
        protected ILinkableItem _itemFromEvent;
        protected ILinkableItem _itemB;
        protected ILinkableItem _itemABonus;
        protected IGoal _goalFindA;
        protected IGoal _goalFindA2;
        protected IGoal _goalLinkAToABonus;
        protected IConnectionGoal _goalLinkBToA;
        protected bool _goalCompleted;

        [UnitySetUp]
        public virtual IEnumerator SetupAssets() {
            _goalCompleted = false;
            _goalController = new GoalController();
            _itemA = new LinkableItem("Evidence A");
            _itemExtra = new LinkableItem("Evidence A2");
            _itemFromEvent = new LinkableItem("Item from event");
            _itemABonus = new LinkableItem(description: "Bonus item for A, autoconnects");
            _itemB = new LinkableItem("Evidence B");
            _goalFindA = new FindItemGoal("Find Evidence A", _itemA);
            _goalFindA2 = new FindItemGoal("Find Evidence A2", _itemA);
            _goalLinkAToABonus = new ConnectionGoal("Connect A to ABonus", _itemA, _itemABonus);
            _goalLinkBToA = new ConnectionGoal("Connect B to A", _itemB, _itemA);

            _goalController.ConnectFailed.AddListener((a, b) => Debug.Log("I have no evidence supporting that claim"));
            _goalController.GoalCompleted.AddListener((goal) => _goalCompleted = true);
            _goalFindA2.Completed.AddListener(() => _goalController.AddItem(_itemExtra));
            _itemA.AddConnection(new Connection(to: _itemABonus));
            _itemB.AddConnection(new Connection(to: _itemA));
            foreach (IConnection item in _itemA.Connections) {
                item.Connected.AddListener(() => _goalController.AddItem(_itemFromEvent));
            }

            yield return null;
        }

        [Test]
        public void AddItem_CompletesFindItemGoal() {
            _goalController.AddGoal(_goalFindA);

            int count = _goalController.CompletedGoalsCount;
            _goalController.AddItem(_itemA);
            Assert.IsTrue(_goalCompleted, "Goal completed trigger activated");
            Assert.AreEqual(count + 1, _goalController.CompletedGoalsCount, "A goal was completed");
        }

        [Test]
        public void AddItem_CompletesAutoLinkGoal() {
            _goalController.AddGoal(_goalLinkAToABonus);

            int count = _goalController.CompletedGoalsCount;
            _goalController.AddItem(_itemA);
            _goalController.AddItem(_itemABonus);

            // Assert.IsTrue(_goalLinkAToABonus.IsCompleted, "Goal completed");
            Assert.IsTrue(_goalCompleted, "Goal completed trigger activated");
            Assert.AreEqual(count + 1, _goalController.CompletedGoalsCount, "A goal was completed");
        }

        [Test]
        public void Connect_CompletesLinkGoal() {
            _goalController.AddGoal(_goalLinkBToA);

            int count = _goalController.CompletedGoalsCount;
            _goalController.AddItem(_itemA);
            _goalController.AddItem(_itemB);
            Assert.IsFalse(_goalLinkBToA.IsCompleted, "Goal not completed");

            _goalController.Connect(_itemB, _itemA);
            Assert.IsTrue(_goalLinkBToA.IsCompleted, "Goal completed trigger activated");
            Assert.AreEqual(count + 1, _goalController.CompletedGoalsCount, "A goal was completed");
        }

        [Test]
        public void Connect_TriggersItemConnectionConnected() {
            Assert.IsFalse(_goalController.HasItem(_itemFromEvent), "Connected action not run");
            _goalController.AddItem(_itemA);
            Assert.IsTrue(_goalController.HasItem(_itemFromEvent), "Connected action not run");
        }

        [Test]
        public void GoalCompleted_AddsItem() {
            _goalController.AddGoal(_goalFindA2);

            _goalController.AddItem(_itemA);
            Assert.IsTrue(_goalFindA2.IsCompleted, "Goal completed");
            Assert.IsTrue(_goalController.HasItem(_itemExtra), "Goal completed trigger on specific goal activated");
        }
    }
}
