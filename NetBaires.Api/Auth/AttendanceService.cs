using System;
using System.Collections.Generic;
using JWT.Builder;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth.CustomsClaims;
using NetBaires.Api.Auth.Tokens;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ICurrentUser _currentUser;
        private readonly AttendanceOptions _attendanceOptions;

        public AttendanceService(ICurrentUser currentUser,
            IOptions<AttendanceOptions> attendanceOptions)
        {
            _currentUser = currentUser;
            _attendanceOptions = attendanceOptions.Value;
        }
        public string GetTokenToReportMyAttendance(Event evenToReport)=>
            TokenService.Generate(_attendanceOptions.ReportAttendanceSecret, new List<CustomClaim>
            {
                new CustomClaim(EnumClaims.UserId.ToString(), _currentUser.User.Id.ToString()),
                new CustomClaim(EnumClaims.EventId.ToString(), evenToReport.Id.ToString())
            }, DateTime.Now.ToUniversalTime().AddMinutes(5));
        
        public string GetTokenToReportGeneralAttendance(Event evenToReport)=>
            TokenService.Generate(_attendanceOptions.ReportAttendanceSecret, new List<CustomClaim>
            {
                new CustomClaim(EnumClaims.EventId.ToString(), evenToReport.Id.ToString())
            }, DateTime.Now.ToUniversalTime().AddMinutes(5));

        public TokenToReportMyAttendance ValidateTokenToReportMyAttendance(string token) =>
            TokenService.Validate<TokenToReportMyAttendance>(_attendanceOptions.ReportAttendanceSecret, token);

        public TokenToReportGeneralAttendance ValidateTokenToReportGeneralAttendance(string token) =>
            TokenService.Validate<TokenToReportGeneralAttendance>(_attendanceOptions.ReportAttendanceSecret, token);
    }
}