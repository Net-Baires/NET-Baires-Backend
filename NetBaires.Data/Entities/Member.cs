using System;
using System.Collections.Generic;
using System.Linq;

namespace NetBaires.Data
{
    public class PushNotificationInformation : Entity
    {
        public string PushNotificationId { get; set; }
    }

    public class Member : Entity
    {
        public string Email { get; set; }
        public List<PushNotificationInformation> PushNotifications { get; set; } = new List<PushNotificationInformation>();
        public long MeetupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Twitter { get; set; }
        public string WorkPosition { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Github { get; set; }
        public string Biography { get; set; }
        public string Picture { get; set; }
        public string PictureName { get; set; }
        public bool Blocked { get; set; }
        public bool Organized { get; set; } = false;
        public bool Colaborator { get; set; } = false;
        public DateTime FirstLogin { get; protected set; } = DateTime.Now;
        public UserRole Role { get; set; } = UserRole.Member;
        public IList<BadgeMember> Badges { get; set; }
        public List<Attendance> Events { get; set; }
        public List<GroupCodeMember> GroupCodes { get; set; }
        public void SetFile(Uri uri, string fileName)
        {
            Picture = uri.AbsoluteUri;
            PictureName = fileName;
        }

        public void AddPushNotification(string pushNotificationId)
        {
            if (!PushNotifications.Any(a => a.PushNotificationId == pushNotificationId))
                PushNotifications.Add(new PushNotificationInformation { PushNotificationId = pushNotificationId });
        }
    }
}