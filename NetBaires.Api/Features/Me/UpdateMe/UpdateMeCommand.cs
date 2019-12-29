using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Me.UpdateMe
{
    public class UpdateMeCommand : UpdateMeCommon, IRequest<IActionResult>
    {
        public IFormFile ImageFile { get; set; }

    }
}