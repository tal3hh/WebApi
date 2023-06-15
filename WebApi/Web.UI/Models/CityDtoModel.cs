using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Models
{
    public class CityDtoModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Population { get; set; }

        public int CountryId { get; set; }
    }
}
