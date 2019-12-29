using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEventLiveDetail
{
    public class GetEventLiveDetailQuery : IRequest<IActionResult>
    {
        public GetEventLiveDetailQuery(int id)
        {
            Id = id;
        }

        public int? Id { get; set; }
        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public EventPlatform Platform { get; set; }
            public string ImageUrl { get; set; }
            public ReportGeneralAttendance GeneralAttendance { get; set; }
            public Members MembersDetails { get; set; }
            public DateTime? StartLiveTime { get; internal set; }
            public bool GeneralAttended { get; internal set; }

            public class ReportGeneralAttendance
            {
                public string TokenToReportGeneralAttendance { get; set; }
                public string GeneralAttendedCode { get; internal set; }
            }
            public class Members
            {
                public int TotalMembersRegistered { get; set; }
                public int TotalMembersAttended { get; set; }
                public List<MemberDetail> MembersAttended { get; set; }
            }
            public class MemberDetail
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Username { get; set; }
                public string Picture { get; set; }
                public DateTime AttendedTime { get;  set; }
            }
        }
    }
}