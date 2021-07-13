using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models;
using Movies.Application.Models.Reviewer;
using Movies.Application.Models.User;
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
            CreateMap<Result<User>, Result<LoginUserResponse>>();

            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, RegisterUserResponse>();
            CreateMap<Result<User>, Result<RegisterUserResponse>>();


            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UpdateUserResponse>();
            CreateMap<Result<User>, Result<UpdateUserResponse>>();

            CreateMap<User, GetUserResponse>();
            CreateMap<Result<User>, Result<GetUserResponse>>();

            CreateMap<RegisterReviewerRequest, Reviewer>();
            CreateMap<Reviewer, RegisterReviewerResponse>();
            CreateMap<Result<Reviewer>, Result<RegisterReviewerResponse>>();

            CreateMap<ReviewerRequest, Reviewer>();
            CreateMap<Reviewer, ReviewerResponse>();
            CreateMap<Result<Reviewer>, Result<ReviewerResponse>>();
            CreateMap<Result<IEnumerable<Reviewer>>, Result<IEnumerable<ReviewerResponse>>>();

        }
    }
}
