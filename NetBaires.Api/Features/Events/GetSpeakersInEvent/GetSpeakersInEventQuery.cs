using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetSpeakersInEvent
{
    public class GetSpeakersInEventQuery : IRequest<IActionResult>
    {
        public int? Id { get; set; }

        public GetSpeakersInEventQuery(int id)
        {
            Id = id;
        }

        public GetSpeakersInEventQuery()
        {
            
        }
    }
}