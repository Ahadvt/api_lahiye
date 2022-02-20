using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.UserApi.Dtos
{
    public class LoginDtos
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginDtosValidator : AbstractValidator<LoginDtos>
    {
        public LoginDtosValidator()
        {
            RuleFor(c => c.UserName).NotEmpty().MaximumLength(20);
            RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
        }
    }
}
