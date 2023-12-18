using System.Collections.Generic;
using lotecsoftware.items;
using UnityEngine;

namespace lotecsoftware.goals {
    /// <summary>
    /// Handles ConnectionGoal.IsCompleted and connecting items.
    /// </summary>
    public class LinkableItemController : IItemApi {
        readonly Dictionary<ILinkableItem, List<ILinkableItem>> _connectedItems;
        public ItemController ItemController { get; }
        public bool AutoConnect { get; set; }

        public LinkableItemController(ItemController itemController = null) {
            _connectedItems = new();
            ItemController = itemController ?? new();

            ItemController.Inventory.Added += (item) => {
                FixAutoConnections(item);
            };
        }

        public bool IsCompleted(IGoal goal) {
            if (goal.IsCompleted) return true;
            if (goal is IConnectionGoal connectionGoal) {
                return IsConnected(connectionGoal.From, connectionGoal.To) || IsConnected(connectionGoal.To, connectionGoal.From);
            } else if (goal is IFindItemGoal findGoal) {
                return ItemController.HasItem(findGoal.ItemToFind);
            } else {
                Debug.LogError($"ConnectionGoal or FindItemGoal required");
            }
            return false;
        }

        // IItemApi
        public void AddItem(IItem item) => ItemController.AddItem(item);
        public bool HasItem(IItem item) => ItemController.HasItem(item);

        /// <summary>
        /// Try connect two LinkableItems together. Will try both a->b and b->a.
        /// To connect an item to another, they must
        /// * exist in the item inventory
        /// * have a connection to eachother
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Was connection made?</returns>
        public bool Connect(ILinkableItem a, ILinkableItem b) {
            if (HasEstablishedConnection(a, b) || HasEstablishedConnection(b, a))
                return false;
            IConnection connection = a.ConnectionTo(b) ?? b.ConnectionTo(a);
            if (connection == null)
                return false;

            _connectedItems[a].Add(b);
            connection.Connected?.Invoke();
            return true;
        }

        bool HasEstablishedConnection(ILinkableItem from, ILinkableItem to) {
            if (!_connectedItems.ContainsKey(from)) {
                _connectedItems[from] = new();
            }
            return _connectedItems[from].Contains(to);
        }

        void FixAutoConnections(IItem itemBase) {
            ILinkableItem item = itemBase as ILinkableItem;
            if (item.Connections == null) {
                return;
            }

            foreach (IConnection connection in item.Connections) {
                if (AutoConnect || connection.AutoConnect) {
                    Connect(item, connection.To);
                }
            }
        }

        /// <summary>
        /// Is an item connected to another item. This connection can be multipla items away.
        /// ex: a -> b -> c -> d. IsConnected(a, d) => true
        /// Recursively follow all completed connections from 'from' and see if any leads to 'to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        bool IsConnected(ILinkableItem from, ILinkableItem to) {
            if (from is IItem fromItem && to is IItem toItem) {
                return ItemController.HasItem(fromItem) && ItemController.HasItem(toItem) && HasConnection(from, to, requireConnected: true);
            }
            return false;
        }

        bool connected(ILinkableItem from, ILinkableItem to) {
            if (!_connectedItems.ContainsKey(from))
                return false;
            return _connectedItems[from].Contains(to);
        }

        /// <summary>
        /// Recursively follow all completed connections from 'from' and see if any leads to 'to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="requireConnected"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        bool HasConnection(ILinkableItem from, ILinkableItem to, bool requireConnected = false, int depth = 10) {
            if (from == to) {
                return true;
            }
            if (depth <= 0 || from.Connections == null || from.Count == 0) {
                if (depth <= 0) {
                    Debug.LogWarning("Connection tree limited to 10 levels deep.");
                }
                return false;
            }

            foreach (IConnection link in from.Connections) {
                if (link.To == null) {
                    if (from is Object obj) {
                        Debug.LogWarning(obj.name + ": Target connection is empty.", obj);
                    }
                    break;
                }
                if ((!requireConnected || connected(from, link.To)) && (link.To == to || HasConnection(link.To, to, requireConnected, depth - 1)))
                    return true;
            }

            return false;
        }


        // Helpers
        public int CountLinks() {
            int cnt = 0;
            foreach (KeyValuePair<ILinkableItem, List<ILinkableItem>> pair in _connectedItems) {
                List<ILinkableItem> list = pair.Value;
                cnt += list.Count;
            }
            return cnt;
        }

        public void PrintConnectedItems() {
            Debug.Log("ConnectedItems:");
            foreach (KeyValuePair<ILinkableItem, List<ILinkableItem>> pair in _connectedItems) {
                Debug.Log($"{pair.Key} -> ");
                foreach (ILinkableItem toItem in pair.Value) {
                    Debug.Log($"  {toItem}");
                }
            }
        }
    }
}
