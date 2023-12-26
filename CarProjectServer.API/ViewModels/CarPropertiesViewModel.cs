using CarProjectServer.API.Models;
using System.Collections.Generic;

namespace CarProjectServer.API.ViewModels
{
    public class CarPropertiesViewModel
    {
        public IEnumerable<BrandViewModel> brands { get; set; }

        public IEnumerable<CarModelViewModel> models { get; set; }

        public IEnumerable<CarColorViewModel> colors { get; set; }
    }
}
