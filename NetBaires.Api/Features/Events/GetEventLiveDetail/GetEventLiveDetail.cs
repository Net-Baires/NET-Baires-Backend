using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetEventLiveDetailHandler : IRequestHandler<GetEventLiveDetailQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetEventLiveDetailHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetEventLiveDetailQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Events.Include(x => x.Attendees)
                                                     .Where(x => x.Id == request.Id
                                                     &&
                                                     x.Live)
                                               .Select(x => new GetEventLiveDetailQuery.Response
                                               {
                                                   Id = x.Id,
                                                   Title = x.Title,
                                                   Description = x.Description,
                                                   ImageUrl = x.ImageUrl,
                                                   Platform = x.Platform,
                                                   GeneralAttendance = new GetEventLiveDetailQuery.Response.ReportGeneralAttendance
                                                   {
                                                       TokenToReportGeneralAttendance = ""
                                                   },
                                                   MembersDetails = new GetEventLiveDetailQuery.Response.Members
                                                   {
                                                       TotalMembersRegistered = x.Attendees.Count,
                                                       TotalMembersAttended = x.Attendees.Count(x => x.Attended)
                                                   }
                                               })
                                               .FirstOrDefaultAsync();
            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotContent();

            eventToReturn.MembersDetails.MembersAttended = await _context.Attendances.Where(x => x.EventId == eventToReturn.Id
                                                                                                &&
                                                                                                x.Attended)
                                                                                    .Select(s => new GetEventLiveDetailQuery.Response.MemberDetail
                                                                                    {
                                                                                        Id = s.Member.Id,
                                                                                        FirstName = s.Member.FirstName,
                                                                                        LastName = s.Member.LastName,
                                                                                        Picture = s.Member.Picture,
                                                                                        Username = s.Member.Username
                                                                                    }).ToListAsync();


            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn);
            return HttpResponseCodeHelper.Ok(eventToReturn);
        }
    }

}