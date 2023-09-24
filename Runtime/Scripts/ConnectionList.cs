using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {

    public interface IConnection {
        ILinkableItem To { get; }
        /// <summary>
        /// Action when connection has been made.
        /// </summary>
        /// <value></value>
        UnityEvent Connected { get; }
    }

    public class Connection : IConnection {
        public ILinkableItem To { get; }
        [SerializeField] UnityEvent _connected = new();
        public UnityEvent Connected => _connected;

        public Connection(ILinkableItem to = null, System.Action action = null) {
            To = to;
            if (action != null) {
                _connected.AddListener(() => action.Invoke());
            }
        }
    }

    [System.Serializable]
    public class ConnectionList : IConnectionList {
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

        public IConnection ConnectionTo(ILinkableItem to) {
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