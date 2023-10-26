namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Модель автомобиля.
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// Идентификатор модели автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование модели.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Цвета, доступные для данной модели.
        /// </summary>
        public ICollection<CarColor> Colors { get; set; }
    }
}