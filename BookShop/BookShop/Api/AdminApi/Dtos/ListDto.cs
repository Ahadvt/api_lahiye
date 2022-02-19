using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Dtos
{
    public class ListDto<TItems>
    {
        public List<TItems> Items { get; set; }
        public int Totalcount { get; set; }
    }
}
