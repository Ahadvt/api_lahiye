using AutoMapper;
using BookShop.Api.AdminApi.Dtos;
using BookShop.Api.AdminApi.Dtos.AuthorDtos;
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

namespace BookShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public AuthorsController(BookDbContext context,IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var query = _context.Authors.Where(a=>a.DisplayStatus);

            ListDto<AuthorGetDto> listDto = new ListDto<AuthorGetDto>
            {
                Items = query.Select(c => new AuthorGetDto
                {
                    FullName=c.FullName,
                    Id=c.Id,
                    Image=c.Image
                }).ToList(),
                Totalcount=query.Count()
                
            };
            return Ok(listDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Author author = _context.Authors.FirstOrDefault(a => a.Id == id&&a.DisplayStatus);
            if (author == null) return NotFound();
            AuthorGetDto authorGetDto = _mapper.Map<AuthorGetDto>(author);
            return Ok(authorGetDto);
        }
        [HttpPost("")]
        public IActionResult Create([FromForm]AuthorPostDto authorPost)
        {
            
            Author author = new Author();

            if (authorPost.ImageFile!=null)
            {
               
                author.Image = authorPost.ImageFile.SaveImage(_env.WebRootPath, "assets/img");
            }
            author.FullName = authorPost.FullName;
            _context.Authors.Add(author);
            _context.SaveChanges();
            return StatusCode(201);
        }
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromForm]AuthorPostDto postDto)
        {
            Author ExsistAuthor = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (ExsistAuthor == null) return NotFound();
            if (postDto.ImageFile!=null)
            {
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/img", ExsistAuthor.Image);
                ExsistAuthor.Image = postDto.ImageFile.SaveImage(_env.WebRootPath, "assets/img");
            }
            ExsistAuthor.FullName = postDto.FullName;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Author author = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return NotFound();
            author.DisplayStatus = false;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
