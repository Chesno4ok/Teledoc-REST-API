using AutoMapper;
using System;
using Teledoc_REST_API.Responses;
using Teledoc_REST_API.Templates;
using Teledoc_REST_API.Models;

namespace Teledoc_REST_API.Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Client, ClientTemplate>().ReverseMap();
            CreateMap<Client, ClientResponse>().ReverseMap();
            CreateMap<Founder, FounderTemplate>().ReverseMap();
            CreateMap<Founder, FounderResponse>().ReverseMap();
            CreateMap<AuthorizationTokenTemplate, AuthorizationToken>().ReverseMap();
        }
    }
}
