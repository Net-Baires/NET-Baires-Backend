﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadges
{

    public class GetBadgesHandler : IRequestHandler<GetBadgeQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetBadgesHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetBadgeQuery request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.Badges.Where(x => (request.BadgeId != null ? x.Id == request.BadgeId : true))
                                                      .Cacheable()
                                                      .AsNoTracking()
                                                      .ToListAsync();

            if (!badgesFromUser.Any())
                return HttpResponseCodeHelper.NotContent();

            var badgeToReturn = _mapper.Map<List<BadgeDetailViewModel>>(badgesFromUser);

            if (request.BadgeId != null)
                return HttpResponseCodeHelper.Ok(badgeToReturn.First());
            else
                return HttpResponseCodeHelper.Ok(badgeToReturn);
        }
    }
}