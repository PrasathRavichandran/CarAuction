using AutoMapper;
using Contracts;
using SearchServices.Models;

namespace SearchServices.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
    }
}
