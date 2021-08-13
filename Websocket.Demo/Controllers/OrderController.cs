using demo.websocket.Services;
using demo.websocket.SocketsManager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demo.websocket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderManager _managers;
        private readonly ConnectionManager _connections;

        public OrderController(OrderManager managers, ConnectionManager connections)
        {
            _managers = managers;
            _connections = connections;
        }

        [HttpGet]
        public string New()
        {
            return _managers.NewOrder();
        }

        [HttpGet("{id}")]
        public bool? GetState(string id)
        {
            return _managers.GetState(id);
        }

        [HttpPost]
        public async Task SetState([FromForm] string id)
        {
            if (!_managers.SetHasOrdered(id)) return;

            var sockets = _connections.GetSocketById(id);

            foreach (var socket in sockets)
                await socket.SendMessage("Has orderd", HttpContext.RequestAborted);
        }
    }
}
