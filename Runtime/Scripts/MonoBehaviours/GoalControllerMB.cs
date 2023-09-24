using lotecsoftware.items;
using UnityEngine;

namespace lotecsoftware.goals {
    public class GoalControllerMB : MonoBehaviour, IGoalApi, IItemApi {
        [SerializeField] GoalMB[] _goals;
        [SerializeField] GoalController _goalController = new();

        public GoalController GoalController => _goalController;

        public void AddGoal(IGoal goal) => ((IGoalApi)GoalController).AddGoal(goal);
        public void AddItem(IItem item) => ((IItemApi)GoalController).AddItem(item);
        public bool HasItem(IItem item) => ((IItemApi)GoalController).HasItem(item);
        // Workaround Unity hates interfaces. Make them show in inspector
        public void AddGoal(GoalMB goal) => ((IGoalApi)GoalController).AddGoal(goal);
        public void AddItem(ItemMB item) => ((IItemApi)GoalController).AddItem(item);
        public bool HasItem(ItemMB item) => ((IItemApi)GoalController).HasItem(item);

        void Awake() {
            if (_goals == null)
                return;

            for (int i = 0; i < _goals.Length; i++) {
                GoalController.AddGoal(_goals[i]);
            }
        }
    }
}
