using System.Collections.Generic;

namespace NetBaires.Api.Options
{
    public class AttendanceOptions
    {
        public string ReportAttendanceSecret { get; set; }
        public string AskAttendanceSecret { get; set; }
    }

    public class CommonOptions
    {
        public string ApiUrl { get; set; }
        public string SiteUrl { get; set; }
    }
    public class CorsOptions
    {
        public List<string> Urls { get; set; }
    }

    public class BadgesOptions
    {
        public string ImageEndPoint { get; set; }
        public string DetailUrl { get; set; }
        public string PublicUrl { get; set; }

    }
}