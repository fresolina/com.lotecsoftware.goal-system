using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    public interface IGoal : IItem {
        bool IsCompleted { get; }
        /// <summary>
        /// Action when goal was completed.
        /// </summary>
        /// <value></value>
        UnityEvent Completed { get; }
    }

    /// <summary>
    /// Base data for a goal. Should be polymorphed into a usable goal.
    /// </summary>
    [System.Serializable]
    public class Goal : IGoal {
        [SerializeField] protected Item _item;
        [SerializeField] protected UnityEvent _completed = new();
        public bool IsCompleted { get; private set; } = false;

        public Goal() {
            _completed.AddListener(() => IsCompleted = true);
        }

        // IItem
        public string Name => ((IItem)_item).Name;
        public string Description => ((IItem)_item).Description;
        public UnityEvent Added => ((IItem)_item).Added;

        // IGoal
        public UnityEvent Completed => _completed;
    }
}
