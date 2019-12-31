using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.GetBadgesFromMember
{
    public class GetBadgesFromMemberQuery : IRequest<IActionResult>
    {
        public string Email { get; set; } = string.Empty;
        public int? Id { get; set; }

        public GetBadgesFromMemberQuery(int? id)
        {
            Id = id;
        }

        public GetBadgesFromMemberQuery(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}