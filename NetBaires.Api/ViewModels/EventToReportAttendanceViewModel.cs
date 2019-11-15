using NetBaires.Api.Features.Events.ViewModels;
using System;

namespace NetBaires.Api.Models
{
    public class EventToReportAttendanceViewModel
    {
        public EventDetailViewModel EventDetail { get; set; } = new EventDetailViewModel();
        public string Token { get; set; }
    }
}