using AutoMapper;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.EventInformation.UpdateEventInformation
{
    public class UpdateEventInformationProfile : Profile
    {
        public UpdateEventInformationProfile()
        {
            CreateMap<UpdateEventInformationCommand, Data.Entities.EventInformation>();
        }
    }
}