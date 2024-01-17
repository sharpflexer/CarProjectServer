using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace CarProjectServer.API.Controllers
{
    public class NotificationTimer
    {
        public static EventHandler eventHandler;

        private static Timer timer;
        private const string notifyString = "Технические работы!";

        public delegate Task NotifyHandler(string message);
        public static event NotifyHandler Notify;

        public static void StartTimer()
        {
            if (timer == null)
            {
                timer = new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            }
        }

        private static void SendMessage(object state)
        {
            if (Notify != null)
            {
                Notify.Invoke(notifyString);
            }
        }
    }
}
