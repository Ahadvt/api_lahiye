using AutoMapper;
using BookShop.Api.AdminApi.Dtos;
using BookShop.Api.AdminApi.Dtos.JanrDtos;
using BookShop.Data.Dal;
using BookShop.Data.Entetiy;
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
    public class JanrsController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public JanrsController(BookDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var query = _context.Janrs.Where(j => j.DisplayStatus);

            ListDto<JanrGetDto> listDto = new ListDto<JanrGetDto>
            {
                Items = query.Select(x => new JanrGetDto { 
                    Name=x.Name,
                    Id=x.Id
                }).ToList(),
                Totalcount=query.Count()
            };
            return Ok(listDto);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Janr janr = _context.Janrs.FirstOrDefault(j => j.Id == id);
            if (janr == null) return NotFound();
            JanrGetDto janrGetDto = _mapper.Map<JanrGetDto>(janr);
            return Ok(janrGetDto);
        }
        [HttpPost("")]
        public IActionResult Create(JanrPostDto janrPost)
        {
            Janr janr = new Janr
            {
               Name=janrPost.Name
            };
            _context.Janrs.Add(janr);
            _context.SaveChanges();
            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id ,JanrPostDto janrPost)
        {
            Janr ExsistJanr = _context.Janrs.FirstOrDefault(j => j.Id == id);
            if (ExsistJanr == null) return NotFound();
            ExsistJanr.Name = janrPost.Name;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Janr janr = _context.Janrs.FirstOrDefault(j => j.Id == id);
            if (janr == null) return NotFound();
            janr.DisplayStatus = false;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
