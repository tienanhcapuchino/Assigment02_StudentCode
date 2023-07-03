using AutoMapper;
using BussinessObject.Entities;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region product
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<ProductAddModel, Product>();
            #endregion
            CreateMap<User, UserProfileModel>().ReverseMap();
            #region order
            CreateMap<OrderAddModel, Order>();
            CreateMap<OrderDetailVM, OrderDetail>();
            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName)).ReverseMap();
            #endregion
        }
    }
}
