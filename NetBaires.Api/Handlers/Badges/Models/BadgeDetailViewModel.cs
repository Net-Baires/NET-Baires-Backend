using System;
using System.Collections.Generic;
using AutoMapper;
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges.Models
{

    public class BadgeDetailViewModel
    {
        public int Id { get; set; }
        public string BadgeId { get; set; }
        public string BadgeUrl { get; set; }
        public DateTime Created { get; set; }
        public string BadgeImageUrl { get; set; }
        public string IssuerUrl { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class BadgeDetailViewModelProfile : Profile
        {
            public BadgeDetailViewModelProfile()
            {
                CreateMap<BadgeMember, BadgeDetailViewModel>()
                    .ForMember(dest => dest.BadgeImageUrl, opt => opt.MapFrom(src => src.BadgeUrl))
                    .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<Badge, BadgeDetailViewModel>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            }
        }
    }

}
