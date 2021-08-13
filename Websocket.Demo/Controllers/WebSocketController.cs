using demo.websocket.Services;
using demo.websocket.SocketsManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace demo.websocket.Controllers
{
    [ApiController]
    //[MiddlewareFilter(typeof(SocketMiddleware))]
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly ConnectionManager _connections;
        private readonly OrderManager _orderManager;

        public WebSocketController(ConnectionManager connections, OrderManager orderManagers)
        {
            _connections = connections;
            _orderManager = orderManagers;
        }

        [HttpGet("{id}")]
        public async Task Get(string id)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
                return;

            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            var state = new OrderController(_orderManager, _connections).GetState(id);

            if (state == null)
            {
                await socket.SendMessage("Order doesn't exist.", HttpContext.RequestAborted);
                await _connections.RemoveSocketAsync(id, socket, HttpContext.RequestAborted);

                return;
            }

            if(state.Value)
            {
                await socket.SendMessage("Order has finished", HttpContext.RequestAborted);
                await _connections.RemoveSocketAsync(id, socket, HttpContext.RequestAborted);

                return;
            }

            await socket.SendMessage("Order doesn't finish", HttpContext.RequestAborted);
            _connections.AddSocket(id, socket);

            await Echo(id, socket);
        }

        private async Task Echo(string id, WebSocket socket)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), HttpContext.RequestAborted);

                if(result.MessageType == WebSocketMessageType.Close)
                {
                    await _connections.RemoveSocketAsync(id, socket, HttpContext.RequestAborted);
                    return;
                }
            }

        }

    }
}
