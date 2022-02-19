using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.Dtos.AuthorDtos
{
    public class AuthorEditDtos
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
