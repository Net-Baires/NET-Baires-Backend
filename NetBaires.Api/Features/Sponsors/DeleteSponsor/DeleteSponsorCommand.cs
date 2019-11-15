using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class DeleteSponsorCommand :  IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}