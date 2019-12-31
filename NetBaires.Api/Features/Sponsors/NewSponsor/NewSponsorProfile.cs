using AutoMapper;
using NetBaires.Data;

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