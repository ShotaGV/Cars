using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cars.Model
{
    public class CarBrand
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public ICollection<CarModel> Models { get; set; }
    }
}
