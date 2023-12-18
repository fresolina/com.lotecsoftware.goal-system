using System.Collections.Generic;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {

    public interface IConnection {
        ILinkable To { get; }
        bool AutoConnect { get; }
        /// <summary>
        /// Action when connection has been made.
        /// </summary>
        /// <value></value>
        UnityEvent Connected { get; }
    }

    public class Connection : IConnection {
        public ILinkable To { get; }
        [SerializeField] bool _autoConnect = false;
        [SerializeField] UnityEvent _connected = new();

        public UnityEvent Connected => _connected;
        public bool AutoConnect => _autoConnect;

        public Connection() : this(null, null) { }
        public Connection(ILinkable to = null, System.Action action = null) {
            To = to;
            if (action != null) {
                _connected.AddListener(() => action.Invoke());
            }

            _autoConnect = string.IsNullOrEmpty(((IItem)To)?.Name);
        }
    }

    [System.Serializable]
    public class ConnectionList : ILinkable {
        readonly List<IConnection> _connections;
        public IEnumerable<IConnection> Connections => _connections;
        public int Count => _connections.Count;

        public ConnectionList(IEnumerable<IConnection> list = null) {
            if (list == null) {
                _connections = new();
            } else {
                _connections = new(list);
            }
        }

        public IConnection ConnectionTo(ILinkable to) {
            if (_connections == null)
                return null;

            for (int i = 0; i < _connections.Count; i++) {
                if (_connections[i].To == to) {
                    return _connections[i];
                }
            }
            return null;
        }

        public void AddConnection(IConnection connection) {
            _connections.Add(connection);
        }
    }
}
