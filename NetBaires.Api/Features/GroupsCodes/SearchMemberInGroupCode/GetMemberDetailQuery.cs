using System;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.SearchMember
{
    public class SearchMemberInGroupCodeQuery : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int GroupCode { get; set; }
        public string Query { get; set; }

        public SearchMemberInGroupCodeQuery(string query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }
    }
}