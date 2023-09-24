using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    public class FindItemGoalMB : GoalMB, IFindItemGoal {
        [SerializeField] FindItemGoalForMB _findItemGoal;

        // IFindItemGoal
        public IItem ItemToFind => ((IFindItemGoal)_findItemGoal).ItemToFind;
        // IGoal
        public override UnityEvent Completed => ((IGoal)_findItemGoal).Completed;
        public override bool IsCompleted => ((IGoal)_findItemGoal).IsCompleted;
        // IItem
        public override string Name => ((IItem)_findItemGoal).Name;
        public override string Description => ((IItem)_findItemGoal).Description;
        public override UnityEvent Added => ((IItem)_findItemGoal).Added;

    }

    /// <summary>
    /// Workaround Unitys inability to serialize object via interface reference.
    /// </summary>
    [System.Serializable]
    public class FindItemGoalForMB : Goal, IFindItemGoal {
        [SerializeField, NotNull] ItemMB _itemToFind;
        public IItem ItemToFind => _itemToFind;
    }
}
