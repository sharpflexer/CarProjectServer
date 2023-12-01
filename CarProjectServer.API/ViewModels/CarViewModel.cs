namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Автомобиль.
    /// </summary>
    public class CarViewModel
    {
        /// <summary>
        /// Идентификатор автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public BrandViewModel Brand { get; set; }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public CarModelViewModel Model { get; set; }

        /// <summary>
        /// Цвет автомобиля.
        /// </summary>
        public CarColorViewModel Color { get; set; }

        /// <summary>
        /// Цена автомобиля.
        /// </summary>
        public double Price { get; set; } = 0;
    }
}
