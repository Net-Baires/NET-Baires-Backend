using System;
using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.ViewModels
{

    public class BadgeDetailViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }
        public string SimpleImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class BadgeDetailViewModelProfile : Profile
        {
            public BadgeDetailViewModelProfile()
            {
                CreateMap<BadgeMember, BadgeDetailViewModel>()
                    .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<Badge, BadgeDetailViewModel>()
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            }
        }  
    }
}
