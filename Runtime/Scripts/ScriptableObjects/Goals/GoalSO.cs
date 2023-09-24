using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    public abstract class GoalSO : ScriptableObject, IGoal {
        public abstract UnityEvent Completed { get; }
        public abstract bool IsCompleted { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract UnityEvent Added { get; }
    }
}
