using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NetBaires.Api.Auth;
using NetBaires.Api.Auth.CustomsClaims;
using NetBaires.Data;

namespace NetBaires.Api.Hubs
{
    public class CommunicationsHub : Hub
    {
        public bool IsLoggued => Context.User.Claims.Any();
        public CurrentUserDto User
        {
            get
            {

                var email = Context.User.Claims.FirstOrDefault(x => x.Type.Contains(EnumClaims.Email.ToString().LowercaseFirst())).Value;
                var id = int.Parse(Context.User.Claims.FirstOrDefault(x => x.Type == EnumClaims.UserId.ToString().LowercaseFirst()).Value);
                var rol = Context.User.Claims.FirstOrDefault(x => x.Type.Contains(EnumClaims.Role.ToString().LowercaseFirst())).Value;
                return new CurrentUserDto(email, id, EnumExtensions.ParseEnum<UserRole>(rol));

            }
        }


        public async Task SendMessage(CommunicationMessage message)
        {

            await Clients.All.SendAsync(message.EventName, message.Data);
        }

        public override Task OnConnectedAsync()
        {
            if (IsLoggued)
            {
                UserHandler.Connected(Context.ConnectionId, User, Context.UserIdentifier);
                Clients.All.SendAsync("ConnectedMember", new ConnectionMessage(User.Id, UserHandler.ConnectedIds.Count()));
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (IsLoggued)
            {
                UserHandler.Disconnected(User.Id.ToString());
                Clients.All.SendAsync("DisconnectedMember", new ConnectionMessage(User.Id, UserHandler.ConnectedIds.Count()));
            }

            return base.OnDisconnectedAsync(exception);
        }
    }

    public class ConnectionMessage
    {
        public int MemberId { get; set; }
        public int TotalConnected { get; set; }

        public ConnectionMessage(int memberId, int totalConnected)
        {
            MemberId = memberId;
            TotalConnected = totalConnected;
        }
    }

    public static class UserHandler
    {
        public static ConcurrentDictionary<string, MemberInfo> ConnectedIds = new ConcurrentDictionary<string, MemberInfo>();

        public static void Connected(string connectionId, CurrentUserDto user, string userIdentifier)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                if (ConnectedIds.ContainsKey(user.Id.ToString()))
                    ConnectedIds.TryRemove(user.Id.ToString(), out var userOut);
                ConnectedIds.AddOrUpdate(user.Id.ToString(),(a)=>
                {
                    
                    return new MemberInfo(userIdentifier, connectionId, user);
                }, (s, info) => info);
            }
        }
        public static void Disconnected(string userId) => ConnectedIds.TryRemove(userId, out var userOut);
    }

    public class MemberInfo
    {
        public string ConnectionId { get; set; }
        public string UserIdentifier { get; set; }
        public CurrentUserDto User { get; set; }
        public MemberInfo(string userIdentifier, string connectionId, CurrentUserDto user)
        {
            UserIdentifier = userIdentifier;
            ConnectionId = connectionId;
            User = user;
        }



    }
    public class CommunicationMessage
    {
        public string EventName { get; set; }
        public object Data { get; set; }
    }
}