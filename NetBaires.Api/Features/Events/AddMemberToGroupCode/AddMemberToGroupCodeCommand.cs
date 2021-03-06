﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.AddMemberToGroupCode
{
    public class AddMemberToGroupCodeCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public int MemberId{ get; set; }
        public int GroupCodeId { get; set; }

        public class Response
        {
            public int Id { get; set; }
            public string Detail { get; set; }
        }
    }
}