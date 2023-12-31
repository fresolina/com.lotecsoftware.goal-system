using System.Collections.Generic;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    /// <summary>
    /// Handles available, todo, done goals.
    /// Requires an Inventory for checking if user has collected an Item.
    /// </summary>
    [System.Serializable]
    public class GoalController : IGoalApi {
        [SerializeField] UnityEvent<ILinkable, ILinkable> _connectFailed = new();
        [SerializeField] UnityEvent<IGoal> _goalCompleted = new();

        readonly List<IGoal> _goalsTodo = new();
        /// Temporary var for UpdateCompletion()
        readonly List<IGoal> _goalsMarkedDone = new();
        readonly LinkableItemController _linkableItemController;
        public List<IGoal> Goals { get; } = new();

        public int CompletedGoalsCount { get; private set; } = 0;
        public UnityEvent<ILinkable, ILinkable> ConnectFailed => _connectFailed;
        public UnityEvent<IGoal> GoalCompleted => _goalCompleted;
        public bool AutoConnect { get => _linkableItemController.AutoConnect; set => _linkableItemController.AutoConnect = value; }

        public GoalController(ItemController itemController = null) {
            _linkableItemController = new LinkableItemController(itemController);
            _linkableItemController.ItemController.Inventory.Added += (IItem item) => {
                UpdateCompletion();
            };
        }

        // IGoalApi
        public void AddItem(IItem item) => _linkableItemController.AddItem(item);
        public bool HasItem(IItem item) => _linkableItemController.HasItem(item);
        public void AddGoal(IGoal goal) {
            if (Goals.Contains(goal))
                return;
            Goals.Add(goal);
            _goalsTodo.Add(goal);
        }

        public void Connect(ILinkable a, ILinkable b) {
            if (!_linkableItemController.Connect(a, b)) {
                ConnectFailed?.Invoke(a, b);
                return;
            }
            UpdateCompletion();
        }

        public void UpdateCompletion() {
            _goalsMarkedDone.Clear();
            for (int i = 0; i < _goalsTodo.Count; i++) {
                IGoal goal = _goalsTodo[i];

                if (_linkableItemController.IsCompleted(goal)) {
                    _goalsMarkedDone.Add(goal);
                }
            }

            for (int i = 0; i < _goalsMarkedDone.Count; i++) {
                IGoal goal = _goalsMarkedDone[i];
                CompletedGoalsCount++;
                _goalsTodo.Remove(goal);
                GoalCompleted?.Invoke(goal);
                goal.Completed?.Invoke();
            }
        }

        // Misc

        public int CountLinks() {
            return _linkableItemController.CountLinks();
        }
    }
}
