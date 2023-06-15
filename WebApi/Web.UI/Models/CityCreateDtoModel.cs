using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class CityCreateDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        public int Population { get; set; }

        public int CountryId { get; set; }
        public SelectList? Countries { get; set; }
    }
}
