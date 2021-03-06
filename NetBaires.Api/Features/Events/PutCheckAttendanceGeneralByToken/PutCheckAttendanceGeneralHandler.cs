﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Auth.Tokens;
using NetBaires.Api.Helpers;
using NetBaires.Api.Models.ServicesResponse.Attendance;
using NetBaires.Api.Options;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.PutCheckAttendanceGeneralByToken
{

    public class PutCheckAttendanceGeneralHandler : IRequestHandler<PutCheckAttendanceGeneralCommand, IActionResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly NetBairesContext _context;
        private readonly AttendanceOptions _assistanceOptions;
        private readonly ILogger<PutCheckAttendanceGeneralHandler> _logger;

        public PutCheckAttendanceGeneralHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<PutCheckAttendanceGeneralHandler> logger)
        {
            _currentUser = currentUser;
            _context = context;
            _assistanceOptions = assistanceOptions.Value;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(PutCheckAttendanceGeneralCommand request, CancellationToken cancellationToken)
        {
            var response = TokenService.Validate<LoginToken>(_assistanceOptions.AskAttendanceSecret, request.Token);

            var eventToCheck = _context.Events.Any(x => x.Id == response.EventId
                                                       &&
                                                       x.GeneralAttended);
            if (!eventToCheck)
                return HttpResponseCodeHelper.NotFound();

            var memberId = _currentUser.User.Id;

            var eventToAdd = _context.Attendances.FirstOrDefault(x => x.EventId == response.EventId
                                                                      &&
                                                                      x.MemberId == memberId);
            if (eventToAdd == null)
            {
                eventToAdd = new Attendance(memberId, response.EventId, AttendanceRegisterType.CurrentEvent);
                await _context.Attendances.AddAsync(eventToAdd);
            }
            eventToAdd.Attend();
            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.Ok(new CheckAttendanceGeneralResponse(response.EventId));
        }
    }
}