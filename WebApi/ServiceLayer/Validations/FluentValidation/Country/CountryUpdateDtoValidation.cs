using FluentValidation;
using ServiceLayer.DTOs.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.FluentValidation.Country
{
    public class CountryUpdateDtoValidation : AbstractValidator<CountryUpdateDto>
    {
        public CountryUpdateDtoValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");
        }
    }
}
