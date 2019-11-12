using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetLivesQuery : IRequest<IActionResult>
    {
    }
}