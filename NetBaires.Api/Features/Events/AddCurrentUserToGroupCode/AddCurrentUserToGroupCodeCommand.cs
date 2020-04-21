using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.AddCurrentUserToGroupCode
{
    public class AddCurrentUserToGroupCodeCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public string Code { get; set; }

        public class Response
        {
            public int Id { get; set; }
            public string Detail { get; set; }
        }
    }
}