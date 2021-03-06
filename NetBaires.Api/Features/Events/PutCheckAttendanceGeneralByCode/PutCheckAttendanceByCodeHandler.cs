﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.Models.ServicesResponse.Attendance;
using NetBaires.Api.Options;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.PutCheckAttendanceGeneralByCode
{

    public class PutCheckAttendanceByCodeHandler : IRequestHandler<PutCheckAttendanceByCodeCommand, IActionResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly NetBairesContext _context;
        private readonly AttendanceOptions _assistanceOptions;
        private readonly ILogger<PutCheckAttendanceByCodeHandler> _logger;

        public PutCheckAttendanceByCodeHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<PutCheckAttendanceByCodeHandler> logger)
        {
            _currentUser = currentUser;
            _context = context;
            _assistanceOptions = assistanceOptions.Value;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(PutCheckAttendanceByCodeCommand request, CancellationToken cancellationToken)
        {

            var eventToCheck = _context.Events.Any(x => x.Id == request.EventId
                                                       &&
                                                       x.GeneralAttended
                                                       &&
                                                       x.GeneralAttendedCode == request.Code);
            if (!eventToCheck)
                return HttpResponseCodeHelper.NotFound();

            var memberId = _currentUser.User.Id;

            var eventToAdd = _context.Attendances.FirstOrDefault(x => x.EventId == request.EventId
                                                                      &&
                                                                      x.MemberId == memberId);
            if (eventToAdd == null)
            {
                eventToAdd = new Attendance(memberId, request.EventId, AttendanceRegisterType.CurrentEvent);
                await _context.Attendances.AddAsync(eventToAdd);
            }
            eventToAdd.Attend();
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.Ok(new CheckAttendanceGeneralResponse(request.EventId));
        }
    }
}