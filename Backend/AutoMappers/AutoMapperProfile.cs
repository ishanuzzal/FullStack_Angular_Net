using AutoMapper;
using MyProject.Dtos;
using MyProject.Models;

namespace MyProject.AutoMappers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() { 
            CreateMap<Product, ProductCreateDtos>();
            CreateMap<Product, ProductCreateDtos>().ReverseMap();
            CreateMap<Product, ShowProductDtos>();


        }
    }
}
