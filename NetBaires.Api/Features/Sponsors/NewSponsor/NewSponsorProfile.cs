using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Sponsors.NewSponsor
{
    public class NewSponsorProfile : Profile
    {
        public NewSponsorProfile()
        {
            CreateMap<NewSponsorCommand, Sponsor>();
        }
    }
}