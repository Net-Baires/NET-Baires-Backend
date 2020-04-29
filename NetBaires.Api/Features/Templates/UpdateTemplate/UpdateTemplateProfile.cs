using AutoMapper;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Templates.UpdateTemplate
{
    public class UpdateTemplateProfile : Profile
    {
        public UpdateTemplateProfile()
        {
            CreateMap<UpdateTemplateCommand, Template>();
        }
    }
}