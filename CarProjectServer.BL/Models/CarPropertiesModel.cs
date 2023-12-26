using CarProjectServer.BL.Models;

namespace CarProjectServer.API.ViewModels
{
    public class CarPropertiesModel
    {
        public IEnumerable<BrandModel> brands { get; set; }

        public IEnumerable<CarModelTypeModel> models { get; set; }

        public IEnumerable<CarColorModel> colors { get; set; }
    }
}
