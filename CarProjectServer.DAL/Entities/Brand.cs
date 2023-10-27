namespace CarProjectServer.DAL.Models
{
    /// <summary>
    /// Марка автомобиля.
    /// </summary>
    public class Brand
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
        public ICollection<CarModelType> Models { get; set; }
    }
}