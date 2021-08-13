using demo.websocket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace demo.websocket.SocketsManager
{
    public static class SocketsExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<OrderManager>();

            return services;
        }

        public static Task SendMessage(this WebSocket socket, string message, CancellationToken cancellationToken)
        {
            return socket.SendAsync(new System.ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
