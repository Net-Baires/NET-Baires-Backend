using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakersQuery : IRequest<IActionResult>
    {
        public int? Id { get; set; }

        public GetSpeakersQuery(int id)
        {
            Id = id;
        }

        public GetSpeakersQuery()
        {
            
        }
    }
}