using AutoMapper;
using DomainLayer.Entities;
using ServiceLayer.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            //CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap().ForAllMembers(x => x.Condition((dest, src, obj) => obj != null));
        }
    }
}
