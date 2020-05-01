namespace NetBaires.Data.Entities
{
    public class CompleteEvent
    {
        public bool ThanksSponsors { get; set; } = false;
        public bool ThanksSpeakers { get; set; } = false;
        public bool ThanksAttendees { get; set; } = false;
        public bool SendMaterialToAttendees { get; set; } = false;
        public bool GiveBadgeToAttendees { get; set; }
        public int BadgeId { get; set; }
    }
}