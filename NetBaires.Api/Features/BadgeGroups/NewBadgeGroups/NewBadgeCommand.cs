using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.BadgeGroups.NewBadgeGroups
{
    public class NewBadgeGroupCommand : IRequest<IActionResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public NewBadgeGroupCommand(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public NewBadgeGroupCommand()
        {
        }
    }
}