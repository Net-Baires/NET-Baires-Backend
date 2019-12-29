using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Sponsors.DeleteSponsor
{
    public class DeleteSponsorCommand :  IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}