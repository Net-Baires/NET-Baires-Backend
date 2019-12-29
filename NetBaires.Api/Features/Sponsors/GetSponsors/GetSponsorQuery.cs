using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Sponsors.GetSponsors
{
    public class GetSponsorsQuery : IRequest<IActionResult>
    {
        public int? Id { get; set; }

        public GetSponsorsQuery(int id)
        {
            Id = id;
        }

        public GetSponsorsQuery()
        {
        }
    }
}