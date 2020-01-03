using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEventLiveDetail
{
    public class GetEventLiveDetailHandler : IRequestHandler<GetEventLiveDetailQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IAttendanceService _attendanceService;
        private readonly NetBairesContext _context;

        public GetEventLiveDetailHandler(IMapper mapper,
            ICurrentUser currentUser,
            IAttendanceService attendanceService,
            NetBairesContext context)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _attendanceService = attendanceService;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetEventLiveDetailQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = _context.Events.Include(x => x.Attendees)
                .Include(x=> x.GroupCodes)
                .ThenInclude(x=> x.Members)
                .Where(x => x.Id == request.Id
                            &&
                            x.Live)
                .Select(x => Tuple.Create(x, _mapper.Map<GetEventLiveDetailQuery.Response>(x)))
                .FirstOrDefault();

            //TODO: Refactor
            if (_currentUser.User.Rol == UserRole.Member)
                eventToReturn.Item2.GroupCodes = null;
            eventToReturn.Item2.GeneralAttendance = eventToReturn.Item1.GeneralAttended &&
                                                    (_currentUser.User.Rol == UserRole.Admin
                                                     ||
                                                     _currentUser.User.Rol == UserRole.Organizer)
                ? new GetEventLiveDetailQuery.Response.ReportGeneralAttendance
                {
                    TokenToReportGeneralAttendance =
                        _attendanceService.GetTokenToReportGeneralAttendance(eventToReturn.Item1),
                    GeneralAttendedCode = eventToReturn.Item1.GeneralAttendedCode
                }
                : new GetEventLiveDetailQuery.Response.ReportGeneralAttendance();
            eventToReturn.Item2.TokenToReportMyAttendance = _attendanceService.GetTokenToReportMyAttendance(eventToReturn.Item1);


            if (eventToReturn.Item1 == null)
                return HttpResponseCodeHelper.NotContent();
            eventToReturn.Item2.MembersDetails = new GetEventLiveDetailQuery.Response.Members
            {
                TotalMembersRegistered = eventToReturn.Item1.Attendees.Count,
                TotalMembersAttended = eventToReturn.Item1.Attendees.Count(l => l.Attended)
            };
            eventToReturn.Item2.MembersDetails.MembersAttended = await _context.Attendances.Include(x => x.Member).Where(x => x.EventId == eventToReturn.Item2.Id
                                                                                                                              &&
                                                                                                                              x.Attended)
                .OrderByDescending(x => x.AttendedTime)
                .Take(8)
                .Select(s => _mapper.Map<GetEventLiveDetailQuery.Response.MemberDetail>(s.Member)).ToListAsync(cancellationToken: cancellationToken);


            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn.Item2);
            return HttpResponseCodeHelper.Ok(eventToReturn.Item2);
        }
    }
}