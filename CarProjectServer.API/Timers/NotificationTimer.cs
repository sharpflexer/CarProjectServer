using CarProjectServer.BL.Services.Implementations;

namespace CarProjectServer.API.Timers
{
    public class NotificationTimer
    {
        public static NotificationTimer _instance;

        public EventHandler eventHandler;

        private Timer timer;
        private const string notifyString = "Технические работы!";

        public delegate Task NotifyHandler(string message);
        public event NotifyHandler Notify;

        private NotificationTimer() { }

        public static NotificationTimer GetInstance()
        {
            if (_instance == null) 
            {
                return new NotificationTimer();
            }

            return _instance;
        }

        public void StartTimer()
        {
            if (timer == null)
            {
                timer = new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            }
        }

        private void SendMessage(object state)
        {
            if (Notify != null)
            {
                Notify.Invoke(notifyString);
            }
        }
    }
}
