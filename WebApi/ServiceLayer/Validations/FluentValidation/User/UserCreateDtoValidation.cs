using FluentValidation;
using ServiceLayer.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.FluentValidation.User
{
    public class UserCreateDtoValidation : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");

            RuleFor(x => x.Surname).NotNull().WithMessage("Surname daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");

            RuleFor(x => x.Adress).NotNull().WithMessage("Adress daxil edin.")
                .Length(1, 100).WithMessage("1-100 intervalinda simvol daxil edin.");
        }
    }
}
