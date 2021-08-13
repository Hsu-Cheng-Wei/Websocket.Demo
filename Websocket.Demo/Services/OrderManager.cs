using demo.websocket.SocketsManager;
using System;
using System.Collections.Concurrent;

namespace demo.websocket.Services
{
    public class OrderManager
    {
        private readonly ConcurrentDictionary<string, bool> _orders;

        public OrderManager(ConnectionManager socketsManager)
        {
            _orders = new ConcurrentDictionary<string, bool>();
        }

        public string NewOrder()
        {
            var id = Guid.NewGuid().ToString();

            _orders.TryAdd(id, false);

            return id;
        }

        public bool? GetState(string id)
        {
            var isExist = _orders.TryGetValue(id, out var val);

            if (!isExist) return null;

            return val;
        }

        public bool SetHasOrdered(string id)
        {
            var isExist = _orders.TryGetValue(id, out _);

            if (!isExist) return false;

            _orders[id] = true;

            return true;
        }
    }
}
