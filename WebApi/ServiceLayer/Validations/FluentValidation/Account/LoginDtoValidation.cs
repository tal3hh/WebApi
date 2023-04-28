using FluentValidation;
using ServiceLayer.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Validations.FluentValidation.Account
{
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Fullname daxil edin...")
                .EmailAddress().WithMessage("Email formatinda yazi daxil edin.");

            RuleFor(x => x.Password).NotNull().WithMessage("Fullname daxil edin...");
        }
    }
}
