namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Марка автомобиля.
    /// </summary>
    public class BrandModel
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
        public ICollection<CarModelModel> Models { get; set; }
    }
}