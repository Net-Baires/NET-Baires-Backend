using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Sponsors
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