using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    [CreateAssetMenu(fileName = "ConnectionGoal", menuName = "Goals/ConnectionGoal")]
    public class ConnectionGoalSO : GoalSO, IConnectionGoal {
        [SerializeField] ConnectionGoalSOData _connectionGoal;

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
    public class ConnectionGoalSOData : Goal, IConnectionGoal {
        [SerializeField, NotNull] LinkableItemSO _from;
        [SerializeField, NotNull] LinkableItemSO _to;

        public ILinkableItem From => _from;
        public ILinkableItem To => _to;
    }
}
