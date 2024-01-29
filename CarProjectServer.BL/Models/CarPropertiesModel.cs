namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Список свойств автомобилей.
    /// </summary>
    public class CarPropertiesModel
    {
        /// <summary>
        /// Список марок.
        /// </summary>
        public IEnumerable<BrandModel> Brands { get; set; }

        /// <summary>
        /// Список моделей.
        /// </summary>
        public IEnumerable<CarModelTypeModel> Models { get; set; }

        /// <summary>
        /// Список цветов.
        /// </summary>
        public IEnumerable<CarColorModel> Colors { get; set; }
    }
}
