using AutoMapper;
using BookShop.Api.UserApi.Dtos;
using BookShop.Data.Entetiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Api.UserApi.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserGtetDto>();
        }
    }
}
