using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public interface IAttendanceService
    {
        string GetTokenToReportMyAttendance(Event evenToReport);
        TokenToReportMyAttendance ValidateTokenToReportMyAttendance(string token);

        string GetTokenToReportGeneralAttendance(Event evenToReport);
        TokenToReportGeneralAttendance ValidateTokenToReportGeneralAttendance(string token);
    }
}