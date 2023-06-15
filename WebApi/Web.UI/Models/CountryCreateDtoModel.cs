using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class CountryCreateDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
    }
}
