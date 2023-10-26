namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Автомобиль.
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// Идентификатор автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public BrandModel Brand { get; set; }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public CarModelModel Model { get; set; }

        /// <summary>
        /// Цвет автомобиля.
        /// </summary>
        public CarColorModel Color { get; set; }
    }
}
