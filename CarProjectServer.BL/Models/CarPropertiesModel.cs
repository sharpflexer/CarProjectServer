using CarProjectServer.BL.Models;

namespace CarProjectServer.API.ViewModels
{
    public class CarPropertiesModel
    {
        public IEnumerable<BrandModel> Brands { get; set; }

        public IEnumerable<CarModelTypeModel> Models { get; set; }

        public IEnumerable<CarColorModel> Colors { get; set; }
    }
}
