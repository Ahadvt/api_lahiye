using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.Dtos.JanrDtos
{
    public class ListDtos<TItems>
    {
        public List<TItems> Items { get; set; }
    }
}
