using FluentValidation;
using ServiceLayer.DTOs.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.FluentValidation.Country
{
    public class CountryDtoValidation : AbstractValidator<CountryDto>
    {
        public CountryDtoValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");
        }
    }
}
