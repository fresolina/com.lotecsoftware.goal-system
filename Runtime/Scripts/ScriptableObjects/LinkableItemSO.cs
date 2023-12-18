using System.Collections.Generic;
using Lotec.Utils.Attributes;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    [System.Serializable]
    public class ConnectionForSO : IConnection {
        [SerializeField] string _title;
        [SerializeField, NotNull] LinkableItemSO _to;
        [SerializeField] bool _autoConnect = false;
        [SerializeField] UnityEvent _connected;

        // Unity Workaround to get Array item title in inspector list, instead of "Element 0".
        public string Title { get { return _title; } internal set { _title = value; } }
        public ILinkableItem To => _to;
        public UnityEvent Connected => _connected;
        public bool AutoConnect => _autoConnect;
    }

    [System.Serializable]
    public class ConnectionListForSO : IConnectionList {
        [SerializeField] List<ConnectionForSO> _connections = new();
        ConnectionList _connectionList;

        public void Init() {
            _connectionList = new(_connections);
        }

        // IConnectionList
        public IEnumerable<IConnection> Connections => ((IConnectionList)_connectionList).Connections;
        public int Count => ((IConnectionList)_connectionList).Count;
        public void AddConnection(IConnection connection) => ((IConnectionList)_connectionList).AddConnection(connection);
        public IConnection ConnectionTo(ILinkableItem to) => ((IConnectionList)_connectionList).ConnectionTo(to);
        internal List<ConnectionForSO> InternalConnectionsList => _connections;
    }

    [CreateAssetMenu(fileName = "LinkableItem", menuName = "Items/LinkableItem")]
    public class LinkableItemSO : ItemSO, ILinkableItem {
        [SerializeField] ConnectionListForSO _connectionsList = new();

        public IEnumerable<IConnection> Connections => _connectionsList.Connections;
        public int Count => _connectionsList.Count;
        public void AddConnection(IConnection connection) => _connectionsList.AddConnection(connection);
        public IConnection ConnectionTo(ILinkableItem to) => _connectionsList.ConnectionTo(to);

        void Awake() {
            _connectionsList.Init();
        }

        void OnValidate() {
            // Workaround Unity not using ToString() for Array title in inspector
            foreach (ConnectionForSO item in _connectionsList.InternalConnectionsList) {
                if (item.To is ScriptableObject so) {
                    item.Title = so.name;
                }
            }
        }
    }
}
