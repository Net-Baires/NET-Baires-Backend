using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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