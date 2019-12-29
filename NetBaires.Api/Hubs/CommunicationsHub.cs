using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NetBaires.Api.Hubs
{
    public class CommunicationsHub : Hub
    {
        public async Task SendMessage(CommunicationMessage message)
        {
            await Clients.All.SendAsync(message.EventName, message.Data);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }

    public class CommunicationMessage
    {
        public string EventName { get; set; }
        public object Data { get; set; }
    }
}