namespace NetBaires.Api.ViewModels
{
    public class EventToReportAttendanceViewModel
    {
        public EventDetailViewModel EventDetail { get; set; } = new EventDetailViewModel();
        public string Token { get; set; }
    }
}