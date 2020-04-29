namespace NetBaires.Data.Entities
{
    public class Material : Entity
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public Event Event { get; set; }
        public int EventId { get; set; }

        public Material(string title, string link)
        {
            Title = title;
            Link = link;
        }
        public Material()
        {

        }
    }
}