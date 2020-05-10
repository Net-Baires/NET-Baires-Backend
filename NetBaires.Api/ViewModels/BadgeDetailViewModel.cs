using System;
using AutoMapper;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.ViewModels
{
    public class BadgeMemberViewModel
    {
        public BadgeDetailViewModel Badge { get; set; }
        public DateTime AssignmentDate { get; set; }
    }

    public class BadgeDetailViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }
        public string SimpleImageUrl { get; set; }
        public string LinkedinImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class BadgeDetailViewModelProfile : Profile
        {
            public BadgeDetailViewModelProfile()
            {
                CreateMap<BadgeMember, BadgeMemberViewModel>()
                    .ForMember(dest => dest.Badge, opt => opt.MapFrom(src => src.Badge))
                    .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<Badge, BadgeDetailViewModel>()
                    .ForAllMembers(
                        opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<GroupCodeBadge, BadgeDetailViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Badge.Id))
                    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Badge.ImageUrl))
                    .ForMember(dest => dest.SimpleImageUrl, opt => opt.MapFrom(src => src.Badge.SimpleImageUrl))
                    .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Badge.Created))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Badge.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Badge.Description))

                    .ForAllMembers(
                        opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));

            }
        }  
    }
}
