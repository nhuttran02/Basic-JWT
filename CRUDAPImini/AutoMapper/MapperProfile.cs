using CRUDAPImini.Models.DTOs;
using CRUDAPImini.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;

namespace CRUDAPImini.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AddRequestDTO, Product>();
            CreateMap<UpdateRequestDTO, Product>();
            CreateMap<Product, ResponseDTO>();
            CreateMap<RegisterDTO, User>();
        }
    }
}
