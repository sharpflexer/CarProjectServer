namespace CarProjectServer.API.Timers
{
    public class NotificationTimer
    {
        /// <summary>
        /// Экземпляр класса NotificationTimer.
        /// </summary>
        public static NotificationTimer _instance;

        /// <summary>
        /// Делегат для обработчика оповещений.
        /// </summary>
        /// <param name="message">Сообщение оповещения.</param>
        public delegate Task NotifyHandler(string message);

        /// <summary>
        /// Событие, возникающее при срабатывании таймера.
        /// Оповещает обработчики о начале технических работ.
        /// </summary>
        public event NotifyHandler Notify;

        /// <summary>
        /// Таймер, вызывающий метод-callback с заданной периодичностью.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Сообщение о технических работах.
        /// </summary>
        private const string notifyString = "Технические работы!";

        /// <summary>
        /// Приватный конструктор, обеспечивает инкапсуляцию 
        /// логики создания экземпляров класса.
        /// </summary>
        private NotificationTimer() { }

        /// <summary>
        /// Получает единственный экземпляр класса, 
        /// если он уже существует,
        /// либо создает новый, если его нет.
        /// </summary>
        /// <returns>Единственный экземпляр NotificationTimer.</returns>
        public static NotificationTimer GetInstance()
        {
            if (_instance == null)
            {
                return new NotificationTimer();
            }

            return _instance;
        }

        /// <summary>
        /// Начинает отсчет таймера.
        /// </summary>
        public void StartTimer()
        {
            if (timer == null)
            {
                timer = new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            }
        }

        /// <summary>
        /// Метод-callback, вызывающий событие, каждый раз,
        /// когда срабатывает таймер.
        /// </summary>
        /// <param name="state"></param>
        private void SendMessage(object state)
        {
            if (Notify != null)
            {
                Notify.Invoke(notifyString);
            }
        }
    }
}
