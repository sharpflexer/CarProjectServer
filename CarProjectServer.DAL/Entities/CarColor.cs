namespace CarProjectServer.DAL.Models
{
    /// <summary>
    /// Цвет автомобиля.
    /// </summary>
    public class CarColor
    {
        /// <summary>
        /// Идентификатор цвета.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование цвета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список моделей данного цвета. (Связь: многие-ко-многим)
        /// </summary>
        public ICollection<CarModelType> Models { get; set; }
    }
}