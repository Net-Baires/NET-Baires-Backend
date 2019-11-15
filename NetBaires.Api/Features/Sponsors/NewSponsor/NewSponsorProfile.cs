using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class NewSponsorProfile : Profile
    {
        public NewSponsorProfile()
        {
            CreateMap<NewSponsorCommand, Sponsor>();
        }
    }
}