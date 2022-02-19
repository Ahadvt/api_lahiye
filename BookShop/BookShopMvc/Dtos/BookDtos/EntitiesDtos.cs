using BookShopMvc.Dtos.AuthorDtos;
using BookShopMvc.Dtos.JanrDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.Dtos.BookDtos
{
    public class EntitiesDtos
    {
        public ListDtos<AuthorGetDtos> Authors { get; set; }
        public ListDtos<JanrGetDtos> Janrs { get; set; }
        public BookPostDtos postDtos { get; set; }
    }
}
