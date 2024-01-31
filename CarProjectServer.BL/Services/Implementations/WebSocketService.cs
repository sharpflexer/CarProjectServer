using CarProjectServer.BL.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CarProjectServer.BL.Services.Implementations
{
    public class WebSocketService : IWebSocketService 
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public void AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
        }

        public void RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out _);
        }

        public async Task SendAll(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            foreach (var wsKeyValue in _sockets)
            {
                if (wsKeyValue.Value.CloseStatus.HasValue)
                {
                    RemoveSocket(wsKeyValue.Key);
                }
                await wsKeyValue.Value.SendAsync(bytes,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
            }
        }

        public async Task SendAllExcept(string message, WebSocket ignoredSocket)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            foreach (WebSocket ws in _sockets.Values)
            {
                if(ws == ignoredSocket)
                {
                    continue;
                }

                await ws.SendAsync(bytes,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
            }
        }
    }
}
