using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

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