namespace NetBaires.Api.Options
{
    public class AssistanceOptions
    {
        public string ReportAssistanceSecret { get; set; }
        public string AskAssistanceSecret { get; set; }
    }

    public class CommonOptions
    {
        public string ApiUrl { get; set; }
        public string SiteUrl { get; set; }
    }

    public class BadgesOptions
    {
        public string ImageEndPoint { get; set; }
        public string DetailUrl { get; set; }
        public string PublicUrl { get; set; }

    }
}