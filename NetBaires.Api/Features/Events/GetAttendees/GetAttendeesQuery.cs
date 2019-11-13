using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetAttendeesQuery : IRequest<IActionResult>
    {

        public int Id { get; set; }

    }
}