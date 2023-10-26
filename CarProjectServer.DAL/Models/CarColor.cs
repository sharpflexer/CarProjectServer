namespace CarProjectServer.API.Models
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
        /// Список моделей данного цвета.
        /// </summary>
        public ICollection<CarModel> Models { get; set; }
    }
}