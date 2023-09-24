namespace lotecsoftware.goals {
    public interface IConnectionGoal : IGoal {
        ILinkableItem From { get; }
        ILinkableItem To { get; }
    }

    public class ConnectionGoal : Goal, IConnectionGoal {
        public ILinkableItem From { get; }
        public ILinkableItem To { get; }

        public ConnectionGoal(string description, ILinkableItem from, ILinkableItem to) {
            _item = new(description: description);
            From = from;
            To = to;
        }
    }
}
