using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
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
            var eventToReturn = _context.Events
                .Include(x => x.GroupCodes)
                .ThenInclude(x => x.Members)
                .Where(x => x.Id == request.Id
                            &&
                            x.Live)
                .Cacheable()
                .FirstOrDefault();

            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotFound();

            var eventViewModel = _mapper.Map<GetEventLiveDetailQuery.Response>(eventToReturn);
            if (eventToReturn == null)
                return HttpResponseCodeHelper.NotFound();
            if (_currentUser.IsLoggued)
            {
                //TODO: Refactor
                if (_currentUser.User.Rol == UserRole.Member)
                    eventViewModel.GroupCodes = null;
                eventViewModel.GeneralAttendance = eventToReturn.GeneralAttended &&
                                                        (_currentUser.User.Rol == UserRole.Admin
                                                         ||
                                                         _currentUser.User.Rol == UserRole.Organizer)
                    ? new GetEventLiveDetailQuery.Response.ReportGeneralAttendance
                    {
                        TokenToReportGeneralAttendance =
                            _attendanceService.GetTokenToReportGeneralAttendance(eventToReturn),
                        GeneralAttendedCode = eventToReturn.GeneralAttendedCode
                    }
                    : new GetEventLiveDetailQuery.Response.ReportGeneralAttendance();
                eventViewModel.TokenToReportMyAttendance = _attendanceService.GetTokenToReportMyAttendance(eventToReturn);


                if (eventToReturn == null)
                    return HttpResponseCodeHelper.NotContent();
                var countAttended = _context.Events.Where(x => x.Id == request.Id)
                    .Select(s => Tuple.Create(s.Attendees.Count, s.Attendees.Count(k => k.Attended)))
                    .FirstOrDefault();

                eventViewModel.Attended = await _context.Attendances.AnyAsync(a => a.EventId == request.Id
                                                                                   &&
                                                                                   a.MemberId == _currentUser.User.Id
                                                                                   &&
                                                                                   a.Attended);
                eventViewModel.MembersDetails = new GetEventLiveDetailQuery.Response.Members
                {
                    TotalMembersRegistered = countAttended.Item1,
                    TotalMembersAttended = countAttended.Item2,
                    EstimatedAttendancePercentage = eventToReturn.EstimatedAttendancePercentage
                };
                eventViewModel.MembersDetails.MembersAttended = await _context.Attendances.Include(x => x.Member).Where(x => x.EventId == eventViewModel.Id
                                                                                                                                  &&
                                                                                                                                  x.Attended)
                    .OrderByDescending(x => x.AttendedTime)
                    .Take(8)
                    .Select(s => _mapper.Map<GetEventLiveDetailQuery.Response.MemberDetail>(s.Member)).ToListAsync(cancellationToken: cancellationToken);

            }
            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventViewModel);
            return HttpResponseCodeHelper.Ok(eventViewModel);
        }
    }
}