using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.GroupsCodes.MakeRaffle
{
    public class MakeRaffleCommand : IRequest<IActionResult>
    {
        public int GroupCodeId { get; set; }
        public bool RepeatWinners { get; set; } = false;
        public int CountOfWinners { get; set; }
    }
}