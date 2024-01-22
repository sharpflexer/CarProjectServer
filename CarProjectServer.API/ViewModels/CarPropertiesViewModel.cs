using CarProjectServer.API.Models;

namespace CarProjectServer.API.ViewModels
{
    /// <summary>
    /// Список свойств автомобилей.
    /// </summary>
    public class CarPropertiesViewModel
    {
        /// <summary>
        /// Список марок.
        /// </summary>
        public IEnumerable<BrandViewModel> Brands { get; set; }

        /// <summary>
        /// Список моделей.
        /// </summary>
        public IEnumerable<CarModelViewModel> Models { get; set; }

        /// <summary>
        /// Список цветов.
        /// </summary>
        public IEnumerable<CarColorViewModel> Colors { get; set; }
    }
}
