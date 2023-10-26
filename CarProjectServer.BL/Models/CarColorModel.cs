namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Цвет автомобиля.
    /// </summary>
    public class CarColorModel
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
        public ICollection<CarModelModel> Models { get; set; }
    }
}