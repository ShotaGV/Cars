
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cars.Model
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarBrandId { get; set; }
        [JsonIgnore]
        public CarBrand CarBrand { get; set; }

    }
}
