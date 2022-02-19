using BookShop.Exstention;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Dtos.AuthorDtos
{
    public class AuthorPostDto
    {
        public string FullName { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class AuthorPotstDtoValidator : AbstractValidator<AuthorPostDto>
    {
        public AuthorPotstDtoValidator()
        {
            RuleFor(a => a.FullName).NotEmpty().WithMessage("author-un fulname-i bos ola bilmez").MinimumLength(4).MaximumLength(20);
            RuleFor(a => a).Custom((x, context) =>
            {

                if (x.ImageFile!=null)
                {
                    if (!x.ImageFile.IsImage())
                    {
                        context.AddFailure("image file olmalidir");
                    }
                    if (x.ImageFile.Length / 1024 / 1024 > 2)
                    {
                        context.AddFailure("image file max size must be 2mb");
                    }
                }
            });
         

        }
    }

}
