using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models;
using Movies.Data.Models;
using Movies.Data.Results;

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

            CreateMap<Result<User>, Result<LoginUserResponse>>();
            CreateMap<Result<User>, Result<RegisterUserResponse>>();
        }
    }
}
