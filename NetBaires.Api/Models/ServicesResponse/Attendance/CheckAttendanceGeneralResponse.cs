using System;
using System.Collections.Generic;
using System.Text;

namespace NetBaires.Api.Models.ServicesResponse.Attendance
{
    public class CheckAttendanceGeneralResponse
    {
        public int EventId { get; set; }

        public CheckAttendanceGeneralResponse(int eventId)
        {
            EventId = eventId;
        }
    }
}
