using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class CountryDtoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string? Name { get; set; }
    }
}
