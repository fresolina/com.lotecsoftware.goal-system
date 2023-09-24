using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    [CreateAssetMenu(fileName = "FindItemGoal", menuName = "Goals/FindItemGoal")]
    public class FindItemGoalSO : GoalSO, IFindItemGoal {
        [SerializeField] FindItemGoalSOData _goal;

        // IFindItemGoal
        public IItem ItemToFind => ((IFindItemGoal)_goal).ItemToFind;
        // IGoal
        public override UnityEvent Completed => ((IGoal)_goal).Completed;
        public override bool IsCompleted => ((IGoal)_goal).IsCompleted;
        // IItem
        public override string Name => ((IItem)_goal).Name;
        public override string Description => ((IItem)_goal).Description;
        public override UnityEvent Added => ((IItem)_goal).Added;
    }

    /// <summary>
    /// Workaround Unitys inability to serialize object via interface reference.
    /// </summary>
    [System.Serializable]
    public class FindItemGoalSOData : Goal, IFindItemGoal {
        [SerializeField, NotNull] ItemSO _itemToFind;
        public IItem ItemToFind => _itemToFind;
    }
}
