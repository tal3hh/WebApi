using FluentValidation;
using ServiceLayer.DTOs.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.FluentValidation.City
{
    public class CityDtoValidation : AbstractValidator<CityDto>
    {
        public CityDtoValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");

        }
    }
}
