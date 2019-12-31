using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Sponsors.UpdateSponsor
{
    public class UpdateSponsorProfile : Profile
    {
        public UpdateSponsorProfile()
        {
            CreateMap<UpdateSponsorCommand, Sponsor>();
        }
    }
}