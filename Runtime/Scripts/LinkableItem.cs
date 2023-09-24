using System.Collections.Generic;
using lotecsoftware.items;
using UnityEngine;
using UnityEngine.Events;

namespace lotecsoftware.goals {
    public interface IConnectionList {
        /// <summary>
        /// Returns the connection if there is an available connection to 'to'.
        /// </summary>
        /// <param name="to"></param>
        /// <returns>Connection</returns>
        IConnection ConnectionTo(ILinkableItem to);
        /// <summary>
        /// List of all available connections this item has to other items.
        /// </summary>
        /// <value>List of connections</value>
        IEnumerable<IConnection> Connections { get; }
        void AddConnection(IConnection connection);
        int Count { get; }
    }

    public interface ILinkableItem : IItem, IConnectionList { }

    [System.Serializable]
    public class LinkableItem : ILinkableItem {
        [SerializeField] Item _item;
        [SerializeField] ConnectionList _connectionsList;

        // ILinkableItem
        // IItem
        public string Name => ((IItem)_item).Name;
        public string Description => ((IItem)_item).Description;
        public UnityEvent Added => ((IItem)_item).Added;
        // IConnectionList
        public IEnumerable<IConnection> Connections => ((IConnectionList)_connectionsList).Connections;
        public int Count => _connectionsList.Count;
        public IConnection ConnectionTo(ILinkableItem to) => ((IConnectionList)_connectionsList).ConnectionTo(to);
        public void AddConnection(IConnection connection) => ((IConnectionList)_connectionsList).AddConnection(connection);

        /// <summary>
        /// An item that has is linked to other items.
        /// </summary>
        /// <param name="name">Leaving name empty, and is linked from something, that connection will autoconnect</param>
        /// <param name="description"></param>
        /// <param name="connections"></param>
        public LinkableItem(string name = null, string description = null, List<IConnection> connections = null) {
            _item = new(name, description);
            _connectionsList = new(connections);
        }
    }
}
