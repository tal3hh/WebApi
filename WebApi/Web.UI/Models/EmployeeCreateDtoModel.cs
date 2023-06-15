using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class EmployeeCreateDtoModel
    {
        [Required(ErrorMessage ="Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Adress is required.")]
        public string? Adress { get; set; }
    }
}
