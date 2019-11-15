using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class UpdateSponsorProfile : Profile
    {
        public UpdateSponsorProfile()
        {
            CreateMap<UpdateSponsorCommand, Sponsor>();
        }
    }
}