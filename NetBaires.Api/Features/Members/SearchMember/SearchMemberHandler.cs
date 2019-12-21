using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
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
            var members = _context.Members.Where(x => x.Email.ToUpper().Contains(request.Query.ToUpper())
                                                                        ||
                                                                        x.FirstName.ToUpper().Contains(request.Query.ToUpper())
                                                                        ||
                                                                        x.LastName.ToUpper().Contains(request.Query.ToUpper())
                                                                        ||
                                                                        x.MeetupId.ToString().ToUpper().Contains(request.Query.ToUpper()))
                                               .Take(10)
                                               .AsNoTracking();

            if (!members.Any())
                return HttpResponseCodeHelper.NotFound();

            return HttpResponseCodeHelper.Ok(_mapper.Map<List<MemberDetailViewModel>>(members));
        }
    }
}