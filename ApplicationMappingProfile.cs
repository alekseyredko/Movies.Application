using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models;
using Movies.Data.Models;

namespace Movies.Application
{
    public class ApplicationMappingProfile: Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<LoginUserRequest, User>();
            CreateMap<User, LoginUserResponse>();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, RegisterUserResponse>();
        }
    }
}
