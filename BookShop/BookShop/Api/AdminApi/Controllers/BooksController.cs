using AutoMapper;
using BookShop.Api.AdminApi.Dtos;
using BookShop.Api.AdminApi.Dtos.BookDtos;
using BookShop.Data.Dal;
using BookShop.Data.Entetiy;
using BookShop.Exstention;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public BooksController(BookDbContext context,IWebHostEnvironment env,IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var query = _context.Books.Where(b => b.InStock&&b.InStock);

            ListDto<BookGetDto> listDto = new ListDto<BookGetDto>
            {
                Items=query.Select(x=> new BookGetDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    Title=x.Title,
                    Description=x.Description,
                    Image=x.Image,
                    SalePrice=x.SalePrice,
                    CostPrice=x.CostPrice,
                    AuthorId=x.AuthorId,
                    JanrId=x.JanrId,
                    PubilisDate=x.PubilisDate
                }).ToList(),
                Totalcount=query.Count()
            };
            return Ok(listDto);

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Book book = _context.Books.FirstOrDefault(b => b.Id == id && b.InStock);
            if (book == null) return NotFound();
            BookGetDto bookGetDto = _mapper.Map<BookGetDto>(book);
            return Ok(bookGetDto);

        }

        [HttpPost("")]
        public IActionResult Create([FromForm] BookPostDto bookPostDto)
        {
            if (_context.Authors.FirstOrDefault(a=>a.Id==bookPostDto.AuthorId&&a.DisplayStatus)==null)
            {
                return NotFound();
            }
            if (_context.Janrs.FirstOrDefault(j => j.Id == bookPostDto.JanrId && j.DisplayStatus) == null)
            {
                return NotFound();
            }
            Book book = BookMap(bookPostDto);
          

            _context.Books.Add(book);
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id,[FromForm]BookPostDto bookPost)
        {
            Book ExsistBook = _context.Books.FirstOrDefault(b => b.Id == id);
            if (ExsistBook == null) return NotFound();
            if (_context.Authors.FirstOrDefault(a => a.Id == bookPost.AuthorId && a.DisplayStatus) == null)
            {
                return NotFound();
            }
            if (_context.Janrs.FirstOrDefault(j => j.Id == bookPost.JanrId && j.DisplayStatus) == null)
            {
                return NotFound();
            }
            if (bookPost.Imagefile!=null)
            {
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img", ExsistBook.Image);
                ExsistBook.Image = bookPost.Imagefile.SaveImage(_env.WebRootPath, "assets/img");
            }
            ExsistBook.JanrId = bookPost.JanrId;
            ExsistBook.AuthorId = bookPost.AuthorId;
            ExsistBook.Name = bookPost.Name;
            ExsistBook.Title = bookPost.Title;
            ExsistBook.Description = bookPost.Description;
            ExsistBook.CostPrice = bookPost.CostPrice;
            ExsistBook.SalePrice = bookPost.SalePrice;
            _context.SaveChanges();
            return NoContent();
        }

        public Book BookMap(BookPostDto bookPostDto)
        {
            Book book = new Book();
            if (bookPostDto.Imagefile != null)
            {
                book.Image = bookPostDto.Imagefile.SaveImage(_env.WebRootPath, "assets/img");
            }
            book.JanrId = bookPostDto.JanrId;
            book.AuthorId = bookPostDto.AuthorId;
            book.Name = bookPostDto.Name;
            book.Title = bookPostDto.Title;
            book.Description = bookPostDto.Description;
            book.CostPrice = bookPostDto.CostPrice;
            book.SalePrice = bookPostDto.SalePrice;
            return book;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Book book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            book.InStock = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
