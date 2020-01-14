using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.UpdateEvent
{
    public class UpdateEventProfile : Profile
    {
        public UpdateEventProfile()
        {
            CreateMap<UpdateEventCommand, Event>()
                .ForMember(x=> x.Live, opt=> opt.Ignore())
                .ForMember(x=> x.GeneralAttended, opt=> opt.Ignore())
                .ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<SponsorEventViewModel, SponsorEventViewModel>()
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));


        }
    }
}