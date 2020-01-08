using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Speakers.GetSpeakers
{
    public class GetSpeakerQuery : IRequest<IActionResult>
    {
        public int Id { get; set; }

        public GetSpeakerQuery(int id)
        {
            Id = id;
        }
    }
}