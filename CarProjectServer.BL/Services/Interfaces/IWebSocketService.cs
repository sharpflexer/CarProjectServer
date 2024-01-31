using System.Net.WebSockets;

namespace CarProjectServer.BL.Services.Interfaces
{
    public interface IWebSocketService
    {
        void AddSocket(WebSocket socket);

        void RemoveSocket(string id);

        Task SendAll(string message);

        Task SendAllExcept(string message, WebSocket ignoredSocket);
    }
}
