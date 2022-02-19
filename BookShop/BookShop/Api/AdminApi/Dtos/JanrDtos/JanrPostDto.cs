using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Dtos.JanrDtos
{
    public class JanrPostDto
    {
        public string Name { get; set; }
    }

    public class JanrValidation : AbstractValidator<JanrPostDto>
    {
        public JanrValidation()
        {
            RuleFor(j => j.Name).NotEmpty().WithMessage("Janir nameni bos ola bilmez").MinimumLength(3).MaximumLength(30);
        }
    }
}
