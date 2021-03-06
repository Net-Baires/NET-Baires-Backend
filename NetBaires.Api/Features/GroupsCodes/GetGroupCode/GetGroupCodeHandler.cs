﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels.GroupCode;
using NetBaires.Data;

namespace NetBaires.Api.Features.GroupsCodes.GetGroupCode
{

    public class GetGroupCodeHandler : IRequestHandler<GetGroupCodeQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetGroupCodeHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetGroupCodeQuery request, CancellationToken cancellationToken)
        {

            var groupCode = await _context.GroupCodes.Include(x=> x.GroupCodeBadges)
                                                     .ThenInclude(x=> x.Badge)
                                                     .Include(x => x.Members)
                                                     .ThenInclude(x=> x.Member)
                                                     .Cacheable()
                                                     .FirstOrDefaultAsync(x => x.Id == request.GroupCodeId);

            if (groupCode == null)
                return HttpResponseCodeHelper.NotFound();

            return HttpResponseCodeHelper.Ok(_mapper.Map<GroupCodeFullDetailResponseViewModel>(groupCode));

        }
    }

}