using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.GetFollowingsFromMember
{
    public class GetFollowingsFromMemberQuery : IRequest<IActionResult>
    {
        public int Id { get; set; }

        public class Response
        {
            public MemberDetailViewModel Member { get; set; }
            public DateTime FollowingDate { get; set; }
        }
    }
}