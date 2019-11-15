using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Me
{
    public class UpdateMeProfile : Profile
    {
        public UpdateMeProfile()
        {
            CreateMap<UpdateMeCommand, Member>();
        }
    }
}