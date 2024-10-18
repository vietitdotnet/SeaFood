using AutoMapper;
using WebProject.Dto;
using WebProject.Entites;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebProject.ATMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<Specification, SpecificationCreateDto>();
            CreateMap<SpecificationCreateDto, Specification>();
            CreateMap<Product, ProductDto>();

            CreateMap<ProductCreateDTO, Product>();
            CreateMap<Product, ProductCreateDTO>();

            CreateMap<ProductDetailDTO, Product>();
            CreateMap<Product, ProductDetailDTO>();

            CreateMap<ProductUpdateDTO, Product>();
            CreateMap<Product, ProductUpdateDTO>();


        }
    }
}