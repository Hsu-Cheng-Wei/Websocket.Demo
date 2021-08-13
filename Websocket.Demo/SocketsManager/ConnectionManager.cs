using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace demo.websocket.SocketsManager
{
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, WebSocket>> _connections;

        public ConnectionManager()
        {
            _connections = new ConcurrentDictionary<string, ConcurrentDictionary<int, WebSocket>>();
        }

        public IEnumerable<string> GetConnectionKeys()
        {
            return _connections.Keys;
        }


        public WebSocket[] GetSocketById(string id)
        {
            return _connections.FirstOrDefault(w => w.Key == id)
                .Value?
                .Values
                .ToArray();
        }

        public async Task RemoveSocketAsync(string id, WebSocket socket, CancellationToken cancellationToken)
        {
            if (socket == null) return;

            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket closed", cancellationToken);

            socket.Dispose();

            var isExist = _connections.TryGetValue(id, out var bag);

            if (!isExist) return;

            bag.TryRemove(socket.GetHashCode(), out _);

            if (!bag.IsEmpty) return;

            _connections.TryRemove(id, out _);
        }

        public void AddSocket(string id, WebSocket socket)
        {
            var bag = _connections.GetOrAdd(id, (i) => new ConcurrentDictionary<int, WebSocket>());

            bag.TryAdd(socket.GetHashCode(),socket);
        }
    }
}
