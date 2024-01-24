namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Технические работы.
    /// </summary>
    public class TechnicalWorkModel
    {
        /// <summary>
        /// Идентификатор технических работ.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Начало технических работ.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Конец технических работ.
        /// </summary>
        public DateTime End { get; set; }
    }
}