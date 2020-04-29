using AutoMapper;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Templates.NewTemplate
{
    public class NewTemplateProfile : Profile
    {
        public NewTemplateProfile()
        {
            CreateMap<NewTemplateCommand, Template>();
        }
    }
}