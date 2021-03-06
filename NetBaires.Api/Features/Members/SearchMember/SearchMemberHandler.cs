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

namespace NetBaires.Api.Features.Members.SearchMember
{

    public class SearchMemberHandler : IRequestHandler<SearchMemberQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public SearchMemberHandler(
            NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(SearchMemberQuery request, CancellationToken cancellationToken)
        {
            var members = _context.Members.Where(x => x.Email.Contains(request.Query)
                                                                       ||
                                                                        x.FirstName.Contains(request.Query)
                                                                       ||
                                                                        x.LastName.Contains(request.Query)
                                                                       ||
                                                                        x.MeetupId.ToString().Contains(request.Query))
                                               .Cacheable()
                                               .Take(10)
                                               .AsNoTracking();

            if (!members.Any())
                return HttpResponseCodeHelper.NotFound();

            return HttpResponseCodeHelper.Ok(_mapper.Map<List<MemberDetailViewModel>>(members));
        }
    }
}