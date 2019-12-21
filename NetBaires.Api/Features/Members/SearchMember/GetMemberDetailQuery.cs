using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class SearchMemberQuery : IRequest<IActionResult>
    {
        public string Query { get; set; }

        public SearchMemberQuery(string query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}