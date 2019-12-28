using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{
    public class UpdateEventProfile : Profile
    {
        public UpdateEventProfile()
        {
            CreateMap<UpdateEventCommand, Event>()
            .ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            CreateMap<UpdateEventCommand.SponsorEvent, SponsorEvent>()
          .ForAllMembers(
              opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            
        }
    }
}