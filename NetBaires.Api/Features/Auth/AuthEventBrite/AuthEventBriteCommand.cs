using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Auth.AuthEventBrite
{
    public class AuthEventBriteCommand : IRequest<IActionResult>
    {
        [Required]
        public string Token { get; set; }
    }
}