using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.SearchMember
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