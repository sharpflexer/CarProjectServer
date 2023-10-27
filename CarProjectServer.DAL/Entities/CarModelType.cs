namespace CarProjectServer.DAL.Models
{
    /// <summary>
    /// Модель автомобиля.
    /// </summary>
    public class CarModelType
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
        /// Цвета, доступные для данной модели. (Связь: многие-ко-многим)
        /// </summary>
        public ICollection<CarColor> Colors { get; set; }
    }
}