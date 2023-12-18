namespace lotecsoftware.goals {
    public interface IConnectionGoal : IGoal {
        ILinkable From { get; }
        ILinkable To { get; }
    }

    public class ConnectionGoal : Goal, IConnectionGoal {
        public ILinkable From { get; }
        public ILinkable To { get; }

        public ConnectionGoal(string description, ILinkable from, ILinkable to) {
            _item = new(description: description);
            From = from;
            To = to;
        }
    }
}
