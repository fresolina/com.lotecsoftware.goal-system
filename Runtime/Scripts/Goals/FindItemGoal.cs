using lotecsoftware.items;

namespace lotecsoftware.goals {
    public interface IFindItemGoal : IGoal {
        IItem ItemToFind { get; }
    }

    [System.Serializable]
    public class FindItemGoal : Goal, IFindItemGoal {
        public IItem ItemToFind { get; }

        public FindItemGoal(string description = null, IItem item = null) {
            _item = new(description: description);
            ItemToFind = item;
        }
    }
}
