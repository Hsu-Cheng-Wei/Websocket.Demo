//using Microsoft.AspNetCore.Http;
//using System.Net.WebSockets;
//using System.Threading.Tasks;

//namespace demo.websocket.SocketsManager
//{
//    public class SocketMiddleware
//    {
//        private readonly HttpContext _context;
//        private readonly RequestDelegate _next;
//        private readonly ConnectionManager _connections;

//        public SocketMiddleware(HttpContext context, RequestDelegate next, ConnectionManager connections)
//        {
//            _context = context;
//            _next = next;
//            _connections = connections;
//        }        

//        public async Task InvokeAsync()
//        {
//            if (!context.HttpContext.WebSockets.IsWebSocketRequest) return;

//            var socket = await _context.WebSockets.AcceptWebSocketAsync();

//            _connections.AddSocket(id, socket);

//            while (socket.State != WebSocketState.Closed) ;

//            await _connections.RemoveSocketAsync(id, socket, _context.RequestAborted);
//        }
//    }
//}
