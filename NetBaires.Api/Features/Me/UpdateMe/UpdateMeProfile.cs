using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Me.UpdateMe
{
    public class UpdateMeProfile : Profile
    {
        public UpdateMeProfile()
        {
            CreateMap<UpdateMeCommand, Member>();
        }
    }
}