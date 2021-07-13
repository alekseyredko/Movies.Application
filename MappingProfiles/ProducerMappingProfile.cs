using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movies.Application.Models;
using Movies.Application.Models.Producer;
using Movies.Data.Models;
using Movies.Data.Results;

namespace Movies.Application.MappingProfiles
{
    public class ProducerMappingProfile: Profile
    {
        public ProducerMappingProfile()
        {
            CreateMap<Producer, ProducerResponse>();
            CreateMap<Result<Producer>, Result<ProducerResponse>>();

            CreateMap<ProducerRequest, Producer>();
            CreateMap<Producer, RegisterProducerResponse>();
            CreateMap<Result<Producer>, Result<RegisterProducerResponse>>();

            CreateMap<Result, Result<TokenResponse>>();
        }
    }
}
