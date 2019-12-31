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

            var eventToReturn =  _context.Events.Include(x => x.Attendees)
                                                     .Where(x => x.Id == request.Id
                                                     &&
                                                     x.Live)
                                               .Select(x =>  Tuple.Create(x,new GetEventLiveDetailQuery.Response
                                               {
                                                   Id = x.Id,
                                                   Title = x.Title,
                                                   Description = x.Description,
                                                   ImageUrl = x.ImageUrl,
                                                   Platform = x.Platform,
                                                   StartLiveTime = x.StartLiveTime,
                                                   GeneralAttended = x.GeneralAttended,
                                                   Attended = x.Attendees.Any(a => a.MemberId == _currentUser.User.Id
                                                                                    &&
                                                                                    a.Attended)
                                               }))
                                               .FirstOrDefault();

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
            eventToReturn.Item2.MembersDetails.MembersAttended = await _context.Attendances.Include(x=> x.Member).Where(x => x.EventId == eventToReturn.Item2.Id
                                                                                                &&
                                                                                                x.Attended)
                .OrderByDescending(x => x.AttendedTime)
                .Take(8)
                .Select(s => new GetEventLiveDetailQuery.Response.MemberDetail
                {
                    Id = s.Member.Id,
                    FirstName = s.Member.FirstName,
                    LastName = s.Member.LastName,
                    Picture = s.Member.Picture,
                    Username = s.Member.Username,
                    AttendedTime = s.AttendedTime
                }).ToListAsync();


            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn.Item2);
            return HttpResponseCodeHelper.Ok(eventToReturn.Item2);
        }
    }

}