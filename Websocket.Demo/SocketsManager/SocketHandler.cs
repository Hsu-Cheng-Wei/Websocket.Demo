//using System;
//using System.Net.WebSockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace demo.websocket.SocketsManager
//{
//    public abstract class SocketHandler
//    {
//        protected readonly ConnectionManager Connections;

//        protected SocketHandler(ConnectionManager connections)
//        {
//            Connections = connections;
//        }

//        public virtual async Task OnConnectedAsync(WebSocket socket, CancellationToken cancellationToken)
//        {
//            await Task.Run(() => Connections.AddSocket(socket), cancellationToken);
//        }

//        public virtual async Task OnDisconnectedAsync(WebSocket socket, CancellationToken cancellationToken)
//        {
//            await Connections.RemoveSocketAsync(Connections.GetId(socket), cancellationToken);
//        }

//        public async Task SendMessageAsync(WebSocket socket, string message, CancellationToken cancellationToken)
//        {
//            if (socket.State != WebSocketState.Open) return;

//            var encoding = Encoding.ASCII.GetBytes(message);

//            var arr = new ArraySegment<byte>(encoding, 0, message.Length);

//            await socket.SendAsync(arr, WebSocketMessageType.Text, true, cancellationToken);
//        }

//        public async Task SendMessageAsync(string id, string message, CancellationToken cancellationToken)
//        {
//            await SendMessageAsync(Connections.GetSocketById(id), message, cancellationToken);
//        }

//        public async Task SendMessageToAllAsync(string message, CancellationToken cancellationToken)
//        {
//            foreach (var socket in Connections.GetAllConnections())
//                await SendMessageAsync(socket, message, cancellationToken);
//        }

//        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer, CancellationToken cancellationToken);

//    }
//}
