using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    // Inherit from empty base class GoalMB so we can have a list of all goal types.
    public class ConnectionGoalMB : GoalMB, IConnectionGoal {
        [SerializeField] ConnectionGoalMBData _connectionGoal;

        // IConnectionGoal
        public ILinkableItem From => ((IConnectionGoal)_connectionGoal).From;
        public ILinkableItem To => ((IConnectionGoal)_connectionGoal).To;
        // IGoal
        public override UnityEvent Completed => ((IGoal)_connectionGoal).Completed;
        public override bool IsCompleted => ((IGoal)_connectionGoal).IsCompleted;
        // IItem
        public override string Name => ((IItem)_connectionGoal).Name;
        public override string Description => ((IItem)_connectionGoal).Description;
        public override UnityEvent Added => ((IItem)_connectionGoal).Added;
    }

    /// <summary>
    /// Workaround Unitys inability to serialize object via interface reference.
    /// </summary>
    [System.Serializable]
    public class ConnectionGoalMBData : Goal, IConnectionGoal {
        [SerializeField, NotNull] LinkableItemMB _from;
        [SerializeField, NotNull] LinkableItemMB _to;

        public ILinkableItem From => _from;
        public ILinkableItem To => _to;
    }
}
