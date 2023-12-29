using Application.DTOs.Requests;
using Application.DTOs.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<ItemEntity, ItemRequest>().ReverseMap();
        CreateMap<ItemEntity, ItemResponse>().ReverseMap();
        CreateMap<UserEntity, UserRequest>().ReverseMap();
        CreateMap<UserEntity, UserResponse>().ReverseMap();
        CreateMap<UserCacheEntity, UserRequest>().ReverseMap();
        CreateMap<UserCacheEntity, UserCacheResponse>().ReverseMap();

        CreateMap<ShopEntity, ShopRequest>().ReverseMap();
        CreateMap<ShopEntity, ShopResponse>().ReverseMap();
        CreateMap<ShopEntity, ShopWithItemsResponse>().ReverseMap();

        CreateMap<OrderEntity, OrderRequest>().ReverseMap();
        CreateMap<OrderEntity, OrderResponse>().ReverseMap();
    }
}
