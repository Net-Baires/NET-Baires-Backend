using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetLinkEventLive
{
    public class GetLinkEventLiveHandler : IRequestHandler<GetLinkEventLiveQuery, IActionResult>
    {
        private readonly NetBairesContext _context;

        public GetLinkEventLiveHandler(NetBairesContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Handle(GetLinkEventLiveQuery request, CancellationToken cancellationToken)
        {
            var eventLink = await _context.Events.Cacheable()
                .Where(x => x.Live
                                && 
                                x.Online)
                .OrderByDescending(x=> x.EndLiveTime)
                .Select(x => x.OnlineLink)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (eventLink == null)
                return HttpResponseCodeHelper.NotFound();


            return HttpResponseCodeHelper.Ok(new GetLinkEventLiveQuery.Response(eventLink));
        }
    }
}