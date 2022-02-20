using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.UserApi.Dtos
{
    public class RegisterDtos
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterValidator : AbstractValidator<RegisterDtos>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(8);
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.ConfirmPassword!=x.Password)
                {
                    context.AddFailure("ConfirmPassword", "please make sure your password match");
                }
            });
        }
    }
}
