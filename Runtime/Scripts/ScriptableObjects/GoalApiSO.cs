using lotecsoftware.items;
using UnityEngine;

namespace lotecsoftware.goals {
    public interface IGoalApi : IItemApi {
        public void AddGoal(IGoal goal);
    }

    /// <summary>
    /// API usable from UnityEvents must have void returns.
    /// </summary>
    // [CreateAssetMenu(fileName = "GoalApi", menuName = "Goals/GoalApi", order = 0)]
    public class GoalApiSO : ScriptableObject, IGoalApi {
        IGoalApi _handler;

        public void Init(IGoalApi handler) {
            _handler = handler;
        }

        // IGoalApi
        public void AddGoal(IGoal goal) => _handler.AddGoal(goal);
        // IItemApi
        public void AddItem(IItem item) => _handler.AddItem(item);
        public bool HasItem(IItem item) => _handler.HasItem(item);
        // Workaround Unity hates interfaces (make it selectable in Inspector)
        public void AddGoal(GoalSO goal) => _handler.AddGoal(goal);
        public void AddItem(ItemSO item) => _handler.AddItem(item);
    }
}
