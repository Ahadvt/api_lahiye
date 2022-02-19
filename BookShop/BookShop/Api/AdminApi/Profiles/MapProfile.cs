
using AutoMapper;
using BookShop.Api.AdminApi.Dtos;
using BookShop.Api.AdminApi.Dtos.BookDtos;
using BookShop.Api.AdminApi.Dtos.JanrDtos;
using BookShop.Data.Entetiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.AdminApi.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorGetDto>();
            CreateMap<Janr, JanrGetDto>();
            CreateMap<Book, BookGetDto>();
            
        }
    }
}
