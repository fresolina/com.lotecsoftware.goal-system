using System.Collections.Generic;
using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    [System.Serializable]
    public class ConnectionForMB : IConnection {
        [SerializeField] string _title;
        [SerializeField, NotNull] LinkableItemMB _to;
        [SerializeField] bool _autoConnect = false;
        [SerializeField] UnityEvent _connected;

        // Unity Workaround to get Array item title in inspector list, instead of "Element 0".
        public string Title { get { return _title; } internal set { _title = value; } }
        public ILinkable To => _to;
        public UnityEvent Connected => _connected;
        public bool AutoConnect => _autoConnect;
    }

    // Wrap ConnectionList in Unity serializable objects.
    [System.Serializable]
    public class ConnectionListForMB : IConnectionList {
        [SerializeField] List<ConnectionForMB> _connections;
        ConnectionList _connectionList;

        public void Init() {
            _connectionList = new(_connections);
        }

        // IConnectionList
        public IEnumerable<IConnection> Connections => ((IConnectionList)_connectionList).Connections;
        public int Count => ((IConnectionList)_connectionList).Count;
        public void AddConnection(IConnection connection) => ((IConnectionList)_connectionList).AddConnection(connection);
        public IConnection ConnectionTo(ILinkable to) => ((IConnectionList)_connectionList).ConnectionTo(to);
        internal List<ConnectionForMB> InternalConnectionsList => _connections;
    }

    public class LinkableItemMB : ItemMB, ILinkable {
        [SerializeField] ConnectionListForMB _connectionsList;

        // ILinkableItem
        // IConnectionList
        public IEnumerable<IConnection> Connections => _connectionsList.Connections;
        public int Count => _connectionsList.Count;
        public void AddConnection(IConnection connection) => _connectionsList.AddConnection(connection);
        public IConnection ConnectionTo(ILinkable to) => _connectionsList.ConnectionTo(to);

        void Awake() {
            _connectionsList.Init();
        }

        void OnValidate() {
            if (_connectionsList == null)
                return;

            // Workaround Unity not using ToString() for Array title in inspector
            foreach (ConnectionForMB item in _connectionsList.InternalConnectionsList) {
                if (item.To is Object obj) {
                    item.Title = obj.name;
                }
            }
        }
    }
}
