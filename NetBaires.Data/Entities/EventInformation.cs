namespace NetBaires.Data.Entities
{
    public class EventInformation : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }

        public EventInformation(string title, string description, bool visible)
        {
            Title = title;
            Description = description;
            Visible = visible;
        }
        public EventInformation()
        {

        }
    }
}