﻿using AutoMapper;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.UpdateBadge
{
    public class UpdateBadgeProfile : Profile
    {
        public UpdateBadgeProfile()
        {
            CreateMap<UpdateBadgeCommand, Badge>().ForAllMembers(
                opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
        }
    }
}