using AutoMapper;
using ND_2023_12_19.DTOs;
using ND_2023_12_19.Entities;

namespace ND_2023_12_19.Profiles;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<ItemEntity, ItemRequest>();
        CreateMap<ItemRequest, ItemEntity>();
        CreateMap<ItemEntity, ItemResponse>();
        CreateMap<ItemResponse, ItemEntity>();
    }
}
