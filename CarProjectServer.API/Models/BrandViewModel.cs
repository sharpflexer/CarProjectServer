namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Марка автомобиля.
    /// </summary>
    public class BrandViewModel
    {
        /// <summary>
        /// Идентификатор марки.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование марки авто.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Модели данной марки.
        /// </summary>
        public ICollection<CarModelViewModel> Models { get; set; }
    }
}