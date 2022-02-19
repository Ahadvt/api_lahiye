using BookShop.Exstention;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Dtos.BookDtos
{
    public class BookPostDto
    {
        public string Name { get; set; }
        public IFormFile Imagefile { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public int AuthorId { get; set; }
        public int JanrId { get; set; }
    }

    public class BookValidator : AbstractValidator<BookPostDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name).NotEmpty().MaximumLength(30).MinimumLength(4);
            RuleFor(b => b.Title).NotEmpty().MaximumLength(700).MinimumLength(15);
            RuleFor(b => b.Description).NotEmpty().MaximumLength(300).MinimumLength(30);
            RuleFor(b => b.SalePrice).NotNull().GreaterThan(0);
            RuleFor(b => b.CostPrice).NotNull().GreaterThan(0);
            RuleFor(b => b.AuthorId).NotNull();
            RuleFor(b => b.JanrId).NotNull();
            RuleFor(b => b).Custom((x, contex) =>
            {
                if (x.Imagefile!=null)
                {
                    if (!x.Imagefile.IsImage())
                    {
                        contex.AddFailure("file must be imagefile");
                    }
                    if (x.Imagefile.Length/1024/1024>2)
                    {
                        contex.AddFailure("image file  max size must be 2mb ");
                    }
                }
            });

            RuleFor(b => b).Custom((x, context) =>
            {
                if (x.SalePrice<x.CostPrice)
                {
                    context.AddFailure("SalePrise cannot be less than CostPrice");
                }
            });

        }
    }
}
